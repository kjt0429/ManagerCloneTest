using UnityEditor;
using hive.manager.editor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Globalization;

namespace hive.manager
{
    public static class HIVEManagerException {
        public static System.Exception CanNotFoundCurrentVersionCode() {
            //현제 버전을 인식할 수 없음.
            return new System.Exception("Can't get current version metadata. check your current version.");
        }

        public static System.Exception SameVersionInstalled() {
            return new System.Exception("Can't install. Already installed same version HIVE SDK.");
        }
    }
    public partial class HIVEManager {
        [SerializeField]
        List<AvailableDownload> availableDownloadList = new List<AvailableDownload>();
        [SerializeField]
        List<AvailableNotice> availableNoticeList = new List<AvailableNotice>();
        [SerializeField]
        VersionInfoFromHash currentVersionInfo;

        static HIVEIgnoreManager ignoreManager = HIVEIgnoreManager.Instance;

        static readonly ulong PATCH_AVAILABLE_FREE_DISK_SIZE = 2147483648;

        //Version HASH Generator;
        static readonly string versionHashIOSPath = Path.Combine(assetsPath(), "Hive_SDK_v4/Plugins/iOS/framework/HIVE_SDK.framework/HIVE_SDK".ConvertUnixPathToOsPath());
        static readonly string versionHashAndroidPath = Path.Combine(assetsPath(), "Hive_SDK_v4/Plugins/Android/libs/HIVE_SDK.aar".ConvertUnixPathToOsPath());
        //최신 버전의 HIVE SDK 부터는 미리 계산된 HASH파일키를 갖고있습니다. (매니저 배포 이후 버전)
        static readonly string versionHashCashedPath = Path.Combine(assetsPath(), "Hive_SDK_v4/Editor/VersionHash.txt".ConvertUnixPathToOsPath());

        static VersionMetadata ignoreChangeMetadata(VersionMetadata metadata) {
            ignoreManager.loadIgnore();
            List<VersionMetadata.Files> ignorePath = new List<VersionMetadata.Files>();
            for (int i=0;i<metadata.files.Count;++i) {
                //메타파일의 경로는 Assets의 상대경로로 넣어줘야한다.
                if(ignoreManager.isIgnore(metadata.files[i].path)){
                    ignorePath.Add(metadata.files[i]);
                }
            }
            
            for (int i=0;i<ignorePath.Count;++i) {
                metadata.files.Remove(ignorePath[i]);
            }

            return metadata;
        }
        
        public void requestNewNotice(System.Action<AvailableNotice> callback) {            
            requestNotices((List<AvailableNotice> noticeData, System.Exception e)=>{
                if (e != null) {
                    throw e;
                }
                noticeData.Sort((a, b) => -1* a.CompareTo(b));
                var latest = getLatestNotice(noticeData);
                if (isSkipNotice(latest)) {
                    latest = null;
                }
                callback(latest);
            });
        }

        public void updateCurrentVersionHash(System.Action<VersionInfoFromHash> callback) {
            if (isInstallSDK()) {
                requestGetVersion(getVersionHash(), (versionInfo, e)=> {
                    if (e != null) {
                        callback(null);
                        throw e;
                    }
                    currentVersionInfo = versionInfo;
                    if (callback != null) {
                        callback(currentVersionInfo);
                    }
                });
            } else {
                if (callback != null) {
                    callback(null);
                }
            }
        }

        public void connectHIVESDKManager(System.Action<bool> callback) {
            updateCurrentVersionHash((info)=>{
                if(callback != null)
                    callback(info != null);
            });
        }

        public void requestAvailableDownload(System.Action<List<AvailableDownload>> callback) {
            requestVersions((List<AvailableDownload> downloadData, System.Exception e)=>{
                if (e != null) {
                    callback(null);
                    throw e;
                }
                availableDownloadList = downloadData;
                availableDownloadList.Sort((a, b) => -1* a.CompareTo(b));
                callback(availableDownloadList);
            });
        }
        public void requestUpdateListViewData(System.Action<List<HIVESDKUpdateListViewData>> callback) {
            requestAvailableDownload((downloads)=>{
                if (callback != null) {
                    if (downloads == null) {
                        callback(null);
                        return;
                    }
                    callback(getUpdateListViewDatas());
                }
            });
        }

