using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using UnityEngine.Networking;

namespace hive.manager {
    public partial class HIVEManager {

        [System.Serializable]
        public class RootObjectForArrayRoot<T> {
            public T arr;
        }

        static IEnumerator sendRequest<T>(UnityWebRequest request, System.Action<T, System.Exception> callback) {
            request.chunkedTransfer = false;
            request.useHttpContinue = false;
            
            request.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            yield return request.SendWebRequest();
            do {  
                yield return null;  
            } while(!request.isDone);  
            try {
                string contents = request.downloadHandler.text;
                if (request.isHttpError || request.isNetworkError) {
                    //Debug.LogError(url + " : "+request.responseCode);
                    var errorMsg = "url : "+request.url+ " - code : " + request.responseCode + ", response : " + 
                    contents + ",\n isHttpError : " + request.isHttpError + ", isNetworkError : "+ request.isNetworkError + ", error : " + request.error;
                    callback(default(T),new HttpListenerException((int)request.responseCode, errorMsg));
                } else {
                    // if (string.IsNullOrEmpty(contents)) {
                    //     callback(default(T), new System.NullReferenceException("Http Response is Empty"));
                    // }
                    contents = contents.Trim();//공백제거
                    if (contents.StartsWith("[")) { //Array Root Object라면
                        //Unity의 Json Uiilty가 Object Root만 파싱할수있기때문에 다음과 같이 처리.
                        callback(JsonUtility.FromJson<RootObjectForArrayRoot<T>>("{\"arr\":"+contents+"}").arr,null);
                    } else {
                        callback(JsonUtility.FromJson<T>(contents),null);
                    }
                }
            } catch(System.Exception e){
                callback(default(T),e);
            }
        }

        //Get Request for json
        static void requestGet<T>(string url, System.Action<T, System.Exception> callback) {
            //Debug.Log(url);
            var request =  UnityWebRequest.Get(url);
            EditorCoroutine.start(sendRequest<T>(request, callback));
        }

        //Post Request for json
        static void requestPostJson<T>(string url, string json, System.Action<T, System.Exception> callback) {
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            // var request = UnityWebRequest.Post(url, formData);
            EditorCoroutine.start(sendRequest<T>(request, callback));
        }

        #region VERSION API
            
        static string versionsAPI = baseUrl+"/versions";
        
        //Get /versions/available
        static void requestVersions(System.Action<List<AvailableDownload>,System.Exception> callback) {
            requestGet<List<AvailableDownload>>(versionsAPI+"/available",callback);
        }

        //Get /versions/{current_version}/to/{update_version}
        static void requestVersionPatchData(string currentVersion, string targetVersion,System.Action<VersionMetadata, System.Exception> callback) {
            requestGet<VersionMetadata>(versionsAPI+"/"+currentVersion+"/to/"+targetVersion,callback);
        }

        static void requestVersionPatchData(string targetVersion,System.Action<VersionMetadata, System.Exception> callback) {
            requestGet<VersionMetadata>(versionsAPI+"/"+targetVersion,callback);
        }

        //Get /versions/hash/{hash}
        static void requestGetVersion(string hash, System.Action<VersionInfoFromHash, System.Exception> callback) {
            requestGet<VersionInfoFromHash>(versionsAPI + "/hash/" + hash,callback);
        }
        #endregion

        #region NOTICES API
        static string noticesAPI = baseUrl+"/notices";
        //Get /version/{version_name}/description
        static void requestNotices(System.Action<List<AvailableNotice>,System.Exception> callback) {
            requestGet<List<AvailableNotice>>(noticesAPI+"/available",callback);
        }
        static void requestNoticesAll(System.Action<List<AvailableNotice>,System.Exception> callback) {
            requestGet<List<AvailableNotice>>(noticesAPI+"/all",callback);
        }
        #endregion

        #region LOG API
        static string logAPI = apiUrl + "/log";
        //Log
        static void requestDonwloadDoneAnalyticsLog(LogAnalyticsDownloadDone data, System.Action<System.Exception> callback) {
            string json = JsonUtility.ToJson(data);
            requestPostJson<object>(logAPI+"/manager",json, (obj,exception)=>{
                callback(exception);
            });
        }


        #endregion
    }
}