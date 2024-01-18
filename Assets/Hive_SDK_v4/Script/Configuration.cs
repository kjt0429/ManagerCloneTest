/**
 * @file    Configuration.cs
 * 
 *  @date		2016-2022
 *  @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 *  @author		ryuvsken
 *  @since		4.0.0
 */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

/**
 * @defgroup	Configuration
 * @{
 * \~korean
 * HIVE SDK 설정 관리<br/><br/>
 * \~english
 * This class manage HIVE SDK configuration<br/><br/>
 * \~
 *  
 */

namespace hive
{
	/**
	 * \~korean
	 * HIVE SDK 설정 관리<br/><br/>
	 * \~english
	 * This class manage HIVE SDK configuration<br/><br/>
	 * \~
	 * @since		4.0.0
	 * @ingroup Configuration
	 * @author ryuvsken
	 */
	public class Configuration {

		/**
     	*  \~korean
     	* @brief MetaData 요청 결과 통지<br/>
     	*
     	* @param result        API 호출 결과
     	* @param value         key에 매칭된 metadata 값
     	*
     	*  \~english
     	* @brief MetaData request result callback<br/>
     	*
     	* @param result        Result of API call
     	* @param value         The metadata value that matches the key
     	*
     	*  \~
     	* @see #getMetaData(String, onConfigurationGetMetaData)
     	*
     	* @ingroup Configuration
     	*/
    	public delegate void onConfigurationGetMetaData(ResultAPI result, String value);


		public static String getConfiguration() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getConfiguration", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			JSONObject jsonConfiguration = resJson.GetField ("getConfiguration");
			if (jsonConfiguration != null)
				return jsonConfiguration.ToString ();
			else
				return "";
		}