        public void requestNoticeListViewData(System.Action<List<HIVENoticeContents>> callback) {
            requestNoticesAll((List<AvailableNotice> noticeData, System.Exception e)=>{
                if (e != null) {
                    callback(null);
                    throw e;
                }
                availableNoticeList = noticeData;
                availableNoticeList.Sort((a, b) => -1* a.CompareTo(b));
                if (callback != null) {
                    callback(getNoticesContents());
                }
            });
        }

        static AvailableNotice getLatestNotice(List<AvailableNotice> availableNoticeList) {
            //get latest version
            AvailableNotice latestNotice = null;
            if (availableNoticeList == null || availableNoticeList.Count == 0) {
                return null;
            }
            latestNotice = availableNoticeList[0];
            for (int i=1; i<availableNoticeList.Count; ++i) {
                if (availableNoticeList[i].isActivation) {
                    latestNotice = latestNotice.registrationDateTime < availableNoticeList[i].registrationDateTime ? availableNoticeList[i] : latestNotice;
                }
            }
            return latestNotice;
        }

        #region check and set one day skip

        static string getProjectIdentity(string key) {
            return Application.companyName+"."+Application.productName+"."+key;
        }

        static bool isSkipNotice(AvailableNotice notice) {
            if (notice == null) {
                return true;
            }
            return EditorPrefs.GetBool(getProjectIdentity(notice.id),false);
        }

        public static void setDontShowNotice(string notice_id) {
            EditorPrefs.SetBool(getProjectIdentity(notice_id),true);
        }
        #endregion

        #region verification sdk
        /// <summary>
        /// SDK가 유효한지 검사합니다.
        /// </summary>
        /// <param name="callback">유효성 검사에 실패한 이유를 반환합니다. 성공할시 리스트는 비어있습니다.</param>
        public void integrityVerification(System.Action<List<string>> callback) {
            beginProcessWorking("sdk verification check");
            if (getCurrentVersion() == null) {
                endProcessWorking();
                throw HIVEManagerException.CanNotFoundCurrentVersionCode();
            }
            requestVersionPatchDataAndIgnore(getCurrentVersion().versionCode, (metadata,e)=> {
                if (e != null) {
                    endProcessWorking();
                    Debug.LogError(e);
                    throw e;
                }
                List<string> needVerificationCheckList = new List<string>();
                int fileCount = 0;
                foreach (var file in metadata.files) {
                    updateProcessWorking(file.path,(float)(fileCount++)/(float)metadata.files.Count);
                    var path = makeAbsoluteAssetsPath(file.path);
                    
                    if (CheckFileValidation(path, file.fileHash) == false) {
                        // 파일 해시가 달라졌다면
                        needVerificationCheckList.Add(file.path);
                    }
                }
                endProcessWorking();
                callback(needVerificationCheckList);
            });
            //VersionMetadata metadata = null;
            //check local data
            //check network data
        }

        /// <summary>
        /// 파일이 유효한지 검사합니다.
        /// </summary>
        /// <param name="FilePath">검사할 파일의 경로입니다.</param>
        /// <param name="validHash">정상파일의 HASH값 입니다.</param>
        /// <returns></returns>
        static bool CheckFileValidation(string FilePath, string validHash) {
            if (Path.GetExtension(FilePath).ToLower() == ".meta") {
                //meta파일은 유니티 버전에 따라 달라질수있고 필요에 따라 수정가능하므로 유효성 검증에서 제외한다.
                return true;
            }
            if (!File.Exists(FilePath)) return false;
            return getHashFromFile(FilePath).ToLower() == validHash.ToLower(); 
        }
        #endregion

        #region DISK SIZE available check
        public static string getNotAvailableDiskSizeErrorMessage() {
            var bytetoGB = PATCH_AVAILABLE_FREE_DISK_SIZE/System.Math.Pow(1024,3);
            return "Need at least "+bytetoGB+"GB of disk space for the patch.("+Application.temporaryCachePath+")";
        }
        public static bool isAvilableDiskSize() {
            var freeDisk = getFreeDiskByteFromPath(Application.temporaryCachePath);
            if (freeDisk <= PATCH_AVAILABLE_FREE_DISK_SIZE) {
                return false;
            }
            return true;
        }
        #endregion

