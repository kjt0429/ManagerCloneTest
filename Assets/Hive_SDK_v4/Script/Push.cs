/**
 * @file    Push.cs
 * 
 *  @date		2016-2022
 *  @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 *  @author		ryuvsken
 *  @since		4.0.0 
 */
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

/**
 * @defgroup Push
 * @{
 * \~korean  모바일 게임에서 푸시 통지 서비스는 게임 유저의 리텐션(잔존율)을 올리기 위한 중요 수단을 제공한다.<br/>
 * HIVE 푸시는 모바일 게임의 리텐션을 증대시키는 마케팅용 광고 푸시를 지원한다.<br/>
 * 또한 Google의 GCM, Apple의 APNS, Amazon의 ADM, 중국 지역의 Jpush를 지원하고, 언어별 발송 시간을 설정할 수 있어 글로벌 서비스 제공에도 대응하고 있다.<br/><br/>
 *
 * \~english In mobile games, push notification service provides an important method to increase the retention of game users.<br/>
 * HIVE Push supports push notification for marketing to increase the user retention of mobile games.<br/>
 * HIVE Push also responds to Google's GCM, Apple's APNS, Amazon's ADM, Jpush in China with language-specific time zones to support global services.<br/><br/>
 */

namespace hive
{
	/**
	 * \~korean  모바일 게임에서 푸시 통지 서비스는 게임 유저의 리텐션(잔존율)을 올리기 위한 중요 수단을 제공한다.<br/>
 	 * HIVE 푸시는 모바일 게임의 리텐션을 증대시키는 마케팅용 광고 푸시를 지원한다.<br/>
 	 * 또한 Google의 GCM, Apple의 APNS, Amazon의 ADM, 중국 지역의 Jpush를 지원하고, 언어별 발송 시간을 설정할 수 있어 글로벌 서비스 제공에도 대응하고 있다.<br/><br/>
 	 *
 	 * \~english In mobile games, push notification service provides an important method to increase the retention of game users.<br/>
 	 * HIVE Push supports push notification for marketing to increase the user retention of mobile games.<br/>
 	 * HIVE Push also responds to Google's GCM, Apple's APNS, Amazon's ADM, Jpush in China with language-specific time zones to support global services.<br/><br/>
	 * \~
	 * @since		4.0.0 
	 * @ingroup Push
	 * @author ryuvsken
	 */
	public class Push {


		/**
		 * \~korean 유저의 푸시 수신 상태를 조회한 결과 통지
		 * 
		 * @param result		API 호출 결과
		 * @param remotePush		유저가 푸시를 수신하는 상태
		 *
		 * \~english Returns information of receiving the remote push
		 * 
		 * @param result		API call result
		 * @param remotePush		The status of receiving push notification
		 * \~
		 * @ingroup Push
		 */
		public delegate	void onRemotePush(ResultAPI result, RemotePush remotePush);


		/**
		 * \~korean 로컬 푸시 등록 결과 통지
		 * 
		 * @param result		API 호출 결과
		 * @param localPush			로컬 푸시 등록 정보
		 *
		 * \~english Returns information of receiving the local push
		 * 
		 * @param result		API call result
		 * @param localPush			Local push registration information. 
		 * \~
		 * @ingroup Push
		 */
		public delegate	void onLocalPush(ResultAPI result, LocalPush localPush);


		/**
		 * \~korean 앱 활성화 시 푸시 설정 결과 통지
		 * 
		 * @param result		API 호출 결과
		 * @param pushSetting		푸시 설정 정보
		 *
		 * \~english Returns the result of push settings on the activated app.
		 * 
		 * @param result		API call result
		 * @param pushSetting		Push settings information
		 * \~
		 * @ingroup Push
		 */
		public delegate void onPushSetting(ResultAPI result, PushSetting pushSetting);

