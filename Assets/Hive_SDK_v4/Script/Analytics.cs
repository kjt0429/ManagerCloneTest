/**
 * @file    Analytics.cs
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
 * @defgroup Analytics
 * @{
 * \~korean  앱과 사용자를 트래킹하고 분석하기 위한 기능 모음<br/>
 * (User Tracking Tool Wrapper & Callect Analytics Log API)<br/><br/>
 * \~english A collection of features for tracking and analyzing apps and users<br/>
 * (User Tracking Tool Wrapper & Callect Analytics Log API)<br/><br/>
 */

 namespace hive
{
	/**
	 * \~korean 앱과 사용자를 트래킹하고 분석하기 위한 기능 모음<br/>
	 * (User Tracking Tool Wrapper & Callect Analytics Log API)<br/><br/>
	 *    
	 * \~english A collection of features for tracking and analyzing apps and users<br/>
	 * (User Tracking Tool Wrapper & Callect Analytics Log API)<br/><br/>
	 * \~
	 * @since		4.0.0 
	 * @author ryuvsken
	 * @ingroup Analytics
	 */
	public class Analytics {

		/**
		 * \~korean 분석용 로그 전송.
		 * 
		 * @param logData 로그 데이터
		 * 
		 * @return bool 로그 큐가 가득차 로그를 쌓지 못했을 경우 false를 반환한다.
		 * 
		 * \~english Send log for analysis.
		 * 
		 * @param logData Log data
		 *
		 * @return bool Returns false if the log queue is full and the log is not stacked.
		 *
		 * \~
		 * @ingroup Analytics
		 */
		public static bool sendAnalyticsLog(JSONObject logData) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Analytics", "sendAnalyticsLog", null);
			jsonParam.AddField ("logData", logData);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

			Boolean isSendAnalyticsLog = false;
			resJsonObject.GetField (ref isSendAnalyticsLog, "sendAnalyticsLog");
			return isSendAnalyticsLog;
		}