        #region patch version

        /// <summary>
        /// 다운로드 및 버전패치를 수행합니다.
        /// </summary>
        /// <param name="download">다운로드 데이터</param>
        /// <param name="callback">패치전 SDK검증과정에서 검증에 실패시 검증에 실패한 목록을 돌려줍니다.</param>
        public void processDownloadAndPatch(AvailableDownload download, System.Action<List<string>,System.Exception> callback) {
            if (download.isActivation == true) { //Check Activation
                try {
                    if (isAvilableDiskSize() == false) {
                        callback(null, new System.Exception(getNotAvailableDiskSizeErrorMessage()));
                        return;
                    }
                    if (isInstallSDK()) {
                        // check integrityVerification
                        integrityVerification((List<string> status)=>{
                            if (status != null && status.Count > 0) {
                                callback(status, null);
                            } else {
                                //patch new version
                                installNewVersion(download.versionCode,(e)=>{
                                    callback(null,e);
                                });
                            }
                        });
                    } else {
                        //patch new version
                        installNewVersion(download.versionCode,(e)=>{
                            callback(null,e);
                        });
                    }
                } catch (System.Exception e) {
                    throw e;   
                }
            }
        }

        /// <summary>
        /// 새로운 버전을 설치합니다.
        /// </summary>
        /// <param name="version">설치하고자 하는 버전</param>
        void installNewVersion(HIVESemanticVersion version, System.Action<System.Exception> callback) {
            try {
                if (isInstallSDK()) {
                    if (getCurrentVersion() == null) {
                        //Debug.LogAssertion("Can't get current version metadata");
                        callback(HIVEManagerException.CanNotFoundCurrentVersionCode());
                        return;
                    }
                    if (version == getCurrentVersion().versionCode) {
                        //Debug.LogAssertion("Can't install same version installed sdk.");
                        callback(HIVEManagerException.SameVersionInstalled());
                        return;
                    }
                    //Request Patch Version
                    requestVersionPatchDataAndIgnore(getCurrentVersion().versionCode,version,(data, e)=>{
                        if (e != null) {
                            callback(e);
                            return;
                        }
                        EditorCoroutine.start(applyPath(data,callback));
                    });
                } else {
                    //Install New Install
                    requestVersionPatchDataAndIgnore(version,(data, e)=>{
                        if (e != null) {
                            callback(e);
                            return;
                        }
                        EditorCoroutine.start(applyPath(data,callback));
                    });
                }
            } catch (System.Exception e){
                callback(e);
                //throw e;
            }
            
        }

