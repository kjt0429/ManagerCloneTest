using System.IO;
using UnityEngine;
using System.Text;
using System.Net;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using NUnit.Framework;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
namespace hive.manager
{
    public static class StringPathUtils {
        /// <summary>
        /// 유닉스 경로를 현재 사용하는 OS의 경로로 치환한다.
        /// </summary>
        /// <param name="path">유닉스 경로</param>
        /// <returns>현재 OS에 맞는 경로</returns>
        public static string ConvertUnixPathToOsPath(this string path) {
            var trim_path = path.Trim();
            var paths = trim_path.Split('/');
            var path_string ="";
            for (int i = 0; i < paths.Length; i++) {
                path_string += paths[i];
                if (i+1 < paths.Length) { //is not last index
                    path_string += Path.DirectorySeparatorChar;
                }
            }
            return path_string;
        }


        public static string GetHashSha1(this string input) {
            using (SHA1Managed sha1 = new SHA1Managed()) {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash) {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }

    public partial class HIVEManager {

        private static string getHashFromFile(string filePath) {
            using(FileStream fs = new FileStream(filePath, FileMode.Open)) {
            using(var cryptoProvider = new System.Security.Cryptography.SHA1CryptoServiceProvider()) {
                byte[] hash = cryptoProvider.ComputeHash(fs);

                StringBuilder formatted = new StringBuilder(2 * hash.Length);
                foreach (byte b in hash)
                {
                    formatted.AppendFormat("{0:X2}", b);
                }
                return formatted.ToString().ToLower();
            }}
        }
        private static string assetsPath() {
            return Application.dataPath;
        }
        static DirectoryInfo assetDirInfo = new DirectoryInfo(assetsPath());
        private static DirectoryInfo assetsPathInfo() {
            return assetDirInfo;
        }
        
        private static string makeAbsoluteAssetsPath(string relativePath){
            return Path.Combine(assetsPath(),relativePath);
        }

        private static string backupPath() {
            return Path.Combine(Application.temporaryCachePath, "HMD_Backup");
            
        }

        private static string makeAbsoulteBackupPath(string relativePath) {
            return Path.Combine(backupPath(), relativePath);
        }

        private static void copyFiles(string sourceDirectory, string targetDirectory)
        {
            if (File.Exists(sourceDirectory)) {
                createDirectory(Directory.GetParent(targetDirectory).FullName);
                File.Copy(sourceDirectory, targetDirectory);
            } else if(Directory.Exists(sourceDirectory)) {
                foreach (var c in Directory.GetFiles(sourceDirectory))
                {
                    var movingPath = c.Replace(sourceDirectory, targetDirectory);
                    File.Copy(c, movingPath);
                }
                foreach (var d in Directory.GetDirectories(sourceDirectory)) {
                    //var s = d.Split(Path.DirectorySeparatorChar);
                    var dname = Path.GetFileName(d);//s[s.Length-1];
                    var copyPath = Path.Combine(targetDirectory, dname);
                    Debug.Log(copyPath);
                    createDirectory(copyPath);
                    copyFiles(d, copyPath);
                }
            } else {
                throw new System.IO.FileNotFoundException(sourceDirectory +" not found");
            }
        }

        #if UNITY_EDITOR_OSX
        [DllImport ("HIVEEditorUtil")]
        private static extern ulong GetFreeDiskSizeFromPath (string path);
        #elif UNITY_EDITOR_WIN
		[DllImport("kernel32.dll", SetLastError=true, CharSet=CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
			out ulong lpFreeBytesAvailable,
			out ulong lpTotalNumberOfBytes,
			out ulong lpTotalNumberOfFreeBytes);
        #endif
        
        private static ulong getFreeDiskByteFromPath(string path) {

            #if UNITY_EDITOR_OSX
            return GetFreeDiskSizeFromPath(path);
            #else
            if (string.IsNullOrEmpty(path))
            {
                throw new System.ArgumentNullException("path");
            }

            DirectoryInfo info = null;
            if (File.Exists(path)) {
                FileInfo file = new FileInfo(path);
                info = file.Directory.Root;
            } else if(Directory.Exists(path)) {
                info = (new DirectoryInfo(path)).Root;
            } else {
                return 0;
            }

            ulong FreeBytesAvailable = 0;
			ulong TotalNumberOfBytes = 0;
			ulong TotalNumberOfFreeBytes = 0;

			bool success = GetDiskFreeSpaceEx(info.FullName, out FreeBytesAvailable, out TotalNumberOfBytes,
				out TotalNumberOfFreeBytes);
			if (!success)
				throw new System.ComponentModel.Win32Exception();
			
			return FreeBytesAvailable;
            #endif            
        }

        /// <summary>
        /// 디렉토리를 생성합니다.
        /// </summary>
        /// <param name="path">생성경로</param>
        private static void createDirectory(string path)
        {
            Directory.CreateDirectory(path); 
        }

        /// <summary>
        /// 파일을 다운로드 합니다. Coroutine으로 호출해주세요.
        /// </summary>
        /// <param name="downloadUrl">다운받을 파일의 URL입니다.</param>
        /// <param name="downloadTargetPath">파일을 다운받을 경로 입니다.</param>
        /// <param name="processCallback">파일 다운로드 진행 상태를 전달 받을 콜백입니다.</param>
        /// <param name="doneCallback">파일 다운로드 완료 콜백입니다. 다운로드된 파일의 경로를 돌려줍니다.</param>
        /// <returns></returns>
        private static IEnumerator downloadFileAsync(string downloadUrl, string downloadTargetPath, System.Action<float> processCallback, System.Action<string> doneCallback) {
            var request =  UnityWebRequest.Get(downloadUrl);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            do {  
                if (processCallback != null) 
                    processCallback(request.downloadProgress);
                yield return null;
            } while(!request.isDone);  

            try {
                if (request.isHttpError || request.isNetworkError) {
                    throw new HttpListenerException((int)request.responseCode);
                }

                //파일 경로 만들기
                var uri = new System.Uri(downloadUrl);
                //string url = downloadUrl;
                string name = Path.GetFileName(uri.GetLeftPart(System.UriPartial.Path));
                var downloadFilePath = Path.Combine(downloadTargetPath, name);

                //파일 쓰기
                if (File.Exists(downloadFilePath)) {
                    File.Delete(downloadFilePath);
                }
                createDirectory(downloadTargetPath);
                byte[] results = request.downloadHandler.data;
                using (var fs = new FileStream(downloadFilePath,FileMode.Create)){
                    fs.Write(results,0,results.Length);
                }
                doneCallback(downloadFilePath);
            } catch(System.Exception e){
                doneCallback(null);
                throw e;
            }
            yield return null;
        }

        [DllImport ("HIVEEditorUtil")]
        private static extern bool unzip (string archive_path, string target_path);

        [DllImport("HIVEEditorUtil")]
        private static extern void setUnzipProgressCallback(UnzipProgressCallback callback);

        [DllImport("HIVEEditorUtil")]
        private static extern void setUnzipErrorCallback(UnzipErrorCallback callback);
         
        [DllImport("HIVEEditorUtil")]
        private static extern void setUnzipProgressReadCallback(UnzipProgressReadCallback callback);

        [DllImport("HIVEEditorUtil")]
        private static extern void setUnzipProgressReadFinishCallback(UnzipProgressReadFinshCallback callback);


        private delegate void UnzipProgressCallback(string archive_path, string  current_path, int current_num, int max_num);
        private delegate void UnzipErrorCallback(string message);
        private delegate bool UnzipProgressReadCallback(string archive_path, string current_path, System.IntPtr data,int size);
        private delegate void UnzipProgressReadFinshCallback(string archive_path, string current_path);

        private static Dictionary<string, FileStream> zaStreams = new Dictionary<string, FileStream>();
        private static string currentZipTargetPath;
        static System.Action<string,float> currentProgress;

        private static void unzipProgress(string archive_path, string  current_path, int current_num, int max_num) {
            if (currentProgress != null) {
                currentProgress(current_path,(float)(current_num)/(float)max_num);
            }
        }
        private static void unzipError(string message) {
            foreach(var stream in zaStreams) {
                stream.Value.Close();
            }
            zaStreams = new Dictionary<string, FileStream>();
            Debug.LogError("===unzip error===\n"+message);
        }
        
        private static bool unzipRead(string archive_path, string current_path, System.IntPtr data, int size) {

            try {
                if (!zaStreams.ContainsKey(current_path)) {
                    zaStreams[current_path] = File.Create(Path.Combine(currentZipTargetPath, HIVEManagerSDKFile.getDownloadedFilePathFromAssetsFilePath(current_path)));
                }
                var arr = new byte[size];
                Marshal.Copy(data, arr, 0, size);
                zaStreams[current_path].Write(arr, 0, size);
            } catch (System.Exception e){
                Debug.LogError(e.Message);
                return false;
            }
            return true;
        }
        private static void unzipReadFinish(string archive_path, string current_path) {
            zaStreams[current_path].Close();
            zaStreams.Remove(current_path);
        }

        private static bool unzipFile(string FilePath, string zipTargetPath, System.Action<string,float> progress)
        {
            if (System.IO.File.Exists(FilePath) == false)
                return false;

            currentProgress = progress;
            currentZipTargetPath = zipTargetPath;
            progress("open zip file",0);
            setUnzipProgressCallback(unzipProgress);
            setUnzipErrorCallback(unzipError);
            setUnzipProgressReadCallback(unzipRead);
            setUnzipProgressReadFinishCallback(unzipReadFinish);
            bool ret = unzip(FilePath, zipTargetPath);
            setUnzipProgressCallback(null);
            setUnzipErrorCallback(null);
            setUnzipProgressReadCallback(null);
            setUnzipProgressReadFinishCallback(null);
            currentProgress = null;
            return ret;
        }
    }

    [TestFixture]
    class HIVEManagerFileTest {
        [Test]
        public void UnixPathToOSPathTest() {
            var path1 = "/UnixPath/UnixPath1/UnixPath2".ConvertUnixPathToOsPath();
            var path1_expect = Path.DirectorySeparatorChar + "UnixPath" + Path.DirectorySeparatorChar + "UnixPath1" + Path.DirectorySeparatorChar + "UnixPath2";

            var path2 = "/UnixPath/UnixPath1/UnixPath2/".ConvertUnixPathToOsPath();
            var path2_expect = Path.DirectorySeparatorChar + "UnixPath" + Path.DirectorySeparatorChar + "UnixPath1" + Path.DirectorySeparatorChar + "UnixPath2" + Path.DirectorySeparatorChar;

            var path3 = "UnixPath/UnixPath1/UnixPath2".ConvertUnixPathToOsPath();
            var path3_expect = "UnixPath" + Path.DirectorySeparatorChar + "UnixPath1" + Path.DirectorySeparatorChar + "UnixPath2";

            var path4 = "UnixPath/UnixPath1/UnixPath2/".ConvertUnixPathToOsPath();
            var path4_expect = "UnixPath" + Path.DirectorySeparatorChar + "UnixPath1" + Path.DirectorySeparatorChar + "UnixPath2" + Path.DirectorySeparatorChar;

            TestContext.WriteLine(path1 + " = " + path1_expect);
            Assert.AreEqual(path1, path1_expect);
            TestContext.WriteLine(path2 + " = " + path2_expect);
            Assert.AreEqual(path2, path2_expect);
            TestContext.WriteLine(path3 + " = " + path3_expect);
            Assert.AreEqual(path3, path3_expect);
            TestContext.WriteLine(path4 + " = " + path4_expect);
            Assert.AreEqual(path4, path4_expect);
        }
    }
}