		/**
		 * \~korean 유저의 푸시 수신 상태 조회
		 * 
		 * @param listener	API 결과 통지
		 *
		 * \~english Request the status of receiving push notification.
		 * 
		 * @param listener	API call result
		 * \~
		 * @ingroup Push
		 */
		public static void getRemotePush(onRemotePush listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Push", "getRemotePush", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean 유저의 푸시 수신 상태 설정
		 * 
		 * @param remotePush	리모트 푸시 정보
		 * @param listener  	API 결과 통지
		 *
		 * \~english Set the status of receiving push notification.
		 * 
		 * @param remotePush	Remote push information.
		 * @param listener  	API call result
		 * \~
		 * @ingroup Push
		 */
		public static void setRemotePush(RemotePush remotePush, onRemotePush listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Push", "setRemotePush", listener);
			jsonParam.AddField ("remotePush", remotePush.TOJSON());

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean 로컬 푸시 등록. iOS에서는 시스템 제한으로 최대 64개 까지 등록 가능
		 * 
		 * @param localPush	로컬 푸시 등록 정보. 
		 * @param listener	API 결과 통지
		 *
		 * \~english Register Local push notification. In iOS, up to 64 can be registered due to the system limit.
		 * 
		 * @param localPush	Local push registration information.
		 * @param listener	API call result
		 * \~
		 * @ingroup Push
		 */
		public static void registerLocalPush(LocalPush localPush, onLocalPush listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Push", "registerLocalPush", listener);
			jsonParam.AddField ("localPush", localPush.TOJSON());

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean 로컬 푸시 해제
		 * 
		 * @param noticeID		로컬 푸시 등록 ID
		 * \~english Unregister Local push notification.
		 * 
		 * @param noticeID		Local push registration ID.
		 * \~
		 * @ingroup Push
		 */
		public static void unregisterLocalPush(int noticeID) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Push", "unregisterLocalPush", null);
			jsonParam.AddField ("noticeID", noticeID);

			HIVEUnityPlugin.callNative (jsonParam);
		}

				/**
		 * \~korean 로컬 푸시 배열 해제
		 * 
		 * @param noticeIDs		로컬 푸시 등록 ID 배열
		 * \~english Unregister Local push notification.
		 * 
		 * @param noticeIDs		Array of Local push registration ID.
		 * \~
		 * @ingroup Push
		 */
		public static void unregisterLocalPushes(List<int> noticeIDs) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Push", "unregisterLocalPushes", null);
			JSONObject jsonArray = new JSONObject();

			if(noticeIDs != null) {
				foreach(int noticeID in noticeIDs) {
					jsonArray.Add(noticeID.ToString());
				}
			}
			jsonParam.AddField ("noticeIDs", jsonArray);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 * \~korean Hive 로컬 푸시를 포함한 모든 로컬 푸시 해제
		 * 
		 * \~english Unregister All local pushes including Hive local pushes.
		 * 
		 * \~
		 * @ingroup Push
		 */
		public static void unregisterAllLocalPushes() {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Push", "unregisterAllLocalPushes", null);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 * \~korean 앱 활성화 시 푸시 수신 여부 설정 값 적용
		 * 
		 * @param setting		유저가 앱 활성화 시 푸시를 수신하는 상태
		 *
		 * \~english Set the value whether to enable push notification or not on the activated app.
		 * 
		 * @param setting		The status of receiving push notification on the activated app.
		 * \~
		 * @ingroup Push
		 */		
		 public static void setForegroundPush(PushSetting setting, onPushSetting listener) {
			
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Push","setForegroundPush", listener);
			jsonParam.AddField("pushSetting", setting.TOJSON());

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 * \~korean 앱 활성화 시 푸시 수신 여부 설정 값 조회
		 * 
		 * @param setting		유저가 앱 활성화 시 푸시를 수신하는 상태
		 *		 * \~english Get the value whether to enable push notification or not on the activated app.
		 * 
		 * @param setting		The status of receiving push notification on the activated app.
		 * \~
		 * @ingroup Push
		 */		
		 public static void getForegroundPush(onPushSetting listener) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Push","getForegroundPush", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}
		/**
     	* \~korean Push Token 명시적 권한 요청
     	*
     	* \~english Request Push Token
     	*
     	* \~
    	 * @ingroup Push
     	*/
		 public static void requestPushPermission() {
			 JSONObject jsonParam = HIVEUnityPlugin.createParam("Push","requestPushPermission", null);
			 HIVEUnityPlugin.callNative(jsonParam);
		 }


		// \internal
		// \~korean Native 영역에서 호출된 요청을 처리하기 위한 플러그인 내부 코드
		// \~english Plug-in internal code to handle requests invoked from the native code.
		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			resJsonObject.GetField (ref handlerId, "handler");
			object handler = (object)HIVEUnityPlugin.popHandler (handlerId);

			if (handler == null) return;

			if ("registerLocalPush".Equals (methodName) ||
				"unregisterLocalPush".Equals(methodName)) {

				onLocalPush listener = (onLocalPush)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), new LocalPush (resJsonObject.GetField ("localPush")));
			}
			else if ("getRemotePush".Equals (methodName) ||
				"setRemotePush".Equals (methodName)) {
				
				onRemotePush listener = (onRemotePush)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), new RemotePush (resJsonObject.GetField ("remotePush")));
			}
			else if ("setForegroundPush".Equals(methodName) || 
					"getForegroundPush".Equals(methodName)) {

						onPushSetting listener = (onPushSetting)handler;
						listener (new ResultAPI(resJsonObject.GetField("resultAPI")), new PushSetting(resJsonObject.GetField ("pushSetting")));
				}
		}

	}




	/**
	 * \~korean 유저가 푸시를 수신하는 상태 정보
	 * 
	 * \~english Information about the status of receiving push notification.
	 * \~
	 * @ingroup Push
	 */
	public class RemotePush {

		public Boolean isAgreeNotice;		///< \~korean 공지 푸시 수신 허용 여부 \~english Whether to receive announcement notification or not.

		public Boolean isAgreeNight;		///< \~korean 야간 푸시 수신 허용 여부 \~english Whether to receive night-time notification or not. 


		public RemotePush() {
		}


		public RemotePush(Boolean isAgreeNotice, Boolean isAgreeNight) {

			this.isAgreeNotice = isAgreeNotice;
			this.isAgreeNight = isAgreeNight;
		}


		public RemotePush(JSONObject resJsonParam) {

			if(resJsonParam == null || resJsonParam.count <= 0)
				return;

			resJsonParam.GetField (ref this.isAgreeNotice, "isAgreeNotice");
			resJsonParam.GetField (ref this.isAgreeNight, "isAgreeNight");
		}


		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("RemotePush {");
			sb.Append("isAgreeNotice = ");
			sb.Append(this.isAgreeNotice);
			sb.Append(", isAgreeNight = ");
			sb.Append(this.isAgreeNight);
			sb.Append (" }\n");

			return sb.ToString();
		}

		public JSONObject TOJSON() {
			
			JSONObject jsonObject = new JSONObject();

			jsonObject.AddField("isAgreeNotice", this.isAgreeNotice);
			jsonObject.AddField("isAgreeNight", this.isAgreeNight);

			return jsonObject;
		}
	}


	/**
	 * \~korean 로컬 푸시 등록 정보
	 * 
	 * \~english Local push registration information.
	 * \~
	 * @ingroup Push
	 */
	public class LocalPush {

		public int noticeId;			///< \~korean 로컬 푸시 통지 ID \~english Local push notification ID

		public String title;			///< \~korean 로컬 푸시 타이틀 \~english Local push title

		public String msg;				///< \~korean 로컬 푸시 메시지 \~english Local push message

		public long after;				///< \~korean 알림 시점 \~english Notification timer

		public String groupId;          ///< \~korean 알림 그룹 ID \~english Notification Group ID


		// \~korean 이하 Android에서 로컬 푸시를 커스터마이징하기 위한 필드 정의
		// \~english Followings are field definition for customizing local push on Android.

		public String bigmsg;			///< \~korean 큰 글씨 \~english Big-text

		public String ticker;			///< \~korean 메시지 티커 \~english Message Ticker

		public String type;				///< \~korean 알림 형태 (bar, popup, toast 등) \~english Notification type (bar, popup, toast etc.)

		public String icon;				///< \~korean 푸시 아이콘 \~english Push icon

		public String sound;			///< \~korean 푸시 알림음 \~english Notification sound

		public String active;			///< \~korean 수행할 동작 \~english Action to take

		public String broadcastAction;
		public int buckettype;
		public int bucketsize;
		public String bigpicture;
		public String icon_color;

		public LocalPush() {
		}


		public LocalPush(int noticeId, String title, String msg, long after) {

			this.noticeId = noticeId;
			this.title = title;
			this.msg = msg;
			this.after = after;
		}


		public LocalPush(JSONObject resJsonParam) {

			if(resJsonParam == null || resJsonParam.count <= 0)
				return;
			
			resJsonParam.GetField (ref this.noticeId, "noticeId");
			resJsonParam.GetField (ref this.title, "title");
			resJsonParam.GetField (ref this.msg, "msg");
			resJsonParam.GetField (ref this.after, "after");
			resJsonParam.GetField (ref this.groupId, "groupId");

			resJsonParam.GetField (ref this.bigmsg, "bigmsg");
			resJsonParam.GetField (ref this.ticker, "ticker");

			resJsonParam.GetField (ref this.type, "type");
			resJsonParam.GetField (ref this.icon, "icon");
			resJsonParam.GetField (ref this.sound, "sound");
			resJsonParam.GetField (ref this.active, "active");

			resJsonParam.GetField (ref this.broadcastAction, "broadcastAction");
			resJsonParam.GetField (ref this.buckettype, "buckettype");
			resJsonParam.GetField (ref this.bucketsize, "bucketsize");
			resJsonParam.GetField (ref this.bigpicture, "bigpicture");
			resJsonParam.GetField (ref this.icon_color, "icon_color");
		}


		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("LocalPush { noticeId = ");
			sb.Append(this.noticeId);
			sb.Append(", title = ");
			sb.Append(this.title);
			sb.Append(", msg = ");
			sb.Append(this.msg);
			sb.Append(", after = ");
			sb.Append(this.after);
			sb.Append (" }\n");

			return sb.ToString();
		}

		public JSONObject TOJSON() {
			
			JSONObject jsonObject = new JSONObject();

			jsonObject.AddField("active", this.active);
			jsonObject.AddField("after", this.after);
			jsonObject.AddField("bigmsg", this.bigmsg);
			jsonObject.AddField("bigpicture", this.bigpicture);
			jsonObject.AddField("broadcastAction", this.broadcastAction);
			jsonObject.AddField("bucketsize", this.bucketsize);
			jsonObject.AddField("buckettype", this.buckettype);
			jsonObject.AddField("icon", this.icon);
			
			if (!string.IsNullOrEmpty(this.icon_color)) {
				JSONObject jsonIconColor = new JSONObject(this.icon_color);
				jsonObject.AddField("icon_color", jsonIconColor);
			}
			
			jsonObject.AddField("msg", this.msg);
			jsonObject.AddField("noticeID", this.noticeId);
			jsonObject.AddField("sound", this.sound);
			jsonObject.AddField("ticker", this.ticker);
			jsonObject.AddField("title", this.title);
			jsonObject.AddField("type", this.type);
			jsonObject.AddField("groupId", this.groupId);

			return jsonObject;
		}
	}

	/**
	 * \~korean 푸시 설정 정보
	 * 
	 * \~english Push settings information
	 * \~
	 * @ingroup Push
	 */
	public class PushSetting {

		
		public Boolean useForegroundRemotePush;		///< \~korean 앱 활성화 시 리모트 푸시 수신 여부 \~english Whether to enable remote notification or not on the activated app.

		public Boolean useForegroundLocalPush;		///< \~korean 앱 활성화 시 로컬 푸시 수신 여부 \~english Whether to enable local notification on the activated app.


		public PushSetting() {
		}


		public PushSetting(Boolean useForegroundRemotePush, Boolean useForegroundLocalPush) {

			this.useForegroundRemotePush = useForegroundRemotePush;
			this.useForegroundLocalPush = useForegroundLocalPush;
		}


		public PushSetting(JSONObject resJsonParam) {

			if(resJsonParam == null || resJsonParam.count <= 0)
				return;

			resJsonParam.GetField (ref this.useForegroundRemotePush, "useForegroundRemotePush");
			resJsonParam.GetField (ref this.useForegroundLocalPush, "useForegroundLocalPush");
		}


		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("PushSetting {");
			sb.Append("useForegroundRemotePush = ");
			sb.Append(this.useForegroundRemotePush);
			sb.Append(", useForegroundLocalPush = ");
			sb.Append(this.useForegroundLocalPush);
			sb.Append (" }\n");

			return sb.ToString();
		}

		public JSONObject TOJSON() {
			
			JSONObject jsonObject = new JSONObject();

			jsonObject.AddField("useForegroundRemotePush", this.useForegroundRemotePush);
			jsonObject.AddField("useForegroundLocalPush", this.useForegroundLocalPush);

			return jsonObject;
		}
	}

}


/** @} */