        /// <summary>
        /// 실제 패치를 진행합니다.
        /// </summary>
        /// <param name="files">패치 데이터</param>
        /// <param name="patchFile">패치 파일 경로</param>
        /// <param name="processCallback">진행상태에 대한 프로그레스바 업데이트를 위한 콜백</param>
        void processPatchApply(List<VersionMetadata.Files> files ,string patchFile, System.Action<string,string,float> processCallback, System.Action doneCallback) {
            List<VersionMetadata.Files> processedFiles = new List<VersionMetadata.Files>();//백업 후 복원을 위한 데이터

            //delete 부터 처리하도록 delete 메소드 파일을 위로 올림.
            files.Sort();

            try {
                clearBackupLocalFile();
                int fileCount = 0;
                foreach(var file in files) {
                    if (processCallback!=null) 
                        processCallback("Patch HIVE SDK",file.path, (float)(fileCount++)/(float)files.Count);
                    
                    var patchPath = Path.Combine(patchFile, HIVEManagerSDKFile.getDownloadedFilePathFromAssetsFilePath(file.path));

                    var validHash = file.changeType == VersionMetadata.Files.ChangeMethod.Delete ?
                                                        makeAbsoluteAssetsPath(file.path) : patchPath;


                    if (!CheckFileValidation(validHash,file.fileHash)) {
                        throw new InvalidDataException("file hash not match. : "+file);
                    }

                    var assetPath = makeAbsoluteAssetsPath(file.path);
                    //만약 ADD인대도 파일이 있다면. 파일을 지우고 속성을 modify로 변경해줌.
                    if ( file.changeType == VersionMetadata.Files.ChangeMethod.Add &&
                        (File.Exists(assetPath) || Directory.Exists(assetPath))) {
                        file.changeType = VersionMetadata.Files.ChangeMethod.Modify;
                    }
                    
                    var isDeleteAssets = file.changeType == VersionMetadata.Files.ChangeMethod.Modify || file.changeType == VersionMetadata.Files.ChangeMethod.Delete;
                    var isCopyAssets = file.changeType == VersionMetadata.Files.ChangeMethod.Modify || file.changeType == VersionMetadata.Files.ChangeMethod.Add;
                    var aleadyAddedProcess = false;

                    if (isDeleteAssets) {
                        backupLocalFile(file.path);
                        if(File.Exists(assetPath)){
                            File.Delete(assetPath);
                        } else if(Directory.Exists(assetPath)) {
                            Directory.Delete(assetPath,true);
                        }
                        var parentDir = Path.GetDirectoryName(assetPath);
                        deleteDirIfIsDirectoryEmpty(parentDir);

                        processedFiles.Add(file);
                        aleadyAddedProcess = true;
                    }

                    if (isCopyAssets) {
                        
                        copyFiles(patchPath, assetPath);
                        if (aleadyAddedProcess == false) {
                            processedFiles.Add(file);
                        }
                    }
                }
                doneCallback();
            } catch (System.Exception e) {
                rollbackLocalFile(processedFiles,(string message,float process)=>{
                    if (processCallback != null)
                        processCallback("Patch Fail... Rollback SDK..",message,process);
                });
                throw e;
            } finally {
                clearBackupLocalFile();
                //최신버전으로 버전메타데이터 갱신
                currentVersionInfo = null;
                requestGetVersion(getVersionHash(), (version, e) => {
                    if (e != null) {
                        throw e;
                    }
                    currentVersionInfo = version;  
                });
            }
        }
        
        static void deleteDirIfIsDirectoryEmpty(string dir) {
            if (Directory.Exists(dir)) {
                if (assetsPathInfo() == new DirectoryInfo(dir)) {
                    Debug.Log(dir);
                    return;
                }
                if (Directory.GetFiles(dir).Length <= 0 && Directory.GetDirectories(dir).Length <= 0) {
                    Debug.Log("remove dir " + dir + ":" + Directory.GetFiles(dir).Length);
                    Directory.Delete(dir);
                    deleteDirIfIsDirectoryEmpty(Directory.GetParent(dir).FullName);
                }
            }
        }

        /// <summary>
        /// 패치 데이터를 롤백합니다.
        /// </summary>
        /// <param name="files">패치가 진행된 파일</param>
        /// <param name="processCallback">진행상태에 대한 프로그레스바 업데이트를 위한 콜백</param>
        static void rollbackLocalFile(List<VersionMetadata.Files> files,System.Action<string,float> processCallback) {
            try {
                int fileCount = 0;
                foreach(var file in files) {
                    var assetPath = makeAbsoluteAssetsPath(file.path);
                    var isDeletedAssets = file.changeType == VersionMetadata.Files.ChangeMethod.Modify || file.changeType == VersionMetadata.Files.ChangeMethod.Delete;
                    var isCopiedAssets = file.changeType == VersionMetadata.Files.ChangeMethod.Modify || file.changeType == VersionMetadata.Files.ChangeMethod.Add;

                    if (processCallback != null) 
                        processCallback(file.path, fileCount);
                    
                    //ADDED 혹은 Modify일 경우 제거하기.
                    if (isCopiedAssets) {
                        if (File.Exists(assetPath)) {
                            File.Delete(assetPath);
                        } else if (Directory.Exists(assetPath)) {
                            Directory.Delete(assetPath);
                        }
                        //현재폴더의 파일이 모두 지워졌다면 폴더 지우기.
                        var parentDir = Path.GetDirectoryName(assetPath);
                        deleteDirIfIsDirectoryEmpty(parentDir);
                    }

                    //지워진 에셋은 원본으로 복구하기.
                    if (isDeletedAssets) {
                        var backupPath = makeAbsoulteBackupPath(file.path);
                        copyFiles(backupPath, assetPath);
                    }
                }
            } catch (System.Exception e){
                Debug.LogError("Critical Error:: can't rollback HIVE SDK Files");
                throw e;
            }
        }

