/**
 * @file    Auth.cs
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
 *  @defgroup	Auth
 * @{
 *  \~korean
 * @brief HIVE SDK 초기화 및 HIVE 인증 기능을 제공한다<br/>
 * HIVE SDK 는 xml 설정 파일을 기반으로 초기화를 수행한다.<br/>
 *
 *  \~english
 * @brief Provides HIVE SDK initialization and HIVE authentication functions<br/>
 * The HIVE SDK performs initialization based on the xml configuration file.<br/><br/>
 *  \~
 *
 *  
 */
 

namespace hive
{
	/**
	 *  \~korean
	 * HIVE SDK 초기화 및 HIVE 인증 기능을 제공한다<br/>
	 * HIVE SDK 는 xml 설정 파일을 기반으로 초기화를 수행한다.<br/>
	 * <br/>
	 * 이 클래스에서 제공하는 상세 기능은 다음과 같다. <br/><br/>
	 *  - HIVE SDK 초기화<br/>
	 *  - 약관 노출<br/>
	 *  - 고객 정보 수집 및 이용 약관 동의<br/>
	 *  - 단말 고유 ID (DID : Device ID) 관리<br/>
	 *  - User 다운로드 / Session 관리<br/>
	 *  - 버전과 서버 점검 및 업데이트 관리<br/>
	 *  - 사용자 제재<br/>
	 *  - Guest / HIVE 로그인 / 로그 아웃 수행<br/>
	 *  - 성인 인증 수행<br/><br/>
	 *
	 *  \~english
	 * Provides HIVE SDK initialization and HIVE authentication functions<br/>
	 * The HIVE SDK performs initialization based on the xml configuration file.<br/><br/>
	 * The detailed functions provided by this class are as follows. <br/><br/>
	 * - HIVE SDK Initialization<br/>
	 * - Exposure Terms and Conditions<br/>
	 * - Collect user information and accept terms and conditions<br/>
	 * - Device unique ID (DID : Device ID) management<br/>
	 * - User download / Session management<br/>
	 * - Version and server maintenance and update management<br/>
	 * - User restraint<br/>
	 * - Guest / HIVE Log-in / Logout <br/>
	 * - Perform adult verification<br/><br/>
	 *  \~
	 * @ingroup	Auth
	 * @since		4.0.0
	 * @author ryuvsken
	 */
	public class Auth {


		/**
		 *  \~korean 
		 * @brief HIVE SDK 초기화 결과 통지
		 * 
		 * @param result			API 호출 결과
		 * @param authInitResult	HIVE SDK 의 초기화 수행 결과
		 *  \~english 
		 * @brief Notify HIVE SDK initialization result listener
		 * 
		 * @param result			API call result
		 * @param authInitResult	HIVE SDK initialization result
		 *  \~
		 * @ingroup	Auth
		 */
		public delegate void onAuthInitialize(ResultAPI result, AuthInitResult authInitResult);


		/**
		 *  \~korean 
		 * @brief HIVE 로그인 결과 통지
		 * 
		 * @param result			API 호출 결과
		 * @param loginType			HIVE 로그인 결과에 따른 HIVE 로그인 형태<br/>만약 loginType 이 LoginType.SELECT 일 경우 HIVE 계정 충돌에 따른 유저 선택을 요청해야 한. 
		 * @param currentAccount	HIVE 로그인이 완료된 유저의 HIVE 계정 정보
		 * @param usedAccount		HIVE 인증 서버에 등록된 유저의 HIVE 계정 정보
		 *  \~english 
		 * @brief HIVE login result listener
     	 *
     	 * @param result			API call result
     	 * @param loginType			HIVE login type according to HIVE login result <br/> If loginType is LoginType.SELECT, it should ask user selection according to HIVE account conflict.
     	 * @param currentAccount	HIVE account information of user who completed HIVE login
     	 * @param usedAccount		Hive account information of user who registered in HIVE authentication server
		 * \~
		 * @ingroup	Auth
		 */
		public delegate void onAuthLogin(ResultAPI result, LoginType loginType, Account currentAccount, Account usedAccount);


