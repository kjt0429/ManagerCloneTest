using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using hive.manager.editor;

//팝업과 비지니스 연결.
namespace hive.manager
{
    [InitializeOnLoad]
    public static class HIVEManagerController {
        static HIVEManager manager = new HIVEManager();


        [System.Serializable]
        public class DontShowAgainAction {
        }

        static HIVEManagerController() {
            manager.connectHIVESDKManager(null);
            if (UnityEditor.SessionState.GetBool("FirstLaunch",true)) {
                UnityEditor.SessionState.SetBool("FirstLaunch",false);
                manager.requestNewNotice((notice)=>{
                    if (notice != null) {
                        ShowOneDaySkipNoticePopup(notice, (alert,isDontShowAgain,popupIdentifier)=>{
                            if (isDontShowAgain) {
                                HIVEManager.setDontShowNotice(popupIdentifier); 
                            }
                        });
                    }
                });
            }
        }

        static HIVEEditorStrings getEditorStrings() {
            return HIVEEditorStrings.Instance;
        }

        #region MenuItem EnableCheck 
        public static bool isShowUpdateListEnable() {
            bool metadataGet = true;
            if (manager.isInstallSDK()) {
                metadataGet = manager.isGetInstalledVersionMetadata();
            }
            return metadataGet;
        }
        public static bool isShowIntegrityVerificationEnable() {
            return manager.isGetInstalledVersionMetadata();
        }
        public static bool isShowNowVersionEnable() {
            return manager.isGetInstalledVersionMetadata();
        }
        public static bool isShowRepairMode() {
            return manager.isGetInstalledVersionMetadata();
        }
        #endregion
        
        #region MenuItem
        public static void ShowUpdateList() {
            EditorUtility.DisplayProgressBar("Show Update List Loading...","request...",0.1f);
            manager.requestUpdateListViewData((viewData)=>{
                EditorUtility.DisplayProgressBar("Show Update List Loading...","request...",1f);
                EditorUtility.ClearProgressBar();
                if (viewData == null) {
                    return;
                }
                HIVEUpdateListPopup.ShowPopup(viewData, 
                    (popup,id)=> {
                        var download = manager.getDownload(long.Parse(id));
                        if (ShowStartUpdatePopup(download.versionCode)) {
                            popup.Close(); //업데이트를 수행하면 업데이트 리스트 팝업 닫음.
                            manager.processDownloadAndPatch(download, (downloadFailureReason,error)=>{
                                if (error != null) {
                                    Debug.LogException(error);
                                    ShowUpdateDoneFailurePopup(error.Message);
                                    throw error;
                                }
                                if (downloadFailureReason != null && downloadFailureReason.Count > 0) {
                                    ShowUpdateDoneFailurePopup(downloadFailureReason);
                                } else {
                                    EditorApplication.LockReloadAssemblies();
                                    ShowUpdateDoneSuccessPopup();
                                    EditorApplication.UnlockReloadAssemblies();
                                    manager.SendSDKInstallLog(download.versionCode,DateTime.Now);
                                    AssetDatabase.Refresh();
                                }
                            });
                        }
                    },
                    (popup, id)=>{
                        // Show Feature Detail
                        var download = manager.getDownload(long.Parse(id));
                        if (download != null) {
                            HIVESDKContentsDetail.ShowPopup(
                                download.title,
                                download.details
                            );
                        } else {
                            Debug.LogError("Can't find Download index");
                        }
                    }
                );
            });
        }

        public static void ShowIntegrityVerification() {
            if (ShowStartVadlidityCheckPopup()) {
                try {
                    EditorUtility.DisplayProgressBar("Show Check Vadlidity Loading...","request...",0.1f);
                    manager.integrityVerification((List<string> integrityFailureReason)=>{
                        EditorUtility.DisplayProgressBar("Show Check Vadlidity Loading...","request...",1f);
                        EditorUtility.ClearProgressBar();
                        if (integrityFailureReason != null && integrityFailureReason.Count > 0) {
                            ShowVadlidityDoneFailurePopup(integrityFailureReason);
                        } else {
                            ShowVadlidityDoneSuccessPopup();
                        }
                    });
                } catch (System.Exception e) {
                    EditorUtility.ClearProgressBar();
                    Debug.LogError(e.ToString());
                    return;
                }
            }
        }


        public static void ShowNotices() {
            EditorUtility.DisplayProgressBar("Show Notice List Loading...","request...",0.1f);
            manager.requestNoticeListViewData((viewData)=>{
                EditorUtility.DisplayProgressBar("Show Notice List Loading...","request...",1f);
                EditorUtility.ClearProgressBar();
                if (viewData != null) {
                    HIVEUpdateNoticePopup.ShowPopup(viewData, (popup, id)=>{
                        var notice = manager.getNotice(id);
                        if (notice != null) {
                            HIVENoticeDetail.ShowPopup(notice.title,notice.details);
                        } else {
                            Debug.LogError("Can't find Notice index");
                        }
                    });
                }
            });
            
        }