        /// <summary>
        /// 패치를 위해 백업해둔 파일을 제거합니다.
        /// </summary>
        static void clearBackupLocalFile() {
            if (Directory.Exists(backupPath())) {
                Directory.Delete(backupPath(),true);
            }
        }

        /// <summary>
        /// 파일을 백업경로에 백업합니다.
        /// </summary>
        /// <param name="filePath">백업할 파일의 경로</param>
        static void backupLocalFile(string filePath) {
            var assetPath = makeAbsoluteAssetsPath(filePath);
            var backupPath = makeAbsoulteBackupPath(filePath);
            copyFiles(assetPath, backupPath);
        }

        /// <summary>
        /// VersionMetadata를 통해 버전을 패치합니다.
        /// 다운로드 및 파일처리를 호출합니다.
        /// </summary>
        /// <param name="meta">메타데이터</param>
        IEnumerator applyPath(VersionMetadata meta, System.Action<System.Exception> doneCallback) {
            var downloadPath = Path.Combine(Application.temporaryCachePath, "HDM/");
            var unzipTempPath = Path.Combine(Application.temporaryCachePath, "HDMUnzipTemp/");
            unzipTempPath = Path.Combine(unzipTempPath, meta.newVersionCode);
            
            try {
                // 1. download file
                if (Directory.Exists(unzipTempPath))
                    Directory.Delete(unzipTempPath, true);
                createDirectory(unzipTempPath);
                createDirectory(downloadPath);
            } catch (System.Exception e) {
                doneCallback(e);
                throw e;
            }


            //2. 파일 다운로드하고 압축풀기.
            bool downloadComplate = false;
            string downloadFilePath = "";
            beginProcessWorking("Download new sdk...");
            EditorCoroutine.start(downloadFileAsync(meta.downloadUrl,downloadPath,
            (float process)=>{
                updateProcessWorking("Downloading...",process);
            },
            (string path)=>{
                downloadFilePath = path;
                downloadComplate = true;
            }));
            do {
                //wait for donwload
                yield return null;
            }while(!downloadComplate);
            endProcessWorking();

            if (string.IsNullOrEmpty(downloadFilePath)) {
                Debug.LogError("HIVE SDK Download fail.... : " + downloadFilePath);
            }

            try {
                beginProcessWorking("Unzip new sdk...");
                if (unzipFile(downloadFilePath, unzipTempPath,
                (string message,float progress)=>{
                    /*UNZIP FILE CALLBACK*/ 
                    updateProcessWorking(message,progress); 
                }) == false) {
                    downloadComplate = false;
                    //File.Delete(downloadFilePath);
                }
                endProcessWorking();

                if(downloadComplate) {
                    //3. 패치를 위해 로컬파일 처리.
                    beginProcessWorking("Patch SDK");
                    processPatchApply(meta.files,unzipTempPath,(string stepName, string message, float process)=>{
                        setProcessWorkingName(stepName);
                        updateProcessWorking(message, process);
                    }, ()=>{doneCallback(null);});
                    endProcessWorking();
                }
                //var downloadFilePath = downloadFile(meta.downloadUrl, downloadPath);
            } catch (System.Exception e) {
                endProcessWorking();
                doneCallback(e);
            } finally {
                //압축해제 파일 제거
                File.Delete(downloadFilePath);
                Directory.Delete(downloadPath, true);
                Directory.Delete(unzipTempPath, true);
            }
        }
        #endregion 