		/**
		 *  \~korean 
		 * @brief HIVE 로그 아웃 결과 통지
		 * 
		 * @param result			API 호출 결과
		 *  \~english 
		 * @brief HIVE logout result listener
		 * 
		 * @param result			API call result
		 * \~
		 * @ingroup	Auth
		 */
		public delegate void onAuthLogout(ResultAPI result);


		/**
		 *  \~korean 
		 * @brief 약관 정보 표시 결과 통지
		 * 
		 * @param result			API 호출 결과
		 *  \~english 
		 * @brief The result of displaying the terms and conditions
		 * 
		 * @param result			API call result
		 * \~
		 * @ingroup	Auth
		 */
		public delegate void onAuthShowTerms(ResultAPI result);


		/**
		 *  \~korean 
		 * @brief 점검 팝업 결과 통지
		 * 
		 * @param result				API 호출 결과
		 * @param authMaintenanceInfo	점검 팝업을 게엠에서 띄우기 위한 데이터
		 *  \~english 
		 * @brief Maintenance popup result
		 * 
		 * @param result				API call result
		 * @param authMaintenanceInfo	Data for pop-ups in game
		 * \~
		 * @ingroup	Auth
		 */
		public delegate void onAuthMaintenance(ResultAPI result, AuthMaintenanceInfo authMaintenanceInfo);


		/**
		 *  \~korean 
		 * @brief 성인 인증 팝업 결과 통지
		 * 
		 * @param result				API 호출 결과
		 *  \~english 
		 * @brief Adult verification pop-up result
		 * 
		 * @param result				API call result
		 * \~
		 * @ingroup	Auth
		 */
		public delegate void onAuthAdultConfirm(ResultAPI result);

		public delegate void onAuthRequestPermissionViewData(ResultAPI result,PermissionViewData data);


		/**
		 *  \~korean 
		 * @brief HIVE SDK 초기화 수행<br/>
		 * 만약 앱이 처음 실행된 경우라면 약관을 노출하고 동의를 받는 과정을 거치게 된다
		 * 
		 * @param listener	API 호출 결과 통지
		 *  \~english 
		 * @brief Initialize HIVE SDK<br/>
	     * If the app is launched for the first time, you will be exposed to the terms and acceptance process.
		 * 
		 * @param listener	API call result
		 * \~
		 * @ingroup	Auth
		 */
		public static void initialize(onAuthInitialize listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "initialize", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 *  \~korean 
		 * @brief SDK 초기화 후 기존에 로그인한 정보에 따라서 수행할 수 있는 로그인 정보를 반환한다.
		 * 
		 * @return 수행 가능한 로그인 정보
		 *  \~english 
		 * @brief After initializing the SDK, it returns the login information that can be executed according to the existing login information.
	     *
	     * @return Possible login type
		 * \~
		 * @ingroup	Auth
		 */
		public static LoginType getLoginType() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "getLoginType", null);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

			String loginType = "";
			resJsonObject.GetField (ref loginType, "getLoginType");
			loginType = loginType.ToUpper ();

			if ("ACCOUNT".Equals (loginType))
				return LoginType.ACCOUNT;
			else if ("SELECT".Equals (loginType))
				return LoginType.SELECT;
			else if ("AUTO".Equals (loginType))
				return LoginType.AUTO;
			else
				return LoginType.GUEST;
		}