		/**
		 * \~korean 
	 	 * @brief Hive SDK 버전 반환
		 * 
		 * @return Hive SDK 버전
		 * \~english 
	 	 * @brief Returns HIVE SDK Version
		 *
		 * @return HIVE SDK version
		 * \~
		 * @ingroup Configuration
		 */
		public static String getHiveSDKVersion() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getHiveSDKVersion", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "getHiveSDKVersion");
			return resultValue;
		}


		/**
		 * \~korean 
	 	 * @brief HIVE SDK 가 참조하고 있는 SDK 의 버전 반환
		 * 
		 * @return HIVE SDK 가 참조하고 있는 SDK 의 버전
		 * \~english 
	 	 * @brief Return version of SDK referenced by HIVE SDK
		 * 
		 * @return Version of SDK referenced by HIVE SDK
		 * \~
		 * @ingroup Configuration
		 */
		public static String getReferenceSDKVersion() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getReferenceSDKVersion", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "getReferenceSDKVersion");
			return resultValue;
		}


		/**
		 * \~korean AppId 반환<br/>
		 * (AppId 는 기본적으로 AndroidManifest.xml 파일의 package 값으로 설정하게 된다.<br/>
		 * 그러나 테스트 설정등의 이유로 API 호출에 대한 변경을 지원한다.)
		 * 
		 * @return AppId
		 * \~english Returns AppId
		 * (By default, AppId will be set to the package name in the AndroidManifest.xml file. However, it supports changes to API calls for reasons such as test setup.)
		 *
		 * @return AppId
		 * \~
		 * @ingroup Configuration
		 */
		public static String getAppId() {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getAppId", null);
			
			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "getAppId");
			return resultValue;
		}


		/**
		 * \~korean AppId 설정<br/>
		 * (AppId 는 기본적으로 AndroidManifest.xml 파일의 package 값으로 설정하게 된다.<br/>
		 * 그러나 테스트 설정등의 이유로 API 호출에 대한 변경을 지원한다.)
		 * 
		 * @param appId AppId
		 * \~english Set AppId 
		 * (By default, AppId will be set to the package name in the AndroidManifest.xml file. However, it supports changes to API calls for reasons such as test setup.)
		 *
		 * @param appId AppId
		 * \~
		 * @ingroup Configuration
		 */
		public static void setAppId(String appId) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setAppId", null);
			jsonParam.AddField ("setAppId", appId);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		// GCPSDK4-284
		public static String getHiveCertificationKey() {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getHiveCertificationKey", null);
			
			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "getHiveCertificationKey");
			return resultValue;
		}


		// GCPSDK4-284
		public static void setHiveCertificationKey(String appKey) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setHiveCertificationKey", null);
			jsonParam.AddField ("appKey", appKey);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean Hive 플랫폼 서버 존 반환
		 * 
		 * @return Hive SDK 플랫폼 서버 존 (sandbox : 개발용, real : 실계용)
		 * \~english Returns Hive platform server zone
		 *
		 * @return Hive platform server zone (sandbox : for development, real : for production)
		 * \~
		 * @ingroup Configuration
		 */
		public static ZoneType getZone() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getZone", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "getZone");

			if ("TEST".Equals (resultValue))
				return ZoneType.TEST;
			else if ("REAL".Equals (resultValue))
				return ZoneType.REAL;
			else if ("DEV".Equals (resultValue))
				return ZoneType.DEV;
			else
				return ZoneType.SANDBOX;
		}


		/**
		 * \~korean Hive 플랫폼 서버 존 설정
		 * 
		 * @param zone Hive SDK 플랫폼 서버 존 (sandbox : 개발용, real : 실계용)
		 * \~english Set Hive Hive platform server zone
		 *
		 * @param zone Hive platform server zone (sandbox : for development, real : for production)
		 * \~
		 * @ingroup Configuration
		 */
		public static void setZone(ZoneType zone) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setZone", null);
			jsonParam.AddField ("setZone", zone.ToString());

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean 서버별 점검공지 팝업지원을 위한 serverId 반환<br/>
		 * (백오피스 월드관리에 등록된 월드값을 서버에 따라 구분하여 입력이 되어야 한다.)
		 * 
		 * @return 서버별 점검공지 팝업지원을 위한 serverId
		 * \~english Return serverId for server-specific maintenance popup support<br>
		 * (The world value registered in the back office world management)
		 *
		 * @return serverId Server ID for server-specific maintenance popup support
		 * \~
		 * @ingroup Configuration
		 */
		public static String getServerId() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getServerId", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "getServerId");
			return resultValue;
		}


		/**
		 * \~korean 서버별 점검공지 팝업지원을 위한 serverId 설정<br/>
		 * (백오피스 월드관리에 등록된 월드값을 서버에 따라 구분하여 입력이 되어야 한다.)
		 *
		 * @param serverId 서버별 점검공지 팝업지원을 위한 serverId
		 * \~english Set serverId for server-specific maintenance popup support<br>
		 * (The world value registered in the back office world management should be inputted according to the server.)
		 *
		 * @param serverId serverId for server-specific maintenance popup support
		 * \~
		 * @ingroup Configuration
		 */
		public static void setServerId(String serverId) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setServerId", null);
			jsonParam.AddField ("setServerId", serverId);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean 서버별 점검공지 팝업지원을 위한 serverId 설정<br/>
		 * (백오피스 월드관리에 등록된 월드값을 서버에 따라 구분하여 입력이 되어야 한다.)
		 *
		 * @param serverId 서버별 점검공지 팝업지원을 위한 serverId
		 * \~english Set serverId for server-specific maintenance popup support<br>
		 * (The world value registered in the back office world management should be inputted according to the server.)
		 *
		 * @param serverId serverId for server-specific maintenance popup support
		 * \~
		 * @ingroup Configuration
		 */
		public static void updateServerId(String serverId) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "updateServerId", null);
			jsonParam.AddField ("updateServerId", serverId);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean 게임에서 사용하는 언어코드를 모듈에도 반영하기 위한 API<br/>
		 * 2자리 소문자로 ("en") 입력해주면 된다. ISO 639-1 형식.
		 * <p>
		 * 내부적으로 대문자도 소문자로 변환된다. 알파벳 대소문자 규칙은 US 규칙을 따른다.
		 * 
		 * @param gameLanguage 게임에서 사용하는 언어코드
		 * \~english API to reflect language code used in game in module<br>
		 * Just type in two lowercase letters ("en"). ISO 639-1 format.<br>
		 * <p>
		 * Internally, uppercase characters are converted to lowercase characters. Alphabetic case rules follow US rules.
		 * 
		 * @param gameLanguage Language code used in game.
		 * \~
		 * @ingroup Configuration
		 */
		public static String setGameLanguage(String gameLanguage) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setGameLanguage", null);
			jsonParam.AddField ("setGameLanguage", gameLanguage);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "setGameLanguage");
			return resultValue;
		}


		/**
		 * \~korean 게임에서 사용하는 언어코드를 모듈에도 반영하기 위한 API<br/>
		 * 2자리 소문자로 ("en") 입력해주면 된다. ISO 639-1 형식.
		 * <p>
		 * 내부적으로 대문자도 소문자로 변환된다. 알파벳 대소문자 규칙은 US 규칙을 따른다.
		 * 
		 * @param gameLanguage 게임에서 사용하는 언어코드
		 * \~english API to reflect language code used in game in module<br>
		 * Just type in two lowercase letters ("en"). ISO 639-1 format.<br>
		 * <p>
		 * Internally, uppercase characters are converted to lowercase characters. Alphabetic case rules follow US rules.
		 * 
		 * @param gameLanguage Language code used in game.
		 * \~
		 * @ingroup Configuration
		 */
		public static String updateGameLanguage(String gameLanguage) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "updateGameLanguage", null);
			jsonParam.AddField ("updateGameLanguage", gameLanguage);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "updateGameLanguage");
			return resultValue;
		}


		/**
		 * \~korean Hive SDK 내부 로그 사용 여부 반환
		 * 
		 * @return Hive SDK 내부 로그 사용 여부
		 * \~english Returns whether HIVE SDK internal log is used
		 *
		 * @return Whether HIVE SDK internal log is used
		 * \~
		 * @ingroup Configuration
		 */
		public static Boolean getUseLog() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getUseLog", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			Boolean resultValue = false;
			resJson.GetField (ref resultValue, "getUseLog");
			return resultValue;
		}


		/**
		 * \~korean Hive SDK 내부 로그 사용 여부 설정
		 * 
		 * @param useLog Hive SDK 내부 로그 사용 여부
		 * \~english Set whether HIVE SDK internal log is used
		 *
		 * @param useLog Whether HIVE SDK internal log is used
		 * \~
		 * @ingroup Configuration
		 */
		public static void setUseLog(Boolean useLog) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setUseLog", null);
			jsonParam.AddField ("setUseLog", useLog);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 * 권한 요청 관련 설정 정보 반환
		 * 
		 * @return 권한 요청 관련 설정 정보
		 * \~english Return permission information. 
		 * 
		 * @return Permission string
		 * \~
		 * @ingroup Configuration
		 */
		public static String getPermissions() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getPermissions", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			JSONObject jsonPermissions;
			jsonPermissions = resJson.GetField ("getPermissions");
			if (jsonPermissions != null)
				return jsonPermissions.ToString ();
			else
				return "";
		}


		/**
		 * 권한 요청 관련 설정 정보 설정
		 * 
		 * @param permissions 권한 요청 관련 설정
		 * \~english Set permissions.
		 * 
		 * @param permissions Permissions.
		 * \~
		 * @ingroup Configuration
		 */
		public static void setPermissions(JSONObject permissions) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setPermissions", null);
			jsonParam.AddField ("setPermissions", permissions);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 * \~korean 회사 반환
		 *
		 * @return 회사 (C2S : 컴투스, GVI : 컴투스홀딩스)
		 * \~english Returns company.
		 *
		 * @return Company (C2S : Com2us, GVI : Com2us Holdings)
		 */
		public static String getCompany() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getCompany", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "getCompany");
			return resultValue;
		}


		/**
		 * \~korean 회사 설정
		 *
		 * @param company (C2S : 컴투스, GVI : 컴투스 홀딩스)
		 * \~english Set company
	     *
	     * @param company (C2S : Com2us, GVI : Com2us Holdings)
		 * \~
		 * @ingroup Configuration
		 */
		public static void setCompany(String company) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setCompany", null);
			jsonParam.AddField ("setCompany", company);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean 채널 반환
		 *
		 * @return channel (C2S : HIVE 플랫폼)
		 * \~english Returns channel
	     *
	     * @return channel (C2S : HIVE Platform)
		 * \~
		 * @ingroup Configuration
		 */
		public static String getChannel() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getChannel", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "getChannel");
			return resultValue;
		}


		/**
		 * \~korean 채널 설정
		 *
		 * @param 채널 (C2S : HIVE 플랫폼)
		 * \~english Set channel
		 *
		 * @param Channel (C2S : HIVE Platform)
		 * \~
		 * @ingroup Configuration
		 */
		public static void setChannel(String channel) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setChannel", null);
			jsonParam.AddField ("setChannel", channel);

			HIVEUnityPlugin.callNative (jsonParam);
		}




		/**
		 * \~korean HTTP Connect Timeout 의 기본 설정 값 반환 (초단위)
		 * 
		 * @return HTTP Timeout 의 기본 설정 값 (초단위)
		 * \~english Returns the default value of HTTP Connect Timeout (in seconds)
		 *
		 * @return Default value of HTTP Connect Timeout (in seconds)
		 * \~
		 * @ingroup Configuration
		 */
		public static int getHttpConnectTimeout() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getHttpConnectTimeout", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			int resultValue = 10;
			resJson.GetField (ref resultValue, "getHttpConnectTimeout");
			return resultValue;
		}


		/**
		 * \~korean HTTP Connect Timeout 의 기본 설정 값 설정 (초단위)
		 * 
		 * @param httpConnectTimeout HTTP Connect Timeout 의 기본 설정 값 (초단위)
		 * \~english Set the value of HTTP Connect Timeout (in seconds)
		 *
		 * @param httpConnectTimeout Value of HTTP Connect Timeout (in seconds)
		 * \~
		 * @ingroup Configuration
		 */
		public static void setHttpConnectTimeout(int httpConnectTimeout) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setHttpConnectTimeout", null);
			jsonParam.AddField ("setHttpConnectTimeout", httpConnectTimeout);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean HTTP Read Timeout 의 기본 설정 값 반환 (초단위)
		 * 
		 * @return HTTP Timeout 의 기본 설정 값 (초단위)
		 * \~english Returns the default value of HTTP Read Timeout (in seconds)
		 *
		 * @return Default value of HTTP Read Timeout (in seconds)
		 * \~
		 * @ingroup Configuration
		 */
		public static int getHttpReadTimeout() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getHttpReadTimeout", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			int resultValue = 15;
			resJson.GetField (ref resultValue, "getHttpReadTimeout");
			return resultValue;
		}


		/**
		 * \~korean HTTP Read Timeout 의 기본 설정 값 설정 (초단위)
		 * 
		 * @param httpReadTimeout HTTP Read Timeout 의 기본 설정 값 (초단위)
		 * \~english Set the value of HTTP Read Timeout (in seconds)
		 *
		 * @param httpReadTimeout Value of HTTP Read Timeout (in seconds)
		 * \~
		 * @ingroup Configuration
		 */
		public static void setHttpReadTimeout(int httpReadTimeout) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setHttpReadTimeout", null);
			jsonParam.AddField ("setHttpReadTimeout", httpReadTimeout);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean 게임 로그 최대 저장 갯수 반환
		 * 
		 * @return 게임 로그 최대 저장 갯수
		 * \~english Returns maximum number of game logs
		 *
		 * @return Maximum number of game logs
		 * \~
		 * @ingroup Configuration
		 */
		public static int getMaxGameLogSize() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getMaxGameLogSize", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			int resultValue = 50;
			resJson.GetField (ref resultValue, "getMaxGameLogSize");
			return resultValue;
		}


		/**
		 * \~korean 게임 로그 최대 저장 갯수 설정<br/>
		 * (특별한 경우가 아니면 변경 금지)
		 * 
		 * @param maxGameLogSize 게임 로그 최대 저장 갯수
		 * \~english Set maximum number of game logs
		 * (Note: No change unless special occasion)
		 *
		 * @param maxGameLogSize Maximum number of game logs
		 * \~
		 * @ingroup Configuration
		 */
		public static void setMaxGameLogSize(int maxGameLogSize) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setMaxGameLogSize", null);
			jsonParam.AddField ("setMaxGameLogSize", maxGameLogSize);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean 결제 마켓 반환
		 * 
		 * @return 결제 마켓 (google, tstore, olleh, ozstore, googleplay_lebi)
		 * \~english Return market
		 * 
		 * @return Market (google, tstore, olleh, ozstore, googleplay_lebi)
		 * \~
		 * @ingroup Configuration
		 */
		public static String getMarket() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getMarket", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "getMarket");
			return resultValue;
		}


		/**
		 * \~korean 결제 마켓 설정
		 * 
		 * @param market 결제 마켓 (google, tstore, olleh, ozstore, googleplay_lebi)
		 * \~english Set market
		 * 
		 * @param market Market (google, tstore, olleh, ozstore, googleplay_lebi)
		 * \~
		 * @ingroup Configuration
		 */
		public static void setMarket(String market) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setMarket", null);
			jsonParam.AddField ("setMarket", market);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 *  \~korean HIVE 서버에서 판단한 국가코드를 반환한다.
		 *
		 * @return ISO
		 *  \~english HIVE 서버에서 판단한 국가코드를 반환한다.
		 *  
		 * @return ISO
		 * \~
		 * @ingroup Configuration
		 */
		public static String getHiveCountry() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getHiveCountry", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "getHiveCountry");
			return resultValue;
		}

		/**
		 *  \~korean HIVE 서버에서 판단한 TimeZone 정보를 반환한다.
		 *
		 * @return JSON String
		 *  \~english HIVE 서버에서 판단한 TimeZone 정보를 반환한다.
		 *  
		 * @return JSON String
		 * \~
		 * @ingroup Configuration
		 */
		public static String getHiveTimeZone() {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getHiveTimeZone", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "getHiveTimeZone");
			return resultValue;
		}

		/**
		* 전송 주기마다 전송할 로그의 최대치를 반환한다.
		*
		* @return uint 전송 주기마다 전송할 로그의 최대치.
		*
		* @ingroup Configuration
		*/
		public static uint getAnalyticsSendLimit() {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getAnalyticsSendLimit", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			uint resultValue = 0;
			resJson.GetField (ref resultValue, "getAnalyticsSendLimit");
			return resultValue;
		}
		
		/**
		* 전송 주기마다 전송할 로그의 최대치 설정.
		*
		* @param limit 전송주기마다 전송할 최대 로그의 양
		*
		* @ingroup Configuration
		*/
		public static void setAnalyticsSendLimit(uint limit) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setAnalyticsSendLimit", null);
			jsonParam.AddField ("setAnalyticsSendLimit", limit);

			HIVEUnityPlugin.callNative (jsonParam);
		}
		
		/**
		* 최대로 쌓을수 있는 로그의 양을 반환한다.
		*
		* @return uint 최대로 쌓을수 있는 로그의 양
		*
		* @ingroup Configuration
		*/
		public static uint getAnalyticsQueueLimit() {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getAnalyticsQueueLimit", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			uint resultValue = 0;
			resJson.GetField (ref resultValue, "getAnalyticsQueueLimit");
			return resultValue;
		}
		
		/**
		* 최대로 쌓을 수 있는 로그의 수
		*
		* @param limit 최대 대기 가능한 로그의 수
		*
		* @ingroup Configuration
		*/
		public static void setAnalyticsQueueLimit(uint limit) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setAnalyticsQueueLimit", null);
			jsonParam.AddField ("setAnalyticsQueueLimit", limit);

			HIVEUnityPlugin.callNative (jsonParam);
		}
		
		/**
		* 로그 전송 주기.
		*
		* @return float 전송주기
		*
		* @ingroup Configuration
		*/
		public static float getAnalyticsSendCycleSeconds() {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getAnalyticsSendCycleSeconds", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			double resultValue = 0;
			resJson.GetField (ref resultValue, "getAnalyticsSendCycleSeconds");
			return (float)resultValue;
		}
		
		/**
		* 로그 전송 주기 설정.
		*
		* @param seconds 전송 주기 (초)
		*
		* @ingroup Configuration
		*/
		public static void setAnalyticsSendCycleSeconds(float seconds) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setAnalyticsSendCycleSeconds", null);
			jsonParam.AddField ("setAnalyticsSendCycleSeconds", seconds);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		public static void setSystemUI(int uiFlags) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setSystemUI", null);
			jsonParam.AddField ("setSystemUI", uiFlags);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		public static int getSystemUI() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getSystemUI", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			int resultValue = 0x0;
			resJson.GetField (ref resultValue, "getSystemUI");
			return resultValue;
		}

		/**
		 * Hive SDK AgeGateU13 적용 여부 반환
		 *
		 * @return Hive SDK AgeGateU13 적용 여부
		 */
		public static Boolean getAgeGateU13() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getAgeGateU13", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			Boolean resultValue = false;
			resJson.GetField (ref resultValue, "getAgeGateU13");
			return resultValue;
		}


		/**
		 * Hive SDK AgeGateU13 적용 여부 설정
		 *
		 * @param ageGateU13 Hive SDK AgeGateU13 적용 여부 설정
		 */
		public static void setAgeGateU13(Boolean ageGateU13) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setAgeGateU13", null);
			jsonParam.AddField ("setAgeGateU13", ageGateU13);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 * Hive SDK 권한고지 팝업 노출 여부 설정
		 *
		 * @param isOn Hive SDK 권한고지 팝업 노출 여부 설정
		 */		
		 public static void setHivePermissionViewOn(Boolean isOn) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setHivePermissionViewOn", null);
			jsonParam.AddField("isOn", isOn);
			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 * 커스텀 권한고지를 위한 데이터 구성
		 *
		 * @param 타겟 언어
		 *
		 * @return 각 언어별 리소스에 맞는 PermissionViewData
		 */	
		public static PermissionViewData getPermissionViewData(HIVELanguage language) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getPermissionViewData", null);

			Debug.Log("Enum To String = " + language.ToString());

			string languageStr = "";
			if (language.Equals(HIVELanguage.HIVELanguageAR)) {
				languageStr = "ar";
            }
			else if (language.Equals(HIVELanguage.HIVELanguageDE)) {
				languageStr = "de";
            }
            else if (language.Equals(HIVELanguage.HIVELanguageEN)){
				languageStr = "en";
            }
            else if (language.Equals(HIVELanguage.HIVELanguageES)){
				languageStr = "es";
            }
            else if (language.Equals(HIVELanguage.HIVELanguageFR)){
				languageStr = "fr";
            }
			else if (language.Equals(HIVELanguage.HIVELanguageID)){
				languageStr = "id";
            }
            else if (language.Equals(HIVELanguage.HIVELanguageIT)){
				languageStr = "it";
            }
            else if (language.Equals(HIVELanguage.HIVELanguageJA)){
				languageStr = "ja";
            }
            else if (language.Equals(HIVELanguage.HIVELanguageKO)){
				languageStr = "ko";
            }
            else if (language.Equals(HIVELanguage.HIVELanguagePT)){
				languageStr = "pt";
            }
            else if (language.Equals(HIVELanguage.HIVELanguageRU)){
				languageStr = "ru";
            }
            else if (language.Equals(HIVELanguage.HIVELanguageTH)){
				languageStr = "th";
            }
            else if (language.Equals(HIVELanguage.HIVELanguageTR)){
				languageStr = "tr";
            }
            else if (language.Equals(HIVELanguage.HIVELanguageVI)){
				languageStr = "vi";
            }
			else if (language.Equals(HIVELanguage.HIVELanguageZHS)){
				languageStr = "zh-hans";
            }
			else if (language.Equals(HIVELanguage.HIVELanguageZHT)){
				languageStr = "zh-hant";
            }
			
			jsonParam.AddField("language", languageStr); 

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);
			JSONObject permissionData = resJson.GetField("data");

			return new PermissionViewData(permissionData);
		}

		/**
		 * Hive 테마 설정
		 *
		 * @param Hive Theme
		 */
		public static void setHiveTheme(HiveThemeType theme) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setHiveTheme", null);
			jsonParam.AddField("hiveThemeType", theme.ToString());

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 * \~korean Hive 테마 반환
		 *
		 * @return Hive Theme Type (hiveLight, hiveDark)
		 * \~english Return Hive Theme
		 *
		 * @return Hive Theme Type (hiveLight, hiveDark)
		 * \~
		 * @ingroup Configuration
		 */

		public static HiveThemeType getHiveTheme() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getHiveTheme", null);

			JSONObject resJson = HIVEUnityPlugin.callNative (jsonParam);

			String resultValue = "";
			resJson.GetField (ref resultValue, "hiveThemeType");

			HiveThemeType hiveThemeType = (HiveThemeType) Enum.Parse(typeof(HiveThemeType), resultValue, true);
			return hiveThemeType;
		}


		/**
		 * \~korean Hive Orientation 설정
		 * 
		 * \~english Set Hive Orientation
		 *
		 * @ingroup Configuration
		 */
		public static void setHiveOrientation(String orientation) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setHiveOrientation", null);
			jsonParam.AddField("hiveOrientation", orientation);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		 public static void setConfigurations(HiveConfigType configType, string value) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setConfigurations", null);
			jsonParam.AddField("configType", configType.ToString());
			jsonParam.AddField("value", value.ToString());

			HIVEUnityPlugin.callNative (jsonParam);

		 } 

		/**
     	 * Game MetaData 요청
     	 *
     	 * @param key 요청 data 키
     	 * @param forceReload network 통신 여부
     	 * @param API 결과 통지
     	 *
     	 */
		 public static void getMetaData(string key, bool forceReload, onConfigurationGetMetaData listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "getMetaData", listener);
			jsonParam.AddField ("key", key);
			jsonParam.AddField ("forceReload", forceReload);

			HIVEUnityPlugin.callNative (jsonParam);	
		}

		/**
		 * Hive 커뮤니티 URL 설정
		 *
		 * @param url 커뮤니티 URL
		 */
		public static void setHiveCommunityUrl(String url) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setHiveCommunityUrl", null);
			jsonParam.AddField("hiveCommunityUrl", url);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 * Hercules 사용 여부 설정
		 *
		 * @param enable Hercules 사용 여부
		 */
		public static void setUseHercules(bool enable) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Configuration", "setUseHercules", null);
			jsonParam.AddField("enable", enable);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			resJsonObject.GetField (ref handlerId, "handler");
			object handler = (object)HIVEUnityPlugin.popHandler (handlerId);

			if (handler == null) return;

			if ("getMetaData".Equals (methodName)) {

				String value = "";
				resJsonObject.GetField (ref value, "value");

				onConfigurationGetMetaData listener = (onConfigurationGetMetaData)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), value);
			}

		}
	}

	/**
	 * \~korean HIVE 플랫폼 서버 존 정의
	 * \~english Hive Platform server zone type
	 * 
	 * @ingroup hive
	 * @author ryuvsken
	 */
	public enum ZoneType {

		SANDBOX,		///< \~korean HIVE 플랫폼 외부 개발 서버 \~english Hive platform server for development
		TEST,			///< \~korean HIVE 플랫폼 내부 개발 서버 \~english Hive platform server for internal test only
		REAL,			///< \~korean HIVE 플랫폼 실서비스 서버 \~english Hive platform server for production
		DEV			///<
	}


	/**
	 * \~korea Hive 테마 타입
	 * \~english Hive theme type
	 *
	 * @ingroup hive
	 */
	 public enum HiveThemeType {
		 hiveLight,				///< \~korean Hive Light Theme \~english Hive Light Theme
		 hiveDark				///> \~korean Hive Dark Theme \~english Hive Dark Theme
	 }

	public enum HiveConfigType {
		googleServerClientId,
		googlePlayGamesServerClientId,
		wechatSecret,
		wechatPaymentKey,
		adjustKey,
		adjustSecretId,
		adjustInfo1,
		adjustInfo2,
		adjustInfo3,
		adjustInfo4,
		singularKey,
		appsflyerKey
    
	}

	/**
	 * \~korean Hive 플랫폼 서버 존 정의
	 * \~english Hive Platform permission type
	 * 
	 * @ingroup hive
	 * @author ryuvsken
	 */
	public enum HIVEPermissionType {
		SDWRITE			///< \~korean 외장 메모리 권한 요청 여부. 이 필드가 true 이면 SDK 초기화시 SDK 내부에서 외장 메모리 접근에 대한 권한 요청 창을 띄운다 - Android only \~english Whether external memory permission is requested. If this field is true, SDK will show permission popup for accessing external memory when initializing SDK - Android only
	}

	/**
	* \~korean HIVE 플랫폼 지원 언어
	* \~english HIVE Platform languages supported
	*
	* @author seokjinyong
	* @ingroup Configuration
	*/
    public enum HIVELanguage {
		HIVELanguageAR,
        HIVELanguageDE,
        HIVELanguageEN,
        HIVELanguageES,
        HIVELanguageFR,
        HIVELanguageID,
        HIVELanguageIT,
        HIVELanguageJA,
        HIVELanguageKO,
        HIVELanguagePT,
        HIVELanguageRU,
        HIVELanguageTH,
        HIVELanguageTR,
        HIVELanguageVI,
        HIVELanguageZHS,
        HIVELanguageZHT,
	}

}


/** @} */