        public static void ShowNowVersion() {
            var current = manager.getCurrentVersion();
            if (current != null) {
                manager.requestLatestVersion((latest)=> {
                    HIVECurrentVersionPopup.ShowPopup(current.versionCode, 
                                                latest, 
                                                HIVEManagerVersion.Version);
                });
            } else {
                Debug.LogError("Can't Find Current version");
            }
        }

        public static void RetryHIVEManagerConnection() {
            manager.connectHIVESDKManager((isConnected)=>{
                EditorUtility.DisplayDialog(getEditorStrings().reconnectDoneTitle,
                                            getEditorStrings().reconnectDoneContents,
                                            getEditorStrings().popupOK);
            });
        }

        public static void RepairSDK() {
            if (manager.getCurrentVersion() != null) {
                if(EditorUtility.DisplayDialog(getEditorStrings().versionRepairTitle,
                                                getEditorStrings().versionRepairContents,
                                                getEditorStrings().popupOK,
                                                getEditorStrings().popupCancel) == false) {
                                                    return;
                                                } 

                manager.integrityVerification((repairFiles)=>{
                    if (repairFiles == null || repairFiles.Count == 0) {
                        EditorUtility.DisplayDialog(getEditorStrings().versionRepairNotNeedTitle,
                                                    getEditorStrings().versionRepairNotNeedContents,
                                                    getEditorStrings().popupOK);
                    } else {
                        manager.repairCurrentVersion(repairFiles,(e)=> {
                            if (e != null) {
                                var failureContents = string.Format(getEditorStrings().versionRepairFailureContents,e.Message);
                                EditorUtility.DisplayDialog(getEditorStrings().versionRepairFailureTitle, failureContents, getEditorStrings().popupOK);
                            } else {
                                EditorUtility.DisplayDialog(getEditorStrings().versionRepairSuccessTitle,
                                                            getEditorStrings().versionRepairSuccessContents, 
                                                            getEditorStrings().popupOK);
                                AssetDatabase.Refresh();
                            }
                        });
                    }
                });
            }
        }
        #endregion

        #region Integrity Verification
        public static bool ShowStartVadlidityCheckPopup() {
            return EditorUtility.DisplayDialog(
                        getEditorStrings().sdkValidityCheckTitle,
                        getEditorStrings().sdkValidityCheckContents,
                        getEditorStrings().popupStart,
                        getEditorStrings().popupCancel);
        }

        public static bool ShowVadlidityDoneSuccessPopup() {
            return EditorUtility.DisplayDialog(
                        getEditorStrings().sdkValidityCheckDoneTitle,
                        getEditorStrings().sdkVadlidityCheckDoneContentsSuccess,
                        getEditorStrings().popupOK);
        }
        public static void ShowVadlidityDoneFailurePopup(List<string> failureReason) {
            HIVEScrollContentsPopup.ShowPopup(
                getEditorStrings().sdkValidityCheckDoneTitle,
                getEditorStrings().sdkVadlidityCheckDoneContentsFailure, failureReason, null);
        }
        #endregion

        #region Update Download
        public static bool ShowStartUpdatePopup(string version) {
            var message = string.Format(getEditorStrings().updateListWouldYouUpdateNewVersion,version); 
            return EditorUtility.DisplayDialog(
                        getEditorStrings().updateListWouldYouUpdateNewVersionTitle,
                        message,
                        getEditorStrings().popupStart,
                        getEditorStrings().popupCancel);
        }
        public static bool ShowUpdateDoneSuccessPopup() {
            if (EditorUtility.DisplayDialog(
                                getEditorStrings().sdkUpdateDoneSuccessTitle,
                                getEditorStrings().sdkUpdateDoneSuccessContents,
                                getEditorStrings().popupOK,
                                getEditorStrings().popupGoDeveloper)) {
                return true;
            } else {
                Application.OpenURL("https://developers.withhive.com");
                return false;
            }
        }
        public static void ShowUpdateDoneFailurePopup(List<string> failureReason) {
            HIVEScrollContentsPopup.ShowPopup(
                getEditorStrings().sdkUpdateFailureDataNotValidTitle,
                getEditorStrings().sdkUpdateFailureDataNotValidContents,failureReason, null);
        }
        public static void ShowUpdateDoneFailurePopup(string failureReason) {
            if (failureReason == null)
                failureReason = "";
            var contents = string.Format(getEditorStrings().sdkUpdateFailureContents, failureReason);
            EditorUtility.DisplayDialog(getEditorStrings().sdkUpdateFailureTitle,contents, getEditorStrings().popupOK);
        }
        #endregion

        public static void ShowOneDaySkipNoticePopup(AvailableNotice notice, HIVENoticeDetail.SaveDontShowAgainDelegate callback) {
            HIVENoticeDetail.ShowPopup(notice.title, notice.details, notice.id, callback);
        }
    }
}