        #region REPAIR MODE
        public void repairCurrentVersion(List<string> repaireFiles, System.Action<System.Exception> callback) {
            if (isAvilableDiskSize() == false) {
                callback(new System.Exception(getNotAvailableDiskSizeErrorMessage()));
                return;
            }
            if (isInstallSDK()) {
                //current version call
                var currentVersion = getCurrentVersion();
                //beginProcessWorking("repaire mode");
                requestVersionPatchData(currentVersion.versionCode,(data, e)=>{
                    if (e != null) {
                        endProcessWorking();
                        callback(e);
                        return;
                    }
                        
                    //files 재처리
                    List<VersionMetadata.Files> repairData = new List<VersionMetadata.Files>();
                    int i=0;
                    foreach(var repairFile in repaireFiles) {
                        var lowwerPath = repairFile.ToLower();
                        var originFile = data.files.Find((file)=>{
                            return file.path.ToLower() == lowwerPath;
                        });
                        var assetPath = makeAbsoluteAssetsPath(repairFile);
                        if (File.Exists(assetPath) || Directory.Exists(assetPath)) {
                            //로컬파일이 변경되었으므로 원본파일로 치환하기위해 Modify메소드 입력.
                            originFile.changeType = VersionMetadata.Files.ChangeMethod.Modify;
                        } else {
                            //파일이 없는 경우는 Add
                            originFile.changeType = VersionMetadata.Files.ChangeMethod.Add;
                        }
                        repairData.Add(originFile);
                        //updateProcessWorking(repairFile,(float)i++/(float)repaireFiles.Count);
                    }
                    //endProcessWorking();
                    data.files = repairData;
                    EditorCoroutine.start(applyPath(data,callback));
                });
            }
        }
        #endregion