		/**
		 * \~korean 사용자 분석을 위한 사용자 정보 트래커 사용 유무 설정
		 * 
		 * @param trackingType 사용자 분석을 위한 사용자 정보 트래커 형태
		 * @param isEnable 트래커 사용 유무
		 * 
		 * \~english Set whether to use User Information Tracker for user analysis
		 * 
		 * @param trackingType User information tracker type
		 * @param isEnable whether to use User Information Tracker 
		 * \~
		 * @ingroup Analytics
		 */
		[System.Obsolete("This is an obsolete method")]
		public static void setEnableTracker(TrackingType trackingType, Boolean isEnable) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Analytics", "setEnableTracker", null);
			jsonParam.AddField ("trackingType", trackingType.ToString());
			jsonParam.AddField ("isEnable", isEnable);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean 사용자 분석을 위한 사용자 정보 트래커 사용 유무 설정
		 * 
		 * @param name 사용자 분석을 위한 사용자 정보 트래커 형태
		 * @param isEnable 트래커 사용 유무
		 * 
		 * \~english Set whether to use User Information Tracker for user analysis
		 * 
		 * @param name User information tracker type
		 * @param isEnable whether to use User Information Tracker 
		 * \~
		 * @ingroup Analytics
		 */
		public static void setEnableTracker(String name, Boolean isEnable) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Analytics", "setEnableTrackerWithName", null);
			jsonParam.AddField ("name", name);
			jsonParam.AddField ("isEnable", isEnable);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean 사용자 분석을 위한 사용자 정보 이벤트 설정
		 * 
		 * @param event	사용자 정보 이벤트
		 * 
		 * \~english Send event for user analysis
		 * 
		 * @param event	Event name
		 * \~
		 * @ingroup Analytics
		 */
		public static void sendEvent(String eventName) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Analytics", "sendEvent", null);
			jsonParam.AddField ("eventName", eventName);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		public static void sendTutorialComplete() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Analytics", "sendTutorialComplete", null);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		public static uint getRemainAnalyticsLogCount() {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Analytics", "getRemainAnalyticsLogCount", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			uint resultValue = 0;
			resJson.GetField (ref resultValue, "getRemainAnalyticsLogCount");
			return (uint)resultValue;
		}

		/**
		 * \~korean 광고 수익 측정 이벤트 설정
		 * 
		 * @param analyticsAdRevenue	광고 수익 측정 데이터
		 * 
		 * \~english Send events for ad revenue measurement
		 * 
		 * @param analyticsAdRevenue	Ad revenue data
		 * \~
		 * @ingroup Analytics
		 */
		public static void sendAdRevenueEvent(AnalyticsAdRevenue analyticsAdRevenue) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Analytics", "sendAdRevenueEvent", null);
			jsonParam.AddField ("analyticsAdRevenue", analyticsAdRevenue.TOJSON());

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		* \~korean
		* @brief 사용자의 퍼널 분석을 위한 지표를 전송한다.
		*
		* 사전정의된 퍼널 목록
		* <table>
		*     <th>
		*         <td>구간명</td><td>퍼널값</td><td>HIVE SDK 자동전송 유무</td>
		*     </th>
		*     <tr>
		*         <td>권한고지</td><td>410</td><td>YES</td>
		*     </tr>
		*     <tr>
		*         <td>고지팝업</td><td>420</td><td>YES</td>
		*     </tr>
		*     <tr>
		*         <td>약관동의</td><td>430</td><td>YES</td>
		*     </tr>
		*     <tr>
		*         <td>게임서버선택</td><td>500</td><td>YES</td>
		*     </tr>
		*     <tr>
		*         <td>서버점검팝업</td><td>600</td><td>YES</td>
		*     </tr>
		*     <tr>
		*         <td>추가다운로드</td><td>700</td><td><b>NO</b></td>
		*     </tr>
		*     <tr>
		*         <td>추가다운로드 완료</td><td>800</td><td><b>NO</b></td>
		*     </tr>
		*     <tr>
		*         <td>로그인</td><td>900</td><td>YES</td>
		*     </tr>
		*     <tr>
		*         <td>전면배너</td><td>1000</td><td>YES</td>
		*     </tr>
		* </table>
		*
		* @param funnelTrack 사전정의된 퍼널의 값
		* @param optionTag 옵션으로 추가 전달할 값
		*/
		public static void sendUserEntryFunnelsLogs(String funnelTrack, String optionTag) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Analytics", "sendUserEntryFunnelsLogs", null);
			jsonParam.AddField ("funnelTrack", funnelTrack);
			jsonParam.AddField ("optionTag", optionTag);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			resJsonObject.GetField (ref handlerId, "handler");
			//object handler = (object)HiveUnityPlugin.popHandler (handlerId);

		}
	}




	/**
	 * \~korean 사용자 분석을 위한 서드 파티 트래커 형태
	 * 
	 * \~english Tracker type for user analysis
	 * \~
	 * @ingroup Analytics
	 * @author ryuvsken
	 */
	[System.Obsolete("This is an obsolete enum")]
	public enum TrackingType {

		ADJUST,
		SINGULAR,
		APPSFLYER,
	};

	public class AnalyticsAdRevenue {
		
		public double revenue = 0.0;                ///< \~korean  광고 노출당 발생하는 광고 수익  \~english Ad impression revenue
		public String adPlatform = "";              ///< \~korean  광고 네트워크 플랫폼 \~english Ad network platform
		public String adUnitId = "";                ///< \~korean 광고 유닛 아이디  \~english Ad Unit Id
		public String adType = "";                  ///< \~korean 광고 노출 타입  \~english  The type of ads
		public String adPlacement = "";             ///< \~korean 광고 노출 위치  \~english ad placement
		public String currency = "USD";             ///< \~korean 통화 코드(ISO_4217 형식 문자열)  \~english ISO_4217 format string (ex. "USD")

		public AnalyticsAdRevenue() {}

		public AnalyticsAdRevenue(double revenue, String adPlatform, String adUnitId, String adType, String adPlacement, String currency) {

			this.revenue = revenue;
			this.adPlatform = adPlatform;
			this.adUnitId = adUnitId;
			this.adType = adType;
			this.adPlacement = adPlacement;
			this.currency = currency;
		}

		public AnalyticsAdRevenue(JSONObject jsonParam) {

			if (jsonParam == null || jsonParam.count <= 0) return;

			jsonParam.GetField (ref this.revenue, "revenue");
			jsonParam.GetField (ref this.adPlatform, "adPlatform");
			jsonParam.GetField (ref this.adUnitId, "adUnitId");
			jsonParam.GetField (ref this.adType, "adType");
			jsonParam.GetField (ref this.adPlacement, "adPlacement");
			jsonParam.GetField (ref this.currency, "currency");
		}

		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("AnalyticsAdRevenue { revenue = ");
			sb.Append(this.revenue);
			sb.Append(", adPlatform = ");
			sb.Append(this.adPlatform);
			sb.Append(", adUnitId = ");
			sb.Append(this.adUnitId);
			sb.Append(", adType = ");
			sb.Append(this.adType);
			sb.Append(", adPlacement = ");
			sb.Append(this.adPlacement);
			sb.Append(", currency = ");
			sb.Append(this.currency);
			sb.Append(" }\n");

			return sb.ToString();
		}

		public JSONObject TOJSON() {
		
			JSONObject jsonObject = new JSONObject();

			jsonObject.AddField("revenue", this.revenue);
			jsonObject.AddField("adPlatform", this.adPlatform);
			jsonObject.AddField("adUnitId", this.adUnitId);
			jsonObject.AddField("adType", this.adType);
			jsonObject.AddField("adPlacement", this.adPlacement);
			jsonObject.AddField("currency", this.currency);

			return jsonObject;
		}
	}
}


/** @} */