		/**
		 *  \~korean 
		 * @brief 주어진 로그인 타입에 따라서 HIVE 로그인을 요청한다.</br>
		 * 
		 * @param loginType 		GUEST : HIVE 게스트 로그인<br/>
		 * 							ACCOUNT : HIVE 로그인<br/>
	 	 * 							AUTO : 자동 로그인 선택 게스트 로그인이나 HIVE 로그인되어 있는 상태 (단말에 세션키가 남아 있는 상태)
		 * @param listener	API 호출 결과 통지
		 *  \~english 
		 * @brief Request HIVE login according to given login type.</br>
	     *
	     * @param loginType 	GUEST : HIVE Guest login<br/>
	     * 						ACCOUNT : HIVE Login<br/>
	     * 						AUTO : Auto Login  (session key remains in the device)
	     * @param listener	API call result
		 * \~
		 * @ingroup	Auth
		 */
		public static void login(LoginType loginType, onAuthLogin listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "login", listener);
			jsonParam.AddField ("loginType", loginType.ToString());

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 *  \~korean 
		 * @brief 게스트 로그인 상태에서 HIVE 로그인을 수행하면 계정 충돌이 발생 할 수 있으며, 유저에게 HIVE 계정 전환을 요청 해야 한다.
		 * 이 메서드 호출하면 유저에게 충돌난 계정 중 하나를 선택하기 위한 대화 상자 노출 하게 된다.
		 * 
		 * @param currentVidData	로그인 되어 있는 유저의 정보
		 * @param usedVidData		HIVE 인증 서버에 등록된 유저 정보
		 * @param listener			API 호출 결과 통지
		 *  \~english 
		 * @brief Performing a HIVE login while logged in as a guest may result in account conflicts and require the user to switch HIVE accounts.<br/>
	     * Calling this method shows a dialog box to the user to select one of the conflicting accounts.
	     *
	     * @param currentVidData	Information of logged in user
	     * @param usedVidData		User information registered in HIVE authentication server
	     * @param listener			API call result
		 * \~
		 * @ingroup	Auth
		 */
		public static void showLoginSelection(JSONObject currentVidData, JSONObject usedVidData, onAuthLogin listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "showLoginSelection", listener);
			jsonParam.AddField("currentData", currentVidData);
			jsonParam.AddField("selectData", usedVidData);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 *  \~korean 
		 * @brief 유저가 충돌난 HIVE 계정 중 하나를 선택하면 HIVE 인증 서버에 결과를 전송해야 한다.<br/>
		 * 이 메서드를 호출하면 주어진 유저의 고유 ID 로 HIVE 인증 서버에 결과를 전송 한다.
		 * 
		 * @param selectedVid		유저가 선택한 유저의 고유 ID
		 * @param listener			API 호출 결과 통지
		 *  \~english 
		 * @brief If the user selects one of the conflicting HIVE accounts, the result must be sent to the HIVE authentication server.<br/>
	     * Calling this method sends the result to the HIVE authentication server with the given user's unique ID.
	     *
	     * @param selectedVid		The unique ID of the user selected by the user
	     * @param listener			API call result listener
		 * \~
		 * @ingroup	Auth
		 */
		public static void bindLogin(String selectedVid, onAuthLogin listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "bindLogin", listener);

			jsonParam.AddField("selectedVid", selectedVid);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 *  \~korean 
		 * @brief HIVE 로그인을 완료 하였으면 유저의 고유 ID 인 VID 와 세션키가 발급된 상태이다.<br/>
		 * 로그 아웃을 요청하면 VID 와 세션키를 초기화 하는 기능을 수행한다<br/>
		 * (주의 : 게스트 로그인시에는 절대 로그 아웃 금지)
		 * 
		 * @param listener 		API 호출 결과 통지
		 *  \~english 
		 * @brief Once you have completed your HIVE login, you are issued a user's unique ID, VID, and session key. <br/>
	     * When requesting logout, it initializes VID and session key<br/>
	     * (Note: Never logout at guest login)
	     *
	     * @param listener 		API call result listener
		 * \~
		 * @ingroup	Auth
		 */
		public static void logout(onAuthLogout listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "logout", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 *  \~korean 
		 * @brief HIVE 유저의 인증 정보 반환한다
		 * 
		 * @return HIVE 유저의 인증 정보
		 * @see Account
		 *  \~english 
		 * @brief Returns the authentication information of HIVE user
	     *
	     * @return The authentication information of HIVE user
		 * @see Account
		 * \~
		 * @ingroup	Auth
		 */
		public static Account getAccount() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "getAccount", null);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