        #region VERSION Method
        /// <summary>
        /// 현제 버전의 해시를 얻습니다.
        /// Hive_SDK_v4/Editor/VersionHash.txt 파일이 존재할경우 해당파일의 값을 우선사용하여 버전을 체크합니다.
        /// 만약 파일이 없을시.
        /// SDK의 각 핵심파일을 이용하여 버전을 검사합니다. (매니저 개발 이전에 배포된 SDK를 호환하기 위한 Legacy입니다.)
        /// </summary>
        /// <returns>버전을 판별하기 위한 해시값 입니다.</returns>
        private static string getVersionHash() {
            if (File.Exists(versionHashCashedPath)) {
                string hash = "";
                StreamReader inp_stm = new StreamReader(versionHashCashedPath);
                while(!inp_stm.EndOfStream) {
                    hash = inp_stm.ReadLine( );
                }
                inp_stm.Close();
                
                if (string.IsNullOrEmpty(hash)) {
                    Debug.LogError("Can' Load Version Hash. contact hive team!");
                    return null;
                }
                return hash;
            } else {
                Debug.Log("Can't find version hash file. check old style.");

                if (File.Exists(versionHashIOSPath) == false) {
                    Debug.LogError("SDK Version Data Error! contact hive team! ("+versionHashIOSPath+")");
                    return null;
                }
                if (File.Exists(versionHashAndroidPath) == false) {
                    Debug.LogError("SDK Version Data Error! contact hive team! ("+versionHashAndroidPath+")");
                    return null;
                }

                var iosHash =  getHashFromFile(versionHashIOSPath);
                var adHash = getHashFromFile(versionHashAndroidPath);
                var versionHashStr = iosHash+adHash;

                using(var cryptoProvider = new System.Security.Cryptography.SHA1CryptoServiceProvider()) {
                    byte[] hash = cryptoProvider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(versionHashStr));

                    System.Text.StringBuilder formatted = new System.Text.StringBuilder(2 * hash.Length);
                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }
                    return formatted.ToString().ToLower();
                }
            }
        }

        /// <summary>
        /// 업데이트가 가능한 버전목록을 얻습니다.
        /// </summary>
        /// <returns>현재 설치된 버전보다 높은버전의 다운가능한 목록을 반환합니다.</returns>
        public List<AvailableDownload> getUpdatableVersion()
        {
            var versionList = availableDownloadList;
            var currnetVersion = getCurrentVersion();

            //TODO currentVersion 널일때 처리하기
            if (currnetVersion != null)
            {
                for (var i = 0; i < versionList.Count; ++i)
                {
                    if (currnetVersion.versionCode < versionList[i].versionCode)
                    {
                        versionList = versionList.GetRange(i, versionList.Count - i);
                        break;
                    }
                }
            }

            return versionList;
        }

        /// <summary>
        /// 특정 파일을 검사하여 SDK의 설치여부를 반환합니다.
        /// </summary>
        /// <returns>SDK 설치여부</returns>
        public bool isInstallSDK() { 
            if (File.Exists(versionHashCashedPath)) {
                return true;
            }
            return (Directory.Exists(versionHashIOSPath) || File.Exists(versionHashIOSPath)) &&
                (Directory.Exists(versionHashAndroidPath) || File.Exists(versionHashAndroidPath));
            //return getCurrentVersion() != null;
        }
        
        /// <summary>
        /// SDK의 버전정보를 받아오는데 성공했는지 여부를 반환합니다.
        /// </summary>
        /// <returns>SDK 버전정보 수신 여부</returns>
        public bool isGetInstalledVersionMetadata() {
            if (isInstallSDK()) {
                return getCurrentVersion() != null;
            }
            return false;;
        }

        /// <summary>
        /// 현 버전의 버전정보를 리턴합니다.
        /// </summary>
        /// <returns>버전 정보</returns>
        public VersionInfoFromHash getCurrentVersion() {
            return currentVersionInfo;
        }

        static void requestVersionPatchDataAndIgnore(string currentVersion, string targetVersion,System.Action<VersionMetadata, System.Exception> callback) {
            requestVersionPatchData(currentVersion, targetVersion,(data, e)=>{
                var ignoredData = ignoreChangeMetadata(data);
                callback(ignoredData, e);
            });
        }

        static void requestVersionPatchDataAndIgnore(string targetVersion,System.Action<VersionMetadata, System.Exception> callback) {
            requestVersionPatchData(targetVersion,(data, e)=>{
                var ignoredData = ignoreChangeMetadata(data);
                callback(ignoredData, e);
            });
        }
        
        /// <summary>
        /// 가장 최신버전의 SDK버전을 리턴합니다.
        /// </summary>
        /// <returns>최신버전의 SemanticVersion</returns>
        public void requestLatestVersion(System.Action<HIVESemanticVersion> callback) {
            requestAvailableDownload((download)=>{
                if (availableDownloadList.Count <= 0) {
                    if (callback != null) {
                        callback(null);
                    }
                }
                HIVESemanticVersion latest = availableDownloadList[0].versionCode;
                foreach (var v in availableDownloadList) {
                    latest = latest < v.versionCode ? v.versionCode : latest;
                }
                if (callback != null) {
                    callback(latest);
                }
            });
        }
        #endregion

        /// <summary>
        /// id로 다운로드 정보를 찾습니다.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AvailableDownload getDownload(long id) {
            if (availableDownloadList == null) {
                return null;
            }
            return availableDownloadList.Find((match)=>{
                return match.id == id;
            });
        }

        /// <summary>
        /// id로 공지정보를 찾습니다.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AvailableNotice getNotice(string id) {
            if (availableNoticeList == null) {
                return null;
            }
            return availableNoticeList.Find((match)=>{
                return match.id == id;
            });
        }

        /// <summary>
        /// UI에 출력하기위한 공지 목록을 얻습니다. 상위목록 6개 까지 얻습니다.
        /// </summary>
        /// <returns></returns>
        public List<HIVENoticeContents> getNoticesContents() {
            var notices = new List<HIVENoticeContents>();
            //팝업에서 최대 6개 까지 표현가능.
            var renderNoticeCount = 6 > availableNoticeList.Count ? availableNoticeList.Count:6;
            for (int i=0;i < renderNoticeCount; i++) {
                notices.Add (
                    new HIVENoticeContents(availableNoticeList[i].id, availableNoticeList[i].title, availableNoticeList[i].lastUpdateDateTime)
                );
            }
            return notices;
        }

        /// <summary>
        /// UI에 출력하기위한 다운로드 가능한 버전목록을 얻습니다.
        /// </summary>
        /// <returns></returns>
        public List<HIVESDKUpdateListViewData> getUpdateListViewDatas() {
            var updates = new List<HIVESDKUpdateListViewData>();
            var updatableVersions = getUpdatableVersion();

            if(updatableVersions == null) {
                return null;
            }

            foreach(var d in updatableVersions) {
                //현재보다 높은버전으로 필터링
                if (currentVersionInfo != null &&
                    currentVersionInfo.versionCode >= d.versionCode) {
                        continue;
                }
                updates.Add(
                    new HIVESDKUpdateListViewData(){
                        id = d.id.ToString(),
                        releaseDate = d.deployDateTime,
                        version = d.versionCode,
                        features = d.title
                    }
                );
            }
            return updates;
        }
    }
}