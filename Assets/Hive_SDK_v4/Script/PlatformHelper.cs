/**
 * @file    PlatformHelper.cs
 * 
 * @author  imsunghoon
 * @date    2017-2022
 * @copyright Copyright © Com2uS Platform Corporation. All Right Reserved.
 * @brief 플랫폼 사용 편의를 위한 헬퍼 기능들의 모음
 *
 */

using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine.UI;
using System.Linq;

/**
 * @defgroup PlatformHelper
 * @{
 */

namespace hive
{
	/**
	 * @brief 플랫폼 사용 편의를 위한 헬퍼 기능들의 모음<br/><br/>
	 * 
	 * @ingroup PlatformHelper
	 * @author imsunghoon
	 */
	public class PlatformHelper {

		/**
		 * @brief Share 관련 동작이 완료되었을 때 호출됨. (share)
		 * 
		 * @param isSuccess Share 결과.
		 * 
		 * @ingroup PlatformHelper
		 */
		public delegate void onShare(ResultAPI result);

		/**
		 * @brief Android에서 재요청된 OS 권한동의에 대한 결과 값을 반환한다.
		 *
		 * @param granted 사용자가 수락한 권한 리스트, denied 사용자가 거부한 권한 리스트
		 *
		 * @ingroup PlatformHelper
		 */
        public delegate void onRequestUserPermissions(ResultAPI result, String[] granted, String[] denied);


		public static void share(PlatformShare platformShare, onShare listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("PlatformHelper", "share", listener);
			jsonParam.AddField (PlatformShare.SHARE_TYPE, (int)platformShare.getShareType());
			jsonParam.AddField (PlatformShare.DATA_SUBJECT, platformShare.getSubject());
			jsonParam.AddField (PlatformShare.DATA_TEXT, platformShare.getText());

			JSONObject jsonArray = new JSONObject();

			if (platformShare.getMedia() != null) {
				
				foreach(String path in platformShare.getMedia()) {
					jsonArray.Add(path);
				}
			}

			jsonParam.AddField(PlatformShare.DATA_MEDIA, jsonArray);

			HIVEUnityPlugin.callNative (jsonParam);
		}

	   /**
		* @brief Android에서 사용자에게 OS 권한을 재요청.
		*
		* @param requests Android에서 요청할 권한들을 포함한 리스트.
		*
		*/
        public static void requestUserPermissions(List<String> requests, onRequestUserPermissions listener)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("PlatformHelper", "requestUserPermissions", listener);
            JSONObject requestJson = new JSONObject();
            foreach (string name in requests)
            {
                requestJson.Add(name);
            }
            jsonParam.AddField("requests", requestJson);
            HIVEUnityPlugin.callNative(jsonParam);
        }

		/**
		 * @brief 업데이트 팝업 설정으로 백그라운드에서 앱 다운로드가 완료되면
		 *
		 * UE에서 [Promotion.EngagementEventType.APPUPDATE_DOWNLOADED] 로 신호를 보낸다.
		 *
		 * 이후 [completeUpdate] 를 호출하면 [completeState] 값에 따라 새 버전으로 설치한다.
		 *
		 * 호출 하지 않으면 기본 동작으로 재시작시 설치를 진행한다.
		 *
		 * @param completeState 1 ImmediateUpdate, 2 RestartUpdate, 3 LaterUpdate. otherwise 1.
		 */
		public static void completeUpdate(int completeState)
        {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("PlatformHelper", "completeUpdate", null);
			jsonParam.AddField("completeState", completeState);

			HIVEUnityPlugin.callNative(jsonParam);
		}

		// Native 영역에서 호출된 요청을 처리하기 위한 플러그인 내부 코드
		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			resJsonObject.GetField (ref handlerId, "handler");
			object handler = (object)HIVEUnityPlugin.popHandler (handlerId);

			if (handler == null) return;

			if ("shareText".Equals(methodName) ||
				"shareMedia".Equals(methodName) ||
				"share".Equals(methodName)) {

				onShare listener = (onShare)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
            else if ("requestUserPermissions".Equals(methodName))
            {
                onRequestUserPermissions listener = (onRequestUserPermissions)handler;

                JSONObject jsonArrayGranted = resJsonObject.GetField("granted");
				String[] granted = new String[]{};
                if (!jsonArrayGranted.isNull && jsonArrayGranted.count > 0)
                {
					ArrayList grantedList = new ArrayList();
                    List<JSONObject> jsonList = jsonArrayGranted.list;

                    foreach (JSONObject jsonItem in jsonList)
                    {
                        grantedList.Add(jsonItem.ToString());
                    }
					granted = (string[])grantedList.ToArray(typeof(string));
                }

				JSONObject jsonArrayDenied = resJsonObject.GetField("denied");
				String[] denied = new String[]{};
                if (!jsonArrayDenied.isNull && jsonArrayDenied.count > 0)
                {
					ArrayList deniedList = new ArrayList();
                    List<JSONObject> jsonList = jsonArrayDenied.list;

                    foreach (JSONObject jsonItem in jsonList)
                    {
                        deniedList.Add(jsonItem.ToString());
                    }
					denied = (string[])deniedList.ToArray(typeof(string));
                }

                listener(new ResultAPI(resJsonObject.GetField("resultAPI")), granted, denied);
            }

		}
			
		public static string saveQrcodeToPng(byte[] qrcodestring){

			#if UNITY_EDITOR
			return "";
			#elif (UNITY_IOS || UNITY_ANDROID)
			Texture2D texture = new Texture2D(1, 1);
			texture.LoadImage (qrcodestring);		
			texture.Apply ();

			byte[] bytes = texture.EncodeToPNG();
			string path = Application.persistentDataPath + "/ShareImage.png";
			File.WriteAllBytes (path, bytes);

			return path;
			#else
			return "";
			#endif
		}

		public static byte[] StringToByteArray(string hex) {
			return Enumerable.Range(0, hex.Length)
				.Where(x => x % 2 == 0)
				.Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
				.ToArray();
		}
	}

	public class PlatformShare {
		
		public enum ShareType : int {

			TEXT = 1, 
			MEDIA = 2
		}

		public const String SHARE_TYPE = "shareType";
		public const String DATA_SUBJECT = "shareSubject";
		public const String DATA_TEXT = "shareText";
		public const String DATA_MEDIA = "shareMedia";

		private ShareType shareType;
		private Dictionary<string, object> shareData = null;

		public PlatformShare() {
			
			if (shareData == null)
				shareData = new Dictionary<string, object>();
		}
		public void setSharetype(ShareType shareType) {

			this.shareType = shareType;
		}

		public ShareType getShareType() {

			return this.shareType;
		}

		public void setSubject(String subject) {

			shareData.Add(DATA_SUBJECT, subject);
		}

		public String getSubject() {

			if (shareData.ContainsKey(DATA_SUBJECT) == true) {
				return shareData[DATA_SUBJECT] as string;
			}
			
			return null;
		}

		public void setText(String text) {

			shareData.Add(DATA_TEXT, text);
		}

		public String getText() {

			if (shareData.ContainsKey(DATA_TEXT) == true) {
				return shareData[DATA_TEXT] as string;
			}

			return null;
		}

		public void setMedia(String[] media) {

			shareData.Add(DATA_MEDIA, media);
		}

		public String[] getMedia() {

			if (shareData.ContainsKey(DATA_MEDIA) == true) {
				return shareData[DATA_MEDIA] as String[];
			}

			return null;
		}
	}
}


/** @} */