			return new Account(resJsonObject.GetField("getAccount"));
		}

		/**
		 *  \~korean 
		 * @brief HIVE 약관 정보를 표시한다.<br/>
		 * HIVE SDK 초기화 시 사용자에게 약관 동의 절차를 거치게 된다.<br/>
	     * 이후 게임에서는 설정 창 등에서 개인 정보 처리 방침 및 이용 약관 정보를 확인할 수 있는 웹뷰를 노출하도록 구성해야 한다.
		 * 
		 * @param listener	API 호출 결과 통지
		 *  \~english 
		 * @brief Display HIVE Terms and Conditions.<br/>
	     * When HIVE SDK is initialized, the user will go through the agreement process.<br/>
	     * After this, the game should expose Personal information processing policies and terms and services in the game settings.
	     *
	     * @param listener	API call result listener
		 * \~
		 * @ingroup	Auth
		 */
		public static void showTerms(onAuthShowTerms listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "showTerms", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 *  \~korean 
		 * @brief 점검 상태 표시 및 데이터 반환한다.<br/>
		 * HIVE SDK 의 초기화가 완료되면 서버 점검 및 업데이트 상태를 확인해야 한다.<br/>
		 * 서버 점검 및 업데이트는 게임 클라이언트의 업데이트 후에 하위 버전을 차단하거나, 게임 서버의 점검 시간 동안 게임 접속을 차단할 수 있다.<br/>
		 * HIVE는 백오피스에 설정된 정보에 따라 서버 점검, 게임 강제 업데이트, 공지 순으로 팝업을 노출하는 기능을 제공한다.
		 * 
		 * @param isShow 		HIVE SDK 에서 점검 팝업을 노출할지 여부<br/>만약 이 값이 true 이면 HIVE SDK 에서 제공 하는 점검 팝업 UI 사용, false 이면 커스터마이징 UI 를 위한 데이터를 수신한다.
		 * @param listener		API 호출 결과 통지
		 *  \~english 
		 * @brief Display the maintenance status and return data.<br/>
 	     * When the HIVE SDK initialization is completed, you need to check the server maintenance and update status.<br/>
	     * Server maintenance and update can block lower version after update of game client, or block game connection during game server maintenance time.<br/>
	     * HIVE provides a function to show popups in the order of server maintenance, game forced update and notice in accordance with the information set in the back office.
	     *
	     * @param isShow 	Whether to show maintenance popups in HIVE SDK <br/> If this value is true, maintenance popup UI provided by HIVE SDK is used, if false, data for customizing UI is received.
	     * @param listener 	API call result
		 * \~
		 * @ingroup	Auth
		 */
		public static void checkMaintenance(Boolean isShow, onAuthMaintenance listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "checkMaintenance", listener);
			jsonParam.AddField ("isShow", isShow);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 *  \~korean 
		 * @brief 성인 인증을 요청한다.<br/>
		 * 고스톱 / 포커류와 같이 성인 인증이 필요한 일부 게임의 경우 성인 인증 기능을 제공한다.
		 * 
		 * @param listener 성인 인증 결과 통지
		 *  \~english 
		 * @brief Request adult verification.<br/>
	     * For some games that require adult verification, such as GoStop / poker, adult verification is provided.
	     *
	     * @param listener Adult verification result
		 * \~
		 * @ingroup	Auth
		 */
		public static void showAdultConfirm(onAuthAdultConfirm listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "showAdultConfirm", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 *  \~korean 
		 * @brief 인증 정보를 포함하여 SDK 에서 사용하는 모든 데이터 초기화 한다.<br/>
		 * HIVE SDK 연동 및 테스트시에 사용된다.
		 * 
		 *  \~english 
		 * @brief Initialize all data used by the SDK, including authentication information.<br/>
	     * Used for HIVE SDK interworking and testing.
		 * \~
		 * @ingroup	Auth
		 */
		public static void reset() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "reset", null);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		* @brief 권한고지 팝업을 구성하기 위한 데이터를 얻습니다.<br/>
		* ResultAPI의 result가 success이며,
		* result의 code가 AuthV4SkipPermissionView 혹은 AuthSkipPermissionView가 아닐 경우 권한고지 데이터를 이용하여 권한고지를 출력 할 수있습니다.
		* PermissionViewData를 참조하여 데이터를 이용하여 UI를 구성하세요.
		* 이 함수를 호출하여 권한고지팝업을 구성할 시 AuthV4.setup, Auth.initialize를 호출하였을때 HIVE SDK의 권한고지 팝업은 나오지 않습니다.
		* @warning code가 AuthV4SkipPermissionView 혹은 AuthSkipPermissionView가 왔을 경우 PermissionViewData에는 빈값이 오게됩니다. 값을 참조할 경우 예기치못한 오류가 발생할 수 있으므로 주의 해주세요. 또한 위 코드는 ResultAPI Success일 경우만 오게됩니다.
		*
		* @see PermissionViewData
		* @see ResultAPI
		*
		* @param handler 성인 인증 결과 통지
		* @ingroup Auth
		*/
		public static void requestPermissionViewData(onAuthRequestPermissionViewData listener) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "requestPermissionViewData", listener);
			HIVEUnityPlugin.callNative (jsonParam);
		}
		
		public static Boolean setEmergencyMode() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Auth", "setEmergencyMode", null);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

			Boolean isEmergencyMode = false;
			resJsonObject.GetField (ref isEmergencyMode, "isEmergencyMode");
			return isEmergencyMode;
		}


		/**
		 * \internal
		 * Native 영역에서 호출된 요청을 처리하기 위한 플러그인 내부 코드
		 * The internal code of the plugin to handle requests invoked from the native 
		 */
		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			resJsonObject.GetField (ref handlerId, "handler");

			object handler = (object)HIVEUnityPlugin.popHandler (handlerId);

			if (handler == null) return;

			if ("initialize".Equals (methodName)) {
					
				onAuthInitialize listener = (onAuthInitialize)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), new AuthInitResult (resJsonObject.GetField ("authInitResult")));
			}
			else if ("login".Equals (methodName)) {

				LoginType loginType = LoginType.GUEST;

				String loginTypeString = "GUEST";
				resJsonObject.GetField (ref loginTypeString, "loginType");
				if ("ACCOUNT".Equals (loginTypeString))
					loginType = LoginType.ACCOUNT;
				else if ("SELECT".Equals (loginTypeString))
					loginType = LoginType.SELECT;
				else if ("AUTO".Equals (loginTypeString))
					loginType = LoginType.AUTO;
				else
					loginType = LoginType.GUEST;

				onAuthLogin listener = (onAuthLogin)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), loginType, new Account (resJsonObject.GetField ("currentAccount")), new Account (resJsonObject.GetField ("usedAccount")));
			}
			else if ("showLoginSelection".Equals (methodName)) {

				LoginType loginType = LoginType.GUEST;

				String loginTypeString = "GUEST";
				resJsonObject.GetField (ref loginTypeString, "loginType");
				if ("ACCOUNT".Equals (loginTypeString))
					loginType = LoginType.ACCOUNT;
				else if ("SELECT".Equals (loginTypeString))
					loginType = LoginType.SELECT;
				else if ("AUTO".Equals (loginTypeString))
					loginType = LoginType.AUTO;
				else
					loginType = LoginType.GUEST;

				onAuthLogin listener = (onAuthLogin)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), loginType, new Account (resJsonObject.GetField ("currentAccount")), new Account (resJsonObject.GetField ("usedAccount")));
			}
			else if ("bindLogin".Equals (methodName)) {

				LoginType loginType = LoginType.ACCOUNT;

				onAuthLogin listener = (onAuthLogin)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), loginType, new Account (resJsonObject.GetField ("currentAccount")), new Account (resJsonObject.GetField ("usedAccount")));

			}
			else if ("logout".Equals (methodName)) {

				onAuthLogout listener = (onAuthLogout)handler;
				listener (new ResultAPI (resJsonObject.GetField("resultAPI")));
			}
			else if ("showTerms".Equals (methodName)) {

				onAuthShowTerms listener = (onAuthShowTerms)handler;
				listener (new ResultAPI (resJsonObject.GetField("resultAPI")));
			}
			else if ("checkMaintenance".Equals (methodName)) {

				AuthMaintenanceInfo authMaintenanceInfo = new AuthMaintenanceInfo (resJsonObject.GetField("authMaintenanceInfo"));

				onAuthMaintenance listener = (onAuthMaintenance)handler;
				listener (new ResultAPI (resJsonObject.GetField("resultAPI")), authMaintenanceInfo);
			}
			else if ("showAdultConfirm".Equals (methodName)) {

				onAuthAdultConfirm listener = (onAuthAdultConfirm)handler;
				listener (new ResultAPI (resJsonObject.GetField("resultAPI")));
			}
			else if("requestPermissionViewData".Equals(methodName)) {

				onAuthRequestPermissionViewData listener = (onAuthRequestPermissionViewData)handler;
				JSONObject permissionData = resJsonObject.GetField("data");
				listener(new ResultAPI (resJsonObject.GetField ("resultAPI")),permissionData == null ? null : new PermissionViewData(permissionData));
			}
		}

	}		// end of public class Auth




	/**
	 *  \~korean HIVE 로그인 형태 정의
	 * 
	 *  \~english HIVE login type definition
	 * \~
	 * @ingroup	Auth
	 */
	public enum LoginType {

		GUEST			///< \~korean HIVE 게스트 로그인 \~english HIVE Guest login
		, ACCOUNT		///< \~korean HIVE 로그인 \~english HIVE Login
		, SELECT		///< \~korean 계정 충돌로 인한 유저 선택이 필요한 경우 \~english When user selection is required due to account conflict
		, AUTO			///< \~korean 게스트 로그인이나 HIVE 로그인되어 있는 상태<br/>(단말에 세션키가 남아 있는 상태) \~english Guest logged in or HIVE logged in<br/>(The session key remains in device)
	}


	/**
	 *  \~korean 점검 화면에서 버튼을 눌렀을때 동작될 행동
	 * 
	 *  \~english Definition of actions to be taken when the button is pressed on the maintenance popup.
	 * \~
	 * @ingroup	Auth
	 */
	public enum AuthMaintenanceActionType {

		OPEN_URL 		///< \~korean 외부 부라우저로 전달된 url 을 실행 \~english Run url passed to external browser.
		, EXIT			///< \~korean 앱 종료 \~english Finish app.
		, DONE			///< \~korean 아무 처리도 하지 않음. \~english Do nothing.
	}

	/**
	 *  \~korean HIVE 인증 사용자 정보
	 * 
	 *  \~english HIVE authentication user information
	 * \~
	 * @ingroup	Auth
	 */
	public class Account {

		public String vid;			///< \~korean HIVE 로그인을 수행하면 게임별로 발급되는 사용자의 고유 ID \~english If you perform HIVE login, the unique ID of the user issued for each game.
		public String uid;			///< \~korean HIVE Social 에서 사용하는 사용자의 고유 ID \~english User unique ID for HIVE Social.
		public String did;			///< \~korean 단말별로 발급되는 고유 ID \~english Unique ID issued to each device.
		public String accessToken;	///< \~korean HIVE 로그인의 유효성을 확인하기 위해서 HIVE 인증 서버에서 발급하는 고유 세션키 \~english Shared session key issued by the HIVE authentication server to verify the validity of the HIVE login.

		public Account() {
		}

		public Account(String vid, String uid, String did, String accessToken) {
			
			this.vid = vid;
			this.uid = uid;
			this.did = did;
			this.accessToken = accessToken;
		}

		public Account(JSONObject jsonParam) {

			if (jsonParam == null || jsonParam.count <= 0)
				return;
			
			jsonParam.GetField (ref this.vid, "vid");
			jsonParam.GetField (ref this.uid, "uid");
			jsonParam.GetField (ref this.did, "did");
			jsonParam.GetField (ref this.accessToken, "accessToken");
		}

		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("Account { vid = ");
			sb.Append(vid);
			sb.Append(", uid = ");
			sb.Append(this.uid);
			sb.Append(", did = ");
			sb.Append(this.did);
			sb.Append(", accessToken = ");
			sb.Append(this.accessToken);
			sb.Append (" }\n");

			return sb.ToString();
		}
	}


	/**
	 * \~korean HIVE SDK 초기화 후 결과 통지시 전달되는 정보
	 * 
	 * \~english Information to be delivered when the result is notified after HIVE SDK initialization.
	 * \~
	 * @ingroup	Auth
	 */
	public class AuthInitResult {

		public Boolean isAuthorized;	///< \~korean HIVE(계정) 로그인 이력 여부<br><br>(true : HIVE 로그인 이력이 있음, false : HIVE 로그인 이력이 없음) \~english HIVE (account) login history <br> <br>(True: Has HIVE login history, false: No HIVE login history)

		/**
		 * \~korean
		 * HIVE 로그인 형태<br/>
		 * (GUEST 이면 GUEST 로그인 가능<br/>
		 * ACCOUNT 이면 HIVE 로그인 (id/password 기반) 가능<br/>
		 * SELECT 이면 이전에 계정 로그인을 통하여 HIVE 로그인 서버에 매핑된 계정 정보가 존재해서 계정 충돌 상태. 유저에게 사용자가 계정을 선택하는 화면을 띄워야 한다.<br/>
		 * AUTO 이면 이미 로그인이 완료되어 세션 키가 존재하는 상황
		 * \~english
		 * HIVE Login type<br/>
	     * If GUEST, login to GUEST<br/>
	     * If ACCOUNT, DO HIVE login (based on id/password) <br/>
	     * If SELECT is present, account information mapped to HIVE login server through account login previously exists, and the account is in conflict state. The game must display a screen for the user to select an account.<br/>
	     * If AUTO, login is completed and the session key exists.
		 */
		public LoginType loginType = LoginType.GUEST;

		public String did;				///< \~korean HIVE 로그인의 유효성을 확인하기 위해서 HIVE 인증 서버에서 발급하는 고유 ID \~english Unique ID issued by HIVE authentication server in order to check the validity of HIVE login.

		public Boolean isPGSLogin;		///< \~korean Google Play Game Service 로그인 가능 여부 (Android only.) \~english Whether Google Play Game Service  (Android only)
		public String playerName;		///< \~korean Google Play Game Service 사용자 프로필 명 (Android only.) \~english Google Play Game Service user profile name (Android only)
		public String playerId;			///< \~korean Google Play Game Service 사용자 계정 (Android only.) \~english Google Play Game Service user ID (Android only)


		public AuthInitResult() {
		}


		public AuthInitResult(Boolean isAuthorized, LoginType loginType, String did, Boolean isPGSLogin, String playerName, String playerId) {

			this.isAuthorized = isAuthorized;
			this.loginType = loginType;
			this.did = did;
			this.isPGSLogin = isPGSLogin;
			this.playerName = playerName;
			this.playerId = playerId;
		}


		public AuthInitResult(JSONObject resJsonParam) {

			if (resJsonParam == null || resJsonParam.count <= 0)
				return;
			
			resJsonParam.GetField (ref this.isAuthorized, "isAuthorized");

			String loginType = "";
			resJsonParam.GetField (ref loginType, "loginType");
			if ("ACCOUNT".Equals (loginType.ToUpper ()))
				this.loginType = LoginType.ACCOUNT;
			else if ("AUTO".Equals (loginType.ToUpper ()))
				this.loginType = LoginType.AUTO;
			else if ("SELECT".Equals (loginType.ToUpper ()))
				this.loginType = LoginType.SELECT;
			else
				this.loginType = LoginType.GUEST;

			resJsonParam.GetField (ref this.did, "did");
			resJsonParam.GetField (ref this.isPGSLogin, "isPGSLogin");
			resJsonParam.GetField (ref this.playerName, "playerName");
			resJsonParam.GetField (ref this.playerId, "playerId");
		}


		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("AuthInitResult {");
			sb.Append ("isAuthorized = ");
			sb.Append(this.isAuthorized);
			sb.Append(", loginType = ");
			sb.Append(this.loginType.ToString());
			sb.Append(", did = ");
			sb.Append(this.did);
			sb.Append(", isPGSLogin = ");
			sb.Append(this.isPGSLogin);
			sb.Append(", playerName = ");
			sb.Append(this.playerName);
			sb.Append(", playerId = ");
			sb.Append(this.playerId);
			sb.Append (" }\n");

			return sb.ToString();
		}
	}


	/**
	 * \~korean 서버 점검 및 업데이트 상태 표시 정보
	 * 
	 * \~english Server maintenance and update status information
	 * \~
	 * @ingroup	Auth
	 */
	public class AuthMaintenanceInfo {

		public String title; 					///< \~korean 점검 제목 \~english Title
		public String message;					///< \~korean 점검 내용 \~english Message
		public String button;					///< \~korean 원버튼 형태의 팝업이며, 그때 팝업 버튼의 Text 문구 ex) 확인 \~english Text for button ex) OK
		public AuthMaintenanceActionType action;///< \~korean 버튼을 눌렀을때 동작될 행동 \~english Actions to be taken when the button is pressed
		public String url;						///< \~korean action 이 OPEN_URL 일 경우에 브라우징 될 URL \~english URL to be browsed if actionType is OPEN_URL
		public int remainingTime;				///< \~korean EXIT 일 경우 점검 완료까지 남은 초단위 시간. 시간은 실시간 갱신되며 0초가 되면 앱 종료 \~english The time in seconds remaining until the maintenance completes if actionType is EXIT. The time is updated in real time.

		public AuthMaintenanceInfo() {
			action = AuthMaintenanceActionType.DONE;
		}

		public AuthMaintenanceInfo(JSONObject jsonParam) {

			if (jsonParam == null || jsonParam.count <= 0)
				return;

			jsonParam.GetField (ref this.title, "title");
			jsonParam.GetField (ref this.message, "message");
			jsonParam.GetField (ref this.button, "button");

			String value = "";
			jsonParam.GetField (ref value, "action");
			if ("OPEN_URL".Equals (value))
				this.action = AuthMaintenanceActionType.OPEN_URL;
			if ("EXIT".Equals (value))
				this.action = AuthMaintenanceActionType.EXIT;
			else
				this.action = AuthMaintenanceActionType.DONE;

			jsonParam.GetField (ref this.url, "url");
			jsonParam.GetField (ref this.remainingTime, "remainingTime");
		}

		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("AuthMaintenanceInfo {");
			sb.Append ("title = ");
			sb.Append(this.title);
			sb.Append(", message = ");
			sb.Append(this.message);
			sb.Append(", button = ");
			sb.Append(this.button);
			sb.Append(", action = ");
			sb.Append(action.ToString());
			sb.Append(", url = ");
			sb.Append(this.url);
			sb.Append(", remainingTime = ");
			sb.Append(this.remainingTime);
			sb.Append (" }\n");

			return sb.ToString();
		}
	}
}


/** @} */



