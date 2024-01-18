/**
 * @file    AuthV4.cs
 * 
 *  @date		2016-2022
 *  @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 *  @author		ryuvsken
 *  @since		4.3.0
 */
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @defgroup	AuthV4
 *  @{
 *  \~korean
 * @brief HIVE SDK 초기화 및 HIVE 인증 기능을 제공한다<br/>
 * HIVE SDK 는 xml 설정 파일을 기반으로 초기화를 수행한다.<br/>
 *
 *  \~english
 * @brief Provides HIVE SDK initialization and HIVE authentication functions<br/>
 * The HIVE SDK performs initialization based on the xml configuration file.<br/><br/>
 *  \~
 *
 */
namespace hive
{
	/**
	 *  \~korean
	 * HIVE SDK 초기화 및 HIVE 인증 기능을 제공한다
	 * HIVE SDK 는 xml 설정 파일을 기반으로 초기화를 수행한다.<br/>
	 * <br/>
	 * 이 클래스에서 제공하는 상세 기능은 다음과 같다. <br/><br/>
	 *  - HIVE SDK 초기화<br/>
	 *  - 고객 정보 수집 및 이용 약관 동의<br/>
	 *  - 단말 고유 ID (DID : Device ID) 관리<br/>
	 *  - User 다운로드 관리<br/>
	 *  - 버전과 서버 점검 및 업데이트 관리<br/>
	 *  - 사용자 제재<br/>
	 *  - Guest / Provider 사인-인/아웃 수행<br/>
	 *  - 프로필, 카페, 1:1문의 수행<br/>
	 *  - 성인 인증 수행<br/>
	 *
	 *  \~english
	 * Provides HIVE SDK initialization and HIVE authentication functions<br/>
	 * The HIVE SDK performs initialization based on the xml configuration file.<br/><br/>
	 * The detailed functions provided by this class are as follows. <br/><br/>
	 * - HIVE SDK Initialization<br/>
	 * - Collect user information and accept terms and conditions<br/>
	 * - Device unique ID (DID : Device ID) management<br/>
	 * - User download / Session management<br/>
	 * - Version and server maintenance and update management<br/>
	 * - User restriction<br/>
	 * - Guest / IdP Log-in / Logout <br/>
	 * - User Profile, 1:1 Inquiry <br/>
	 * - Perform adult verification<br/><br/>
	 *  \~
	 * Created by hife on 2017. 3. 22
	 *
	 * @author hife
	 * @since		4.3.0
	 * @ingroup AuthV4
	 *
	 */
	public class AuthV4 {

		/**
		*  \~korean
		* @brief Provider 형태 정의
		* 여기서 AUTO 는 자동로그인의 용도로 쓰이며<br/>
		* isAutoSignIn() 이 true 일 경우 SignIn 시 AUTO 로 입력해 주면 된다.<br/>
		*
		*  \~english
		* @brief Provider Types
		* AUTO is for Automatic Login <br/>
		* If the result of isAutoSignIn() call is true, You need to set parameter as AUTO when you call SignIn.<br/>
		*  \~
		* @ingroup AuthV4
		*
		*/
		public enum ProviderType {

			GUEST = 0
			, HIVE = 1
			, FACEBOOK = 2
			, GOOGLE = 3
			, QQ = 4
			, WEIBO = 5
			, VK = 6
			, WECHAT = 7
			, APPLE = 8
			, SIGNIN_APPLE = 9
			, LINE = 10
			, TWITTER = 11
			, WEVERSE = 12
			, NAVER = 13
			, GOOGLE_PLAY_GAMES = 14
			, HUAWEI = 15
			, CUSTOM = 98
			, AUTO = 99
		}

		/**
		*  \~korean
		* @brief 점검 화면에서 버튼을 눌렀을때 동작될 행동
		* OPEN_URL : 외부 브라우저로 전달된 URL 을 실행<br/>
		* EXIT : 앱 종료<br/>
		* DONE : 아무 처리 하지 않고 점검 팝업 종료<br/>
		*
		*  \~english
		* @brief Actions to be taken when a button is pressed on the maintenance popup.
		* OPEN_URL : Open URL passed to external browser<br/>
		* EXIT : Exit App<br/>
		* DONE : Close the popup without any action<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public enum AuthV4MaintenanceActionType {

			OPEN_URL = 1
			, EXIT
			, DONE
		}

		/**
		*  \~korean
		* @brief 유저의 프로필 정보
		* playerID : 유저의 고유한 ID<br/>
		* playerName : 외부에 보여질 유저의 닉네임, 처음 연결된 Provider 의 정보로 채워지며<br/>
		*   HIVE 멤버쉽으로 연동한 사용자는 변경이 가능하다.<br/>
		* playerImageUrl : 유저의 섬네일 이미지 URL, PlayerName 과 마찬가지로 처음 연결된 Provider 의 정보로 채워지며<br/>
		*   HIVE 멤버쉽으로 연동한 사용자는 변경이 가능하다.<br/>
		*
		*  \~english
		* @brief User Profile Information
		* playerID : User's unique ID<br/>
		* playerName : The nickname of the user to be shown outside. It is filled with information from the first connected provider, but can be changed when user register HIVE Membership. <br/>
		* playerImageUrl : Thumbnail image URL of the user. Like PlayerName, it is filled with information from the first connected provider, but can be changed when user register HIVE Membership.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		
		*/
		public class ProfileInfo {
			
			private const Int64 serialVersionUID = 4839752348329;

			public Int64 playerId;
			public String playerName;
			public String playerImageUrl;

			public ProfileInfo() {}

			public ProfileInfo(Int64 playerId, String playerName, String playerImageUrl) {

				this.playerId = playerId;
				this.playerName = playerName;
				this.playerImageUrl = playerImageUrl;
			}

			public ProfileInfo(JSONObject jsonParam) {

				if (jsonParam == null || jsonParam.count <= 0) return;

				jsonParam.GetField (ref this.playerId, "playerId");
				jsonParam.GetField (ref this.playerName, "playerName");
				jsonParam.GetField (ref this.playerImageUrl, "playerImageUrl");
			}

			public virtual String toString() {

				StringBuilder sb = new StringBuilder();

				sb.Append("ProfileInfo { playerId = ");
				sb.Append(this.playerId);
				sb.Append(", playerName = ");
				sb.Append(this.playerName);
				sb.Append(", playerImageUrl = ");
				sb.Append(this.playerImageUrl);
				sb.Append(" }\n");

				return sb.ToString();
			}
		}

		/**
		*  \~korean
		* @brief 사인-인 유저의 정보 
		* 프로필 정보 (ProfileInfo) 와 함께 유저의 토큰과 DID 값이 포함되어 있다.<br/>
		* <br/>
		* playerToken : 사인-인 검증에 필요한 playerId 와 연결된 토큰<br/>
		* did : 단말 고유 ID (DID : Device ID). 처음 AuthV4.setup() 호출 시 생성되며 이후 앱 삭제 전까지 바뀌지 않는다.<br/>
		*
		*  \~english
		* @brief Sign-in User Information
		* it includes user's tocken and DID value along with ProfileInfo.<br/>
		* <br/>
		* playerToken : Token associated with playerId required for sign-in verification<br/>
		* did : Device unique ID (DID). It is created when setup () is called for the first time and does not change until after the app is deleted.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		
		*/
		public class PlayerInfo : ProfileInfo {

			private const Int64 serialVersionUID = 3738492374234L;

			public String playerToken;
			public String did;
			public Dictionary<ProviderType, ProviderInfo> providerInfoData = new Dictionary<ProviderType, ProviderInfo>();
			public Dictionary<String, ProviderInfo> customProviderInfoData = new Dictionary<String, ProviderInfo>();

			public PlayerInfo() {}

			// public PlayerInfo(long playerId, String playerName, String playerImageUrl, 
			// 				String playerToken, String did, HashMap<ProviderType, ProviderInfo> providerInfoData) {
				public PlayerInfo(Int64 playerId, String playerName, String playerImageUrl, 
				String playerToken, String did, Dictionary<ProviderType, ProviderInfo> providerInfoData, Dictionary<String, ProviderInfo> customProviderInfoData) {

				this.playerId = playerId;
				this.playerName = playerName;
				this.playerImageUrl = playerImageUrl;

				this.playerToken = playerToken;
				this.did = did;
				this.providerInfoData = providerInfoData;
				this.customProviderInfoData = customProviderInfoData;
			}


			public PlayerInfo(JSONObject jsonParam) {

				if (jsonParam == null || jsonParam.count <= 0) return;

				jsonParam.GetField (ref this.playerId, "playerId");
				jsonParam.GetField (ref this.playerName, "playerName");
				jsonParam.GetField (ref this.playerImageUrl, "playerImageUrl");

				jsonParam.GetField (ref this.playerToken, "playerToken");
				jsonParam.GetField (ref this.did, "did");

				JSONObject providerInfoListJson = jsonParam.GetField("providerInfoData");
				if (providerInfoListJson != null && providerInfoListJson.count > 0) {		//	replace count check.  .isArray is not correct
					List<JSONObject> providerInfoJsonList = providerInfoListJson.list;

					foreach (JSONObject jsonItem in providerInfoJsonList) {

						ProviderInfo providerInfo = new ProviderInfo(jsonItem);
						providerInfoData.Add(providerInfo.providerType, providerInfo);
					}
				
				}
				JSONObject customProviderInfoListJson = jsonParam.GetField("customProviderInfoData");

				if (customProviderInfoListJson != null && customProviderInfoListJson.count > 0) {
					List<JSONObject> customProviderInfoJsonList = customProviderInfoListJson.list;
					foreach (JSONObject jsonItem in customProviderInfoJsonList) {

						ProviderInfo providerInfo = new ProviderInfo(jsonItem);
						customProviderInfoData.Add(providerInfo.providerName, providerInfo);
					}
				}
			}

			public override String toString() {

				StringBuilder sb = new StringBuilder();

				sb.Append("ProfileInfo { playerId = ");
				sb.Append(this.playerId);
				sb.Append(", playerName = ");
				sb.Append(this.playerName);
				sb.Append(", playerImageUrl = ");
				sb.Append(this.playerImageUrl);
				sb.Append(", playerToken = ");
				sb.Append(this.playerToken);
				sb.Append(", did = ");
				sb.Append(this.did);
				sb.Append(", providerInfoData = { ");

				var providerEnumerator = this.providerInfoData.GetEnumerator();
				while(providerEnumerator.MoveNext())
					sb.Append(providerEnumerator.Current.Value.toString() + "\n, ");
				sb.Append(" }");

				sb.Append(", customProviderInfoData = { ");

				var customProviderEnumerator = this.customProviderInfoData.GetEnumerator();
				while (customProviderEnumerator.MoveNext())
					sb.Append(customProviderEnumerator.Current.Value.toString() + "\n, ");
				sb.Append(" }");

				sb.Append(" }\n");

				return sb.ToString();
			}
		}

		/**
		*  \~korean
		* @brief 프로바이더 정보
		* 연결된 프로바이더의 UserId 를 포함하고 있다. providerUserId 가 없다면 연결된 상태가 아니다.<br/>
		* Provider 자동 로그인 (묵시적) 을 사용한다면 signIn(Provider) 호출 결과에서 providerUserId 도 같이 체크 하도록 한다.<br/>
		* <br/>
		* providerType : 현재 Provider 종류<br/>
		* providerUserId : Provider User 고유 ID (PlayerID 가 아니다)<br/>
		* providerEmail : Provider 계정에 등록된 email (default "")<br/>
		*
		*  \~english
		* @brief Identity Provider (IdP) Information
		* It contains the UserId of the associated Identity Provider. If there is no providerUserId, it is not connected.<br/>
		* If you are using Provider auto-login (implicit login), also check providerUserId in the result of signIn(Provider) call. <br/>
		* <br/>
		* providerType : Current Provider Type<br/>
		* providerUserId : Provider's Unique User ID (It is not PlayerID)<br/>
		* providerEmail : Provider's account email (default "")<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public class ProviderInfo {

			private const Int64 serialVersionUID = 9834592837492L;

			public ProviderType providerType;
			public String providerName;
			public String providerUserId;
			public String providerEmail = "";

			public ProviderInfo() {}

			public ProviderInfo(ProviderType providerType, String providerName, String providerUserId, String providerEmail) {

				this.providerType = providerType;
				this.providerName = providerName;
				this.providerUserId = providerUserId;
				this.providerEmail = providerEmail;
			}

			public ProviderInfo(JSONObject jsonParam) {

				if (jsonParam == null || jsonParam.count <= 0) return;

				int providerTypeNum = 0;
				jsonParam.GetField (ref providerTypeNum, "providerType");
				this.providerType = (ProviderType)Enum.ToObject(typeof(ProviderType) , providerTypeNum);

				jsonParam.GetField (ref this.providerName, "providerName");
				jsonParam.GetField (ref this.providerUserId, "providerUserId");
				jsonParam.GetField (ref this.providerEmail, "providerEmail");
			}

			public String toString() {

				StringBuilder sb = new StringBuilder();

				sb.Append(" { providerType = ");
				sb.Append(this.providerType);
				sb.Append(", providerName = ");
				sb.Append(this.providerName);
				sb.Append(", providerUserId = ");
				sb.Append(this.providerUserId);
				sb.Append(", providerEmail = ");
				sb.Append(this.providerEmail);
				sb.Append(" }\n");

				return sb.ToString();
			}
		}

		/**
		*  \~korean
		* @brief 서버 점검 및 업데이트 상태 표시 정보
		* title : 점검 제목<br/>
		* message : 점검 내용<br/>
		* button : 버튼의 Text 문구 ex) 확인<br/>
		* action : 버튼을 눌렀을때 동작될 행동<br/>
		* url : action 이 OPEN_URL 일 경우에 브라우징 될 URL<br/>
		* remainingTime : EXIT 일 경우 점검 완료까지 남은 초단위 시간. 시간은 실시간 갱신되며 0초가 되면 앱 종료<br/>
		* startDate : 점검시작일 YYYY-mm-dd HH:ii<br/>
		* endDate : 점검종료일 YYYY-mm-dd HH:ii<br/>
		* customerButton : 고객센터 버튼의 Text 문구 ex) 고객센터<br/>
		* customerLink : 고객센터 버튼을 눌렀을때 이동할 URL<br/>
		* exButtons : 점검 팝업 커스텀시 구성하기 위한 버튼 정보<br/>
		*
		*  \~english
		* @brief Server Maintenance and Update status display information 
		* title : Title<br/>
		* message : Contents<br/>
		* button : Text on the button ex) OK<br/>
		* action : Actions to be taken when the button is pressed<br/>
		* url : URL to be browsed when action is OPEN_URL<br/>
		* remainingTime : When action is EXIT, the time in seconds remaining until the maintenance completes. The time is updated in real time, and the app will be closed at 0 seconds.<br/>
		* startDate : start time YYYY-mm-dd HH:ii<br/>
		* endDate : end time YYYY-mm-dd HH:ii<br/>
		* customerButton : Customer Service Text on the button ex) Customer Service<br/>
		* customerLink : The URL to go to when the Customer Service button is clicked<br/>
		* exButtons : Button information to configure when customizing the maintenance popup<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public class AuthV4MaintenanceInfo {
			
			public String title;                    ///< \~korean 점검 제목  \~english Title
			public String message;                    ///< \~korean 점검 내용  \~english Contents
			public String button;                    ///< \~korean 버튼의 Text 문구 ex) 확인  \~english Text on the button ex) OK
			public AuthV4MaintenanceActionType action;///< \~korean 버튼을 눌렀을때 동작될 행동  \~english  Actions to be taken when the button is tapped
			public String url;                        ///< \~korean action 이 OPEN_URL 일 경우에 브라우징 될 URL  \~english  URL to be browsed when action is OPEN_URL
			public int remainingTime;                ///< \~korean EXIT 일 경우 점검 완료까지 남은 초단위 시간. 시간은 실시간 갱신되며 0초가 되면 앱 종료  \~english When action is EXIT, the time in seconds remaining until the maintenance completes. The time is updated in real time, and the app will be closed at 0 seconds.
			public String startDate;                ///< \~korean 점검시작일 YYYY-mm-dd HH:ii  \~english Maintenance start time YYYY-mm-dd HH:ii
			public String endDate;                  ///< \~korean 점검종료일 YYYY-mm-dd HH:ii  \~english Maintenance end time YYYY-mm-dd HH:ii
			public String customerButton;           ///< \~korean 고객센터 버튼의 Text 문구  \~english Customer Service Text on the button
			public String customerLink;             ///< \~korean 고객센터 버튼을 눌렀을때 이동할 URL  \~english The URL to go to when the Customer Service button is clicked
			public List<AuthV4MaintenanceExtraButton> exButtons = new List<AuthV4MaintenanceExtraButton>();                ///< \~korean 점검 팝업 커스텀시 구성하기 위한 버튼 정보  \~english Button information to configure when customizing the maintenance popup

			public AuthV4MaintenanceInfo() {}

			public AuthV4MaintenanceInfo(String title, String message, String button, 
										AuthV4MaintenanceActionType action, String url, int remainingTime, 
										String startDate, String endDate, String customerButton, String customerLink, List<AuthV4MaintenanceExtraButton> exButtons) {

				this.title = title;
				this.message = message;
				this.button = button;
				this.action = action;
				this.url = url;
				this.remainingTime = remainingTime;
				this.startDate = startDate;
				this.endDate = endDate;
				this.customerButton = customerButton;
				this.customerLink = customerLink;
				this.exButtons = exButtons;
			}

			public AuthV4MaintenanceInfo(JSONObject jsonParam) {

				if (jsonParam == null || jsonParam.count <= 0) return;

				jsonParam.GetField (ref this.title, "title");
				jsonParam.GetField (ref this.message, "message");
				jsonParam.GetField (ref this.button, "button");

				int actionNum = 0;
				jsonParam.GetField (ref actionNum, "action");;
				this.action = (AuthV4MaintenanceActionType)Enum.ToObject(typeof(AuthV4MaintenanceActionType) , actionNum);

				jsonParam.GetField (ref this.url, "url");
				jsonParam.GetField (ref this.remainingTime, "remainingTime");
				jsonParam.GetField (ref this.startDate, "startDate");
				jsonParam.GetField (ref this.endDate, "endDate");
				jsonParam.GetField (ref this.customerButton, "customerButton");
				jsonParam.GetField (ref this.customerLink, "customerLink");

				JSONObject exButtonsArray = jsonParam.GetField ("exButtons");
				if (exButtonsArray != null && exButtonsArray.count > 0) {
					List<JSONObject> jsonList = exButtonsArray.list;
					foreach (JSONObject jsonItem in jsonList) {
						AuthV4MaintenanceExtraButton button = new AuthV4MaintenanceExtraButton(jsonItem);
						this.exButtons.Add(button);
					}
				}
			}

			public String toString() {

				StringBuilder sb = new StringBuilder();

				sb.Append("AuthV4MaintenanceInfo { title = ");
				sb.Append(this.title);
				sb.Append(", message = ");
				sb.Append(this.message);
				sb.Append(", button = ");
				sb.Append(this.button);
				sb.Append(", action = ");
				sb.Append(this.action);
				sb.Append(", url = ");
				sb.Append(this.url);
				sb.Append(", remainingTime = ");
				sb.Append(this.remainingTime);
				sb.Append(", startDate = ");
				sb.Append(this.startDate);
				sb.Append(", endDate = ");
				sb.Append(this.endDate);
				sb.Append(", customerButton = ");
				sb.Append(this.customerButton);
				sb.Append(", customerLink = ");
				sb.Append(this.customerLink);

				sb.Append(", exButtons = [");
				if(this.exButtons != null && this.exButtons.Count > 0) {
					foreach (AuthV4.AuthV4MaintenanceExtraButton exButton in this.exButtons) {
						sb.Append(exButton.toString());
						sb.Append(", ");
					}
				}				
				sb.Append("]");
				sb.Append(" }\n");

				return sb.ToString();
			}
		}

		/**
		*  \~korean
		* @brief 점검 팝업을 커스터마이징 하기 위한 버튼 정보
		* action : 버튼을 눌렀을때 동작될 행동<br/>
		* url : action 이 OPEN_URL 일 경우에 브라우징 될 URL<br/>
		* button : 버튼의 Text 문구 ex) 확인<br/>
		*
		*  \~english
		* @brief Button information for customizing the maintenance popup 
		* action : Actions to be taken when the button is pressed<br/>
		* url : URL to be browsed when action is OPEN_URL<br/>
		* button : Text on the button ex) OK<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public class AuthV4MaintenanceExtraButton {

			public AuthV4MaintenanceActionType action;
			public String url;
			public String button;
			
			public AuthV4MaintenanceExtraButton(AuthV4MaintenanceActionType action, String url, String button)
			{
				this.action = action;
				this.url = url;
				this.button = button;
			}

			public AuthV4MaintenanceExtraButton(JSONObject jsonParam) {
				
				if (jsonParam == null)
					return;
				
				int actionNum = 0;
				jsonParam.GetField (ref actionNum, "action");;
				this.action = (AuthV4MaintenanceActionType)Enum.ToObject(typeof(AuthV4MaintenanceActionType) , actionNum);
				
				jsonParam.GetField (ref this.url, "url");
				jsonParam.GetField (ref this.button, "button");
			}

			public String toString() {

				StringBuilder sb = new StringBuilder();

				sb.Append("AuthV4MaintenanceExtraButton { action = ");
				sb.Append(this.action);
				sb.Append(", url = ");
				sb.Append(this.url);
				sb.Append(", button = ");
				sb.Append(this.button);
				sb.Append (" }\n");

				return sb.ToString();
			}
		}

		/**
		*  \~korean
		* @brief AuthV4 초기화 결과 통지
		* result : 최초 실행 시 DID 를 받아오지 못하거나, Provider List 를 받아오지 못하면 실패한다. 그 외 성공.<br/>
		* isAutoSignIn : 로컬에 이전 세션이 남아있는지 여부. true 일 경우 SignIn(ProviderType.AUTO) 을 호출 한다.<br/>
		*   그 외의 경우 providerTypeList 중 하나로 SignIn 을 요청이 가능 하다.<br/>
		* providerTypeList : 현재 단말에서 사인-인 가능한 Provider List 이다.<br/>
		*   단말의 현재 지역 (IP) 에 따라 다르게 보여질 수 있다.<br/>
		*   GUEST 를 포함하고 있으며 일부 지역 혹은 환경에서는 GUEST 도 불가능 할 수 있다. (분산서버)<br/>
		*
		*  \~english
		* @brief AuthV4 initialization result callback
		* result : When initial initialize is executed, it fails if it does not receive DID or Provider List. Otherwise, it is a success.<br/>
		* isAutoSignIn : Whether there is an old session left on local storage. If true, call SignIn (ProviderType.AUTO).<br/>
		*   Otherwise, you can request SignIn as one of providerTypeList.<br/>
		* providerTypeList : A provider list that can be signed in from the current device.<br/>
		*   And may be different depending on the current area (IP) of the device.<br/>
		*   GUEST is included, and in some areas or environments GUEST may not be possible.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public delegate void onAuthV4Setup(ResultAPI result, Boolean isAutoSignIn, String did, List<ProviderType> providerTypeList);

		/**
		*  \~korean
		* @brief AuthV4 사인-인 결과 통지
		* result : SUCCESS 가 아닐 경우 다시 로그인 화면으로 돌아가야한다.
		*   세션이 만료되었거나 정상적이지 않은 정보 일 경우 INVALID_SESSION 이 올 수 있으며<br/>
		*   유저가 외부 로그인 창을 취소 하였을 경우 CANCELED,<br/>
		*   서버 응답 지연 등에 의해 NETWORK 나 TIMEOUT 이 올 수 있다.<br/>
		*   그 외 실패에 대한 상황은 RESPONSE_FAIL 이다.<br/>
		* playerInfo : result 가 SUCCESS 일 경우 playerInfo 에 사인-인에 성공한 유저의 정보가 담겨져 있다.<br/>
		*   providerInfoData 에는 연결되어있는 Provider 의 정보가 들어있다. 없는 Provider 는 연결되어있지 않은 상태.<br/>
		*
		*  \~english
		* @brief AuthV4  Sign-in result callback
		* result : If it is not SUCCESS, you should return to the login screen again.
		*   If the session has expired or is not normal, INVALID_SESSION is returned,<br/>
		*   or CANCELED if the user cancels the external login window. <br/>
		*   NETWORK or TIMEOUT can be caused by delay of server response, <br/>
		*   and for other failures are RESPONSE_FAIL.<br/>
		* playerInfo : If result is SUCCESS, the playerInfo contains information about the user who successfully signed in.<br/>
		*   providerInfoData contains the information of the connected Provider. The missing Provider is not connected.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public delegate void onAuthV4SignIn(ResultAPI result, PlayerInfo playerInfo);

		/**
		*  \~korean
		* @brief AuthV4 사인-아웃 결과 통지
		* result : setup 이 되지 않았거나 (NEED_INITIALIZE) SignIn 혹은 SignOut 이 진행중일 경우 (IN_PROGRESS) 실패가 될 수 있다.<br/>
		*   그 외 성공<br/>
		*
		*  \~english
		* * @brief AuthV4 Sign out result callback
		* result : If setup fails (NEED_INITIALIZE) or SignIn or SignOut is in progress (IN_PROGRESS), it may fail.<br/>
		*   Otherwise, success.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public delegate void onAuthV4SignOut(ResultAPI result); 

		/**
		*  \~korean
		* @brief AuthV4 Provider 연동 결과 통지
		* result : 지원하지 않는 ProviderType (INVALID_PARAM) 이거나 <br/>
		*   Sign 이 진행중일 경우 (IN_PROGRESS) 실패가 될 수 있다.<br/>
		*   SignIn 이 되지 않은 상태에서는 INVALID_SESSION 에러가 된다.<br/>
		*   이미 연결되어있는 Provider 의 경우 INVALID_PARAM 에러가 되며<br/>
		*   해당 Provider 에 대해 disconnect 를 먼저 호출 후 다시 connect 해야 한다.<br/>
		*   연결을 시도한 Provider 가 이미 다른 playerId 에 연결되어 있을 경우 CONFLICT_PLAYER 가 되며<br/>
		*   conflictPlayer 객체에 해당 충돌 유저 정보가 포함되어있다.<br/>
		* conflictPlayer : result 가 CONFLICT_PLAYER 일 경우 충돌 유저에 대한 정보 이다.<br/>
		*   playerId 와 충돌난 ProviderInfo 의 정보만 포함되어있다.<br/>
		*   playerToken 은 비어 있음.<br/>
		*
		*  \~english
		* @brief AuthV4 Provider connect result callback
		* result :If ProviderType is not supported (INVALID_PARAM) <br/>
		*   or if the Sign-in is in progress (IN_PROGRESS), it may fail.<br/>
		*   If sign-in fails, an INVALID_SESSION error is returned.<br/>
		*   An INVALID_PARAM error will occur if the provider is already connected. <br/>
		*   In this case, you must first call disconnect for the provider and then connect again.<br/>
		*   CONFLICT_PLAYER is returned if the provider attempting to connect is already connected to another playerId<br/>
		*   and the conflictPlayer object returned contains the corresponding conflict user information.<br/>
		* conflictPlayer : If result is CONFLICT_PLAYER, it contains information about the conflicting user.<br/>
		*   Only the information of the ProviderInfo that conflicted with playerId is included.<br/>
		*   playerToken is empty.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public delegate void onAuthV4Connect(ResultAPI result, PlayerInfo conflictPlayer); 

		/**
		*  \~korean
		* @brief AuthV4 Provider 연동 해지 결과 통지
		* 인증 서버에 disconnect 상황을 전달하고 이후 요청한 Provider 를 Logout 시킨다.<br/>
		* <br/>
		* result : 인증 서버에 전달이 실패할 경우와 Sign 이 진행중이거나 setup 이 되지 않은 상황,<br/>
		*   또는 이미 disconnected 된 Provider 일 경우 실패가 될 수 있다.<br/>
		*   그 외 성공.<br/>
		*
		*  \~english
		* @brief AuthV4 Provider disconnect result callback
		* It pass the disconnect status to the authentication server and then logout the requested provider.<br/>
		* <br/>
		* result : It can fail if the delivery to the authentication server fails, if the Sign-in is in progress, if it is not setup, or if it is already a disconnected Provider.<br/>
		*   Otherwise, success.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public delegate void onAuthV4Disconnect(ResultAPI result); 

		/**
		*  \~korean
		* @brief AuthV4 Profile Profile 정보 요청 결과 통지
		* 프로필 서버에 요청한 playerId 들에 대한 profile 정보를 전달한다.<br/>
		* <br/>
		* result : 요청에 대한 결과. 실패일 경우 profileInfoList 는 비어있다.<br/>
		* profileInfoList : 요청한 playerId 들에 대한 profile 정보.<br/>
		*   각각 playerId, playerName, playerImageUrl 이 포함되어 있다.<br/>
		*
		*  \~english
		* @brief AuthV4 Profile information request result callback
		* The profile information of the playerIds which are requested to the profile server is returned.<br/>
		* <br/>
		* result : Results for request. In case of failure, profileInfoList is empty.<br/>
		* profileInfoList : Profile information for requested playerIds.<br/>
		*   Each profile has playerId, playerName, playerImageUrl.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public delegate void onAuthV4GetProfile(ResultAPI result, List<ProfileInfo> profileInfoList);

		/**
		*  \~korean
		* @brief AuthV4 Profile UI 요청 결과 통지
		* UI 창이 닫히면 호출 된다.<br/>
		* <br/>
		* result : 입력된 playerId 가 잘못된 형태가 아니면 성공<br/>
		*
		*  \~english
		* @brief AuthV4 Profile UI request result callback
		* Invoked when the UI window is closed.<br/>
		* <br/>
		* result : If the entered playerId is not of the wrong type, it succeeds<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		
		*/
		public delegate void onAuthV4ShowProfile(ResultAPI result); 

		/**
		*  \~korean
		* @brief AuthV4 1:1 문의 UI 요청 결과 통지
		* UI 창이 닫히면 호출 된다.<br/>
		* <br/>
		* result : setup 이 되어있지 않을 경우 NEED_INITIALIZE 가 발생할 수 있다. 그 외 성공.<br/>
		*
		*  \~english
		* @brief AuthV4 1:1 Inquiry UI request result callback
		* Invoked when the UI window is closed.<br/>
		* <br/>
		* result : NEED_INITIALIZE can occur if 'setup' has not been called. Otherwise, success.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
        public delegate void onAuthV4ShowInquiry(ResultAPI result);

		/**
		*  \~korean
		* @brief AuthV4 내 문의 UI 요청 결과 통지
		* UI 창이 닫히면 호출 된다.<br/>
		* <br/>
		*
		*  \~english
		* @brief AuthV4 My Inquiry UI request result callback
		* Invoked when the UI window is closed.<br/>
		* <br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
        public delegate void onAuthV4ShowMyInquiry(ResultAPI result);


		/**
		*  \~korean
		* @brief AuthV4 챗봇 1:1 문의 UI 요청 결과 통지
		* UI 창이 닫히면 호출 된다.<br/>
		* <br/>
		* result : setup 이 되어있지 않을 경우 NEED_INITIALIZE 가 발생할 수 있다. 그 외 성공.<br/>
		*
		*  \~english
		* @brief AuthV4 Chatbot 1:1 Inquiry UI request result callback
		* Invoked when the UI window is closed.<br/>
		* <br/>
		* result : NEED_INITIALIZE can occur if 'setup' has not been called. Otherwise, success.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
        public delegate void onAuthV4ShowChatbotInquiry(ResultAPI result);

		/**
		*  \~korean
		* @brief AuthV4 약관 다시보기 정보 표시 결과 통지
		* UI 창이 닫히면 호출 된다.<br/>
		* <br/>
		* result : setup 이 되어있지 않을 경우 NEED_INITIALIZE 가 발생할 수 있다. 그 외 성공.<br/>
		*
		*  \~english
		* @brief AuthV4 Terms of Service UI request result callback
		* Invoked when the UI window is closed.<br/>
		* <br/>
		* result : NEED_INITIALIZE can occur if 'setup' has not been called. Otherwise, success.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public delegate void onAuthV4ShowTerms(ResultAPI result); 

		/**
		*  \~korean
		* @brief AuthV4 성인인증 정보 표시 결과 통지
		* UI 창이 닫히면 호출 된다.<br/>
		* <br/>
		* result : 사인-인 이 필요하기 때문에 사인-인 이 되어있지 않을 경우 INVALID_SESSION 이 올 수 있다.<br/>
		*   성인인증에 정상적으로 진행되었을 경우 SUCCESS 가 된다.<br/>
		*
		*  \~english
		* @brief AuthV4 Adult authentication request result callback
		* Invoked when the UI window is closed.<br/>
		* <br/>
		* result : INVALID_SESSION can come if you do not have a sign-in because you need a sign-in. <br/>
		*    SUCCESS, if you have successfully completed the adult authentication<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public delegate void onAuthV4AdultConfirm(ResultAPI result); 

		/**
		*  \~korean
		* @brief AuthV4 Provider 상태 조회 결과 통지
		* 요청한 Provider 로 Login 이 되어있지 않으면 Login 까지 시도한다.<br/>
		* 현재 playerId 와 connect 를 요청하지는 않는다.<br/>
		* <br/>
		* result : SUCCESS 는 조회에 성공했다는 뜻이며 providerInfo 에 providerUserId 여부로 로그인된 유저를 판단해야 한다.<br/>
		* providerInfo : 실제 provider 에 로그인 되어있다면 providerUserId 가 존재한다. 그 외 providerType 만 존재.<br/>
		*
		*  \~english
		* @brief AuthV4 Provider status request result callback
		* If the requested provider is not logged in, it tries to login.<br/>
		* It does not request connect with the current playerId.<br/>
		* <br/>
		* result : SUCCESS means that the query was successful and you can determine whether the requested user is a logged-in user or not by whether providerInfo has a providerUserId. <br/>
		* providerInfo : If you are logged in to the actual provider, there is a providerUserId. Otherwise, only providerType exists.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public delegate void onDeviceProviderInfo(ResultAPI result, ProviderInfo providerInfo); 

		/**
		*  \~korean
		* @brief AuthV4 서버 점검 조회 결과 통지
		* Configuration 에 설정한 ServerId 로 점검상태 여부를 체크한다.<br/>
		* 요청시 isShow 가 true 였을 경우 점검 팝업이 노출되고 닫힌 이후에 불리게 된다.<br/>
		* <br/>
		* result : 서버에 조회 요청이 성공하면 SUCCESS 가 된다.<br/>
		*   SUCCESS 이지만 점검 공지 내용이 없다면 maintenanceInfo 는 비어있게 된다.<br/>
		* maintenanceInfo : 점검 공지 팝업에 노출되는 동작에 대한 정보 이다.<br/>
		*   요청시 isShow 를 false 로 주었다면 이 정보를 가지고 직접 UI 를 구성하여 노출하여야 한다.<br/>
		*
		*  \~english
		* @brief AuthV4 Sever maintenance check request result callback
		* Check whether the server is in the server maintenance with the ServerId set in Configuration.<br/>
		* If isShow is true on request, the maintenance popup is exposed and this callback will be called after the popup is closed.<br/>
		* <br/>
		* result : If the query to the server is successful, it will be SUCCESS.<br/>
		*   If it is SUCCESS but there is no notice of maintenance, maintenanceInfo will be empty.<br/>
		* maintenanceInfo :  Maintenance popup information about the action that is exposed to the popup.<br/>
		*   If isShow is set to false on request, you must make and expose the UI directly by your self with this information.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public delegate void onAuthV4Maintenance(ResultAPI result, List<AuthV4MaintenanceInfo> maintenanceInfoList); 

		/**
		*  \~korean
		* @brief HIVE SDK 사용자 Provider 친구목록의 profile 요청 결과 통지.
		* result : API 호출 결과.<br/>
		* 실패일 경우 providerUserIdList는 비어있다.<br/>
		* providerType : API 호출시 요청한 ProviderType.<br/>
		* providerUserIdList : 요청한 providerUserId들에 대한 id 정보.<br/>
		*
		*  \~english
		* @brief HIVE SDK  Provider friend list request result callback
		* result : Result of API.<br/>
		* In case of failure, providerUserIdList is empty.<br/>
		* providerType : The ProviderType requested when calling the API.<br/>
		* providerUserIdList : The requested providerUserId list.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public delegate void onGetProviderFriendsList(ResultAPI result, ProviderType providerType, Dictionary<String,Int64> providerUserIdList);

		/**
		*  \~korean
		* @brief {@link #resolveConflict(AuthV4ResolveConflictListener)} 호출 결과 통지
		* AuthV4.resolveConflict() 에서 사용<br/>
		* 앱 내에서 로그아웃 할 수 없는 IDP (Apple GameCenter)의 경우 로그아웃은 불가능 하다.<br/>
		* 따라서, 이런경우 실제 IDP 로그아웃에 실패했어도 내부 충돌 정보는 삭제 됩니다.
		*
		*  \~english
		* @brief Result callback of {@link #resolveConflict(AuthV4ResolveConflictListener)} call 
		* AuthV4.resolveConflict() use it.<br/>
		* Logging out is not possible for IdPs that can not log out of the app. ex) Apple GameCenter<br/>
		* Therefore, even if the actual IDP logout fails, the internal conflict information is deleted .
		*
		*  \~
		* @see #resolveConflict(AuthV4ResolveConflictListener)
		* @ingroup AuthV4
		*
		*/
		public delegate void onAuthV4ResolveConflict(ResultAPI result);

		/**
		*  \~korean
		* @brief {@link #showDeviceManagement(AuthV4ShowDeviceManagementListener)} 호출 결과 통지
		* AuthV4.showDeviceManagement() 에서 사용<br/>
		* 기기 관리 서비스를 이용하면서 등록이 안된 기기는 로그인이 불가능 하다.<br/>
		* 따라서, 이런 경우 AuthV4NotRegisteredDevice 에러를 받게 된다.
		*
		*  \~english
		* @brief {@link #showDeviceManagement(AuthV4ShowDeviceManagementListener)} call 
		* AuthV4.showDeviceManagement() use it.<br/>
		* Devices that are not registered while using the device management service cannot sign in.<br/>
		* So, in this case, you will get an 'AuthV4NotRegisteredDevice' error.
		*
		*  \~
		* @see #showDeviceManagement(AuthV4ShowDeviceManagementListener)
		* @ingroup AuthV4
		*/
		public delegate void onAuthV4ShowDeviceManagement(ResultAPI result);
		
		/**
         *  \~korean
         * @brief {@link #getHiveTalkPlusLoginToken(AuthV4GetHiveTalkPlusLoginTokenListener)} 호출 결과 통지
         * AuthV4.getHiveTalkPlusLoginToken() 에서 사용<br/>
         * HiveTalkPlus 로그인시 필요한 로그인 토큰을 얻어온다.
         *
         *  \~english
         * @brief {@link #getHiveTalkPlusLoginToken(AuthV4GetHiveTalkPlusLoginTokenListener)} call
         * AuthV4.getHiveTalkPlusLoginToken() use it.<br/>
         * Get the required login token when logging in to HiveTalkPlus.
         *
         *  \~
         * @see #getHiveTalkPlusLoginToken(AuthV4GetHiveTalkPlusLoginTokenListener)
         * @ingroup AuthV4
         */
		public delegate void onAuthV4GetHiveTalkPlusLoginToken(ResultAPI result, string loginToken);


		public delegate void onAuthV4RequestPermissionViewData(ResultAPI result,PermissionViewData data);


			//4.7.0_ADDED
			/**
			*  \~korean
			* @brief AuthV4 Game Center 로그인 안내 팝업 결과 통지<br>
			* <br>
			* @param isDismiss : 팝업이 정상적으로 닫힌 경우 true가 전달된다.
			*
			*  \~english
			* @brief AuthV4 Result callback after Game Center login pops up<br>
			* <br>
			* @param isDismiss : Returns true if the popup is successfully closed.
			*  \~
			*/
			public delegate void onAuthV4DialogDismiss(bool isDismiss);
		/**
		*  \~korean
		* @brief AuthV4 초기화를 수행한다.
		* Configuration 영역을 제외한 모든 API 중 가장 먼저 호출되어야 하며<br/>
		* 선 호출이 되지 않을시 일부 API 에서는 NEED_INITIALIZE 에러가 발생할 수 있다.<br/>
		* Android 의 경우 초기 퍼미션 요청 UI 동작이 추가되며<br/>
		* 이후 약관 동의, 다운로드 체크, DID 설정, config.xml 설정, ProviderList 설정 등이 진행 된다.<br/>
		* 최초 실행 시 DID 를 받아오지 못하거나, Provider List 를 받아오지 못하면 listener 결과로 실패가 내려간다.<br/>
		* listener 에 포함된 providerTypeList 를 통해 signIn 에 필요한 UI 를 구성해야 한다.<br/>
		* 직접 UI 를 구현하지 않을 시 showSignIn() 을 이용한다.<br/>
		* <br/>
		* AuthV4 의 setup 이 되면 기존의 Auth 와 Social 영역의 기능은 사용할 수 없게된다.<br/>
		* 반대의 경우에도 Auth 로 initialize 가 되면 AuthV4 와 Provider 영역은 사용할 수 없게 된다.<br/>
		* 다른 기능 단위에서 (Push, Pomotion, IAP 등) 필요한 vid 는 AuthV4 로 setup 한 경우 playerId 를 String 으로 넣어주면 된다.<br/>
		*
		* @param listener onAuthV4Setup AuthV4 초기화 결과 통지<br/>
		*                 result : 최초 실행 시 DID 를 받아오지 못하거나, Provider List 를 받아오지 못하면 실패한다. 그 외 성공.<br/>
		*                 isAutoSignIn : 로컬에 이전 세션이 남아있는지 여부. true 일 경우 SignIn(ProviderType.AUTO) 을 호출 한다.<br/>
		*                   그 외의 경우 providerTypeList 중 하나로 SignIn 을 요청이 가능 하다.<br/>
		*                 providerTypeList : 현재 단말에서 사인-인 가능한 Provider List 이다.<br/>
		*                   단말의 현재 지역 (IP) 에 따라 다르게 보여질 수 있다.<br/>
		*                   GUEST 를 포함하고 있으며 일부 지역 혹은 환경에서는 GUEST 도 불가능 할 수 있다. (분산서버)<br/>
		*
		*  \~english
		* @brief AuthV4 Perform initialization.
		* It should be called first among all the APIs except for the configuration area,<br/>
		* and if this is not called first, some API may cause a NEED_INITIALIZE error.<br/>
		* On Android, the permission request UI is exposed<br/>
		* Afterwards, consent to the Terms, download check, DID setting, config.xml setting, ProviderList setting and etc. are performed.<br/>
		* If the DID is not received on the first execution, or the provider list is not received, the result is sent to the listener as failure.<br/>
		* You need to configure the UI for signIn using the providerTypeList contained in listener.<br/>
		* If you do not implement the UI directly, use showSignIn().<br/>
		* <br/>
		* When the setup of AuthV4 is called, the functions of the existing Auth and Social areas are disabled.<br/>
		* In the opposite case, when Auth is initialized, AuthV4 and Provider areas are disabled.<br/>
		* Required VID for other functional units (Push, Pomotion, IAP, etc.), you need to insert the playerId instead,if you set up with AuthV4.<br/>
		*
		* @param listener onAuthV4Setup AuthV4 Setup request result callback<br/>
		*                 result : If the DID is not received or the provider list is not received, it fails. Otherwise, success.<br/>
		*                 isAutoSignIn : Whether an old session remains local storage. If true, call SignIn (ProviderType.AUTO).<br/>
		*                   Otherwise, you can request SignIn as one of providerTypeList.<br/>
		*                 providerTypeList : A provider list that can be signed in at the current device.<br/>
		*                   And may be different depending on the current area (IP) of the device.<br/>
		*                   GUEST is included, and in some areas or environments GUEST may not be possible.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void setup(onAuthV4Setup listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "setup", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		*  \~korean
		* @brief 주어진 providerType 에 따라 signIn 을 요청 한다. 
		* 로컬에 저장된 세션이 있는데 AUTO 가 아니거나<br/>
		* 로컬에 저장된 세션이 없는데 AUTO 이면 INVALID_PARAM 이 발생한다.<br/>
		* 또한, isAutoSignIn() 호출로도 AUTO 인지 아닌지 체크할 수 있다.<br/>
		* 이미 sign 이 진행중이면 IN_PROGRESS 가 발생하며,<br/>
		* GUEST 가 아닌 다른 ProviderType 의 경우 외부 인증창이 한번 더 노출 될 수 있다.<br/>
		* <br/>
		* signIn 에 성공하게 되면 listener 에 포함되어있는 playerInfo 를 통해 유저 정보를 얻고<br/>
		* 연동된 provider 상태를 UI 에 표시해 주면 된다.<br/>
		* <br/>
		* signIn(AUTO) 의 경우 저장된 playerId 의 세션만으로 sign-in 되기 때문에<br/>
		* 묵시적 로그인을 수행하는 Provider가 실제 단말에도 로그인 되어있는지의 여부를 체크하고 계정을 동기화 하려면 checkProvider() 를 호출 하여야 한다.<br/>
		* 다를 경우 signOut() 후 signIn(Provider) 로 계정 전환을 할 수 있다.<br/>
		* <br/>
		* BLACKLIST 일 경우 SDK에서 제재 팝업을 띄우고 BLACKLIST 에러를 내려 준다.<br/>
		*
		* @param providerType signIn 요청할 ProviderType
		*
		* @param listener onAuthV4SignIn AuthV4 사인-인 결과 통지
		*
		*  \~english
		* @brief Requests signIn according to the given providerType. 
		* If there is a locally stored session and it is not AUTO<br/>
		* or if there is no session stored locally and it is AUTO, it will cause INVALID_PARAM error.<br/>
		* You can also check whether it is AUTO or not with isAutoSignIn ().<br/>
		* IN_PROGRESS occurs when the sign-in is already in progress,<br/>
		* and the external authentication window can be exposed once again for ProviderType other than GUEST.<br/>
		* <br/>
		* If signIn succeeds, you can get the user information through the playerInfo contained in the listener<br/>
		* and display the status of the linked provider in the UI<br/>
		* <br/>
		* In the case of signIn (AUTO), since it is sign-in only with the session of the stored playerId,<br/>
		* you should call checkProvider () to check whether the provider performing the implicit login is also logged in the actual terminal and to synchronize the account. <br/>
		* If it is different, you can call signOut () and call signIn (Provider) to switch the account.<br/>
		* <br/>
		* In the case of BLACKLIST, SDK will pop up a restriction popup and issue a BLACKLIST error.<br/>
		*
		* @param providerType  ProviderType to request signIn
		*
		* @param listener onAuthV4SignIn AuthV4 signIn result callback 
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void signIn(ProviderType providerType, onAuthV4SignIn listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "signIn", listener);
			jsonParam.AddField("providerType", providerType.ToString());

			HIVEUnityPlugin.callNative (jsonParam);
			HIVEUnityPlugin.IMECompositionModeOn();
		}

		/**
        *  \~korean
        * @brief 자체 구현한 커스텀 로그인 후 획득한 authKey 값으로 signIn을 요청 한다.
        * <br/>
        * signIn 에 성공하게 되면 handler 에 포함되어있는 playerInfo 를 통해 유저 정보를 얻는다.<br/>
        *
        * @param signInListener onSignIn AuthV4 사인-인 결과 통지
        *
        *  \~english
        * @brief Requests signIn with authKey gained from customized provider login implementation.
        * <br/>
        * If signIn succeeds, you can get the user information through the playerInfo contained in the handler<br/>
        *
        * @param signInListener onSignIn AuthV4 signIn result callback
        *  \~
        * @ingroup AuthV4
        *
        */
		public static void signInWithAuthKey(String authKey, onAuthV4SignIn listener) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "signInWithAuthKey", listener);
			jsonParam.AddField("authKey", authKey);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		*  \~korean
		* @brief 로컬에 있는 세션을 제거 한다.
		* 로그인 된 모든 Provider 들을 로그아웃 시키며 결과와 상관 없이 로컬 세션을 제거하고 성공 콜백을 준다.<br/>
		* signIn/Out 이 진행중일 경우 IN_PROGRESS 가 발생할 수 있다.<br/>
		* <br/>
		* 게스트 상태인 player 의 경우 (연동된 Provider 가 하나도 없는 상태) 다시 사인-인 할 수 없게 되니 주의.<br/>
		* @param listener onAuthV4SignOut 사인-아웃 결과 통지
		*
		*  \~english
		* @brief it remove the local session..
		* It logs out all the providers that are logged in, removes the local session and gives a success callback regardless of the result.<br/>
		* IN_PROGRESS may occur when signIn / Out is in progress<br/>
		* <br/>
		* Note that in the case of a player in the guest state (no connected Provider), the player will not be able to sign in again.<br/>
		* @param listener onAuthV4SignOut Sign-out result callback
		*
		*  \~
		*
		* @ingroup AuthV4
		*
		*/
		public static void signOut(onAuthV4SignOut listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "signOut", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
     	* \~korean
		* @brief 계정을 삭제하고 로컬에 있는 세션을 제거 한다.
		* 서버에 계정 삭제를 요청하고 연동된 모든 Provider 들을 해제 한다.
		* 성공 시 로그인 된 모든 Provider 들을 로그아웃 시키며 로컬 세션을 제거하고 성공 콜백을 준다.<br></br>
		* signIn/Out 이 진행중일 경우 IN_PROGRESS 가 발생할 수 있다.<br></br>
		* <br></br>
		* @param listener onAuthV4SignOut 계정 삭제 결과 통지
		*
		* \~english
		* @brief Delete the account and remove the local session.
		* Request to the server to delete the account and disconnected all providers.
		* If success, all logged in providers are logged out, the local session is removed, and a success callback is given.
		* IN_PROGRESS may occur when signIn / Out is in progress<br></br>
		* <br></br>
		* @param listener onAuthV4SignOut Delete account result callback
		*
		* \~
		*
		* @ingroup AuthV4
		*/
		public static void playerDelete(onAuthV4SignOut listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "playerDelete", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		*  \~korean
		* @brief 현재 사인-인 된 유저에 요청한 provider 와 연동 한다.
		* 요청한 Provider 로 로그인을 시도 한 뒤, 로그인에 성공하면 인증서버에 연동 요청을 보낸다.<br/>
		* 해당 Provider 의 UserId 에 이미 매칭된 playerId 가 있을 경우 listener 의 result 에 CONFLICT_PLAYER 에러를 내려준다.<br/>
		* 이 경우 listener 에 포함되어 있는 conflictPlayer 객체의 충돌 유저 정보를 가지고 유저에게 계정 선택을 위한 UI 를 구성 해야한다.<br/>
		* 유저가 선택을 하면 선택된 유저 정보로 selectConflict() 를 호출해 주면 된다.<br/>
		* <br/>
		* 직접 UI 를 구성하지 않을 경우 showConflictSelection() 을 이용할 수 있다.<br/>
		* 이 경우 selectConflict() 까지 진행되며 사인-인 완료된 결과까지 받을 수 있다.<br/>
		* <br/>
		* 연동이 완료되면 해당 상태를 앱 내 UI 에 갱신해 준다.<br/>
		* <br/>
		* BLACKLIST 일 경우 SDK에서 제재 팝업을 띄우고 BLACKLIST 에러를 내려 준다.<br/>
		*
		* @param providerType connect 요청할 ProviderType.<br/>
		*                     AUTO 나 GUEST 는 INVALID_PARAM 에러를 발생시킨다.<br/>
		*
		* @param listener onAuthV4Connect <br/>
		*                 <br/>
		*                 result : 지원하지 않는 ProviderType (INVALID_PARAM) 이거나 <br/>
		*                   Sign 이 진행중일 경우 (IN_PROGRESS) 실패가 될 수 있다.<br/>
		*                   SignIn 이 되지 않은 상태에서는 INVALID_SESSION 에러가 된다.<br/>
		*                   이미 연결되어있는 Provider 의 경우 INVALID_PARAM 에러가 되며<br/>
		*                   해당 Provider 에 대해 disconnect 를 먼저 호출 후 다시 connect 해야 한다.<br/>
		*                   연결을 시도한 Provider 가 이미 다른 playerId 에 연결되어 있을 경우 CONFLICT_PLAYER 가 되며<br/>
		*                   conflictPlayer 객체에 해당 충돌 유저 정보가 포함되어있다.<br/>
		*                 <br/>
		*                 conflictPlayer : result 가 CONFLICT_PLAYER 일 경우 충돌 유저에 대한 정보 이다.<br/>
		*                   playerId 와 충돌난 ProviderInfo 의 정보만 포함되어있다.<br/>
		*                   playerToken 은 비어 있음.<br/>
		*
		*  \~english
		* @brief It connects to the currently requested provider for the currently signed-in user.
		* After logging in with the requested provider, if it is successful, it sends an linking request to the authentication server.<br/>
		* If there is a playerId already linked to the UserId of the corresponding provider, a CONFLICT_PLAYER error is returned in the result of the listener.<br/>
		* In this case, you should configured an UI for account selection to the user with the conflict user information of the conflictPlayer object included in the listener.<br/>
		* When the user makes a selection, calls selectConflict () with the selected user information.<br/>
		* <br/>
		* If you do not customize the UI, you can use showConflictSelection().<br/>
		* In this case, when the user selects it, it can proceed to selectConflict() and receive the result of the sign-in completed.<br/>
		* <br/>
		* When the linking is completed, the corresponding state should be updated on the UI of the app.<br/>
		* <br/>
		* In the case of BLACKLIST, SDK will show a restriction popup and issue a BLACKLIST error.<br/>
		*
		* @param providerType ProviderType to request connect.<br/>
		*                     AUTO or GUEST causes an INVALID_PARAM error.<br/>
		*
		* @param listener onAuthV4Connect <br/>
		*                 <br/>
		*                 result :If ProviderType is not supported (INVALID_PARAM) <br/>
		*                   or if the Sign-in is in progress (IN_PROGRESS), it may fail.<br/>
		*                   If sign-in fails, an INVALID_SESSION error is returned.<br/>
		*                   An INVALID_PARAM error will occur if the provider is already connected. <br/>
		*                   In this case, you must first call disconnect for the provider and then connect again.<br/>
		*                   CONFLICT_PLAYER is returned if the provider attempting to connect is already connected to another playerId<br/>
		*                   and the conflictPlayer object returned contains the corresponding conflict user information.<br/>
		*                 conflictPlayer : If result is CONFLICT_PLAYER, it contains information about the conflicting user.<br/>
		*                   Only the information of the ProviderInfo that conflicted with playerId is included.<br/>
		*                   playerToken is empty.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void connect(ProviderType providerType, onAuthV4Connect listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "connect", listener);
			jsonParam.AddField("providerType", providerType.ToString());

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		* 자체 구현한 커스텀 로그인 후 획득한 authKey 값으로 connect를 요청 한다.
		* <br>Requests connect with authKey gained from customized provider login implementation.
		*
		* <br><h4>korean</h4>
		* connect 에 성공하게 되면 handler 에 포함되어있는 playerInfo 를 통해 유저 정보를 얻는다.<br>
		*
		*
		* <br><h4>english</h4>
		* If connect succeeds, you can get the user information through the playerInfo contained in the handler<br>
		*
		* @param authKey AuthKey to request connect
		* @param listener onAuthV4Connect AuthV4 connect result callback
		*/
		public static void connectWithAuthKey(String authKey, onAuthV4Connect listener)
		{

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "connectWithAuthKey", listener);
			jsonParam.AddField("authKey", authKey);

			HIVEUnityPlugin.callNative(jsonParam);
		}

		/**
		*  \~korean
		* @brief 현재 사인-인 된 유저에 요청한 provider 와 연동을 해제 한다.
		* 인증 서버에 disconnect 상황을 전달하고 이후 요청한 Provider 를 Logout 시킨다.<br/>
		* 인증 서버에 전달이 실패할 경우와 Sign 이 진행중이거나 setup 이 되지 않은 상황,<br/>
		* 또는 이미 disconnected 된 Provider 일 경우 실패가 될 수 있다.<br/>
		* <br/>
		* disconnected 로 인해 연동이 모두 해제될 경우 게스트 상태가 될 수 있으니 주의.<br/>
		*
		* @param providerType disconnect 요청할 ProviderType.<br/>
		*
		* @param listener onAuthV4Disconnect
		*                 result : 인증 서버에 전달이 실패할 경우와 Sign 이 진행중이거나 setup 이 되지 않은 상황,<br/>
		*                   또는 이미 disconnected 된 Provider 일 경우 실패가 될 수 있다.<br/>
		*                   그 외 성공.<br/>
		*
		*
		*  \~english
		* @brief Releases the currently signed-in user from the requested provider.
		* It pass the disconnect status to the authentication server and then logout the requested provider.<br/>
		* If delivery fails to the authentication server, if the Sign or setup is in progress, <br/>
		* or if the provider is already disconnected, it may fail.<br/>
		* <br/>
		* Note that if all account link are disconnected due to 'disconnect', it may become a guest state.<br/>
		*
		* @param providerType disconnect 요청할 ProviderType.<br/>
		*
		* @param listener onAuthV4Disconnect
		*                 result : If delivery fails to the authentication server, if the Sign or setup is in progress, <br/>
		*                   or if the provider is already disconnected, it may fail.<br/>
		*                   Otherwise, success.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*/
		public static void disconnect(ProviderType providerType, onAuthV4Disconnect listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "disconnect", listener);
			jsonParam.AddField("providerType", providerType.ToString());

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		* 현재 사인-인 된 유저에 요청한 provider 와 연동을 해제 한다.
		* <br>Releases the currently signed-in user from the requested provider.
		*
		* <br><h4>korean</h4>
		* 인증 서버에 disconnect 상황을 전달하고 이후 요청한 Provider 를 Logout 시킨다.<br>
		* 인증 서버에 전달이 실패할 경우와 Sign 이 진행중이거나 setup 이 되지 않은 상황,<br>
		* 또는 이미 disconnected 된 Provider 일 경우 실패가 될 수 있다.<br>
		* <br>
		* disconnected 로 인해 연동이 모두 해제될 경우 게스트 상태가 될 수 있으니 주의.<br>
		*
		*
		* <br><h4>english</h4>
		* It pass the disconnect status to the authentication server and then logout the requested provider.<br>
		* If delivery fails to the authentication server, if the Sign or setup is in progress, <br>
		* or if the provider is already disconnected, it may fail.<br>
		* <br>
		* Note that if all account link are disconnected due to 'disconnect', it may become a guest state.<br>
		*
		* @param providerName  disconnect to ProviderName.
		* @param listener      onAuthV4Disconnect
		* result : If delivery fails to the authentication server, if the Sign or setup is in progress, <br>
		* or if the provider is already disconnected, it may fail.<br>
		* Otherwise, success.<br>
		*/
		public static void disconnectWithName(String providerName, onAuthV4Disconnect listener)
		{

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "disconnectWithName", listener);
			jsonParam.AddField("providerName", providerName);

			HIVEUnityPlugin.callNative(jsonParam);
		}

		/**
		*  \~korean
		* @brief connect 로 인한 계정 충돌 상황 (CONFLICT_PLAYER) 일 때 유저를 선택 한다.
		* conncet 로 인해 계정이 충돌 된 상황 (CONFLICT_PLAYER) 에서 자체 UI 를 구성하였을 경우<br/>
		* 선택한 유저를 SDK에 통보해 주기 위해 호출 한다.<br/>
		* SDK 상태가 충돌 상태가 아니거나 signIn 이 진행중, 혹은 잘못된 playerId 일 경우 INVALID_PARAM 이 발생 할 수 있다.<br/>
		* 충돌 상태가 되었다고 해도 selectConflict() 가 호출 되어 signIn 되기 전까지는 기존 유저가 signIn 된 상태로 본다.<br/>
		* <br/>
		* showConflictSelection() 을 사용하였을 경우 호출할 필요가 없다.<br/>
		*
		* @param selectedPlayerId 선택한 유저의 playerId
		*
		* @param listener onAuthV4SignIn<br/>
		*                 SUCCESS 가 되기 전까지는 기존 유저가 signIn 된 상태이다. 주의.<br/>
		*
		*  \~english
		* @brief When an account conflict (CONFLICT_PLAYER) occurs due to connect, it notify the selected user to the SDK.
		* If you use your own customized UI for an account conflict situation (CONFLICT_PLAYER), <br/>
		* you need to call it to notify the selected user when user select one.<br/>
		* INVALID_PARAM can occur if the SDK state is not in a conflict state, signIn is in progress, or the wrong playerId.<br/>
		* Even if a conflict occurs, the existing user is considered to be signIn until  selectConflict () is called then sign in.<br/>
		* <br/>
		* You do not need to call it if you use showConflictSelection().<br/>
		*
		* @param selectedPlayerId PlayerId of the selected user
		*
		* @param listener onAuthV4SignIn<br/>
		*                 Note: The existing user is a signined user, until SUCCESS. <br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void selectConflict(Int64 selectedPlayerId, onAuthV4SignIn listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "selectConflict", listener);
			jsonParam.AddField("selectedPlayerId", selectedPlayerId);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		*  \~korean
		* @brief signIn(AUTO) 가 가능한지 여부를 반환한다.
		* AuthV4.setup() 호출 시 알려주는 값과 동일 하며,<br/>
		* 로컬에 저장된 기존 세션만 체크 하기때문에 세션이 실제로 만료되지 않은 유효한 토큰인지는 알 수 없다.<br/>
		* true 일 경우 'TOUCH TO START' 등의 UI 를 통해 signIn(AUTO) 를 호출하고<br/>
		* 여기서 실패할 경우 다시 초기 사인-인 씬으로 돌아가야 한다.<br/>
		* <br/>
		* false 일 경우 AuthV4.setup() 호출 시 받은 providerList 를 통해 UI 를 구성하여 signIn 을 시도 하거나<br/>
		* showSignIn() 을 통해 HIVE 에서 제공하는 UI 를 사용하여도 된다.<br/>
		* <br/>
		* 앱 중간에 signOut 등 사인-인 씬으로 돌아왔을 경우 이 API 를 통해 UI 구성을 여부를 체크할 수 있다.<br/>
		*
		* @return boolean signIn(AUTO) 가 가능한지 여부
		*
		*  \~english
		* @brief It returns whether signIn (AUTO) is enabled.
		* It is the same value that you would get when you called setup(),<br/>
		* and it only checks for existing sessions that are stored locally, so you do not know if the session token is a valid token that the session has not been expired.<br/>
		* If true, then signIn (AUTO) should be called through UI such as 'TOUCH TO START', <br/>
		* and if signIn(AUTO) fails, it should return to the initial sign-in screen again.<br/>
		* <br/>
		* If it is false, you can customize the UI through the providerList provided at setup() call<br/>
		* or use the UI provided by HIVE via showSignIn()<br/>
		* <br/>
		* If you return to the sign-in scene such as signOut in the middle of your app, you can check whether you need to configure the UI through this API.<br/>
		*
		* @return boolean Whether signIn (AUTO) is enabled.
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static Boolean isAutoSignIn() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "isAutoSignIn", null);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

			Boolean isAutoSignIn = false;
			resJsonObject.GetField (ref isAutoSignIn, "isAutoSignIn");
			return isAutoSignIn;
		}


		/**
		*  \~korean
		* @brief 현재 사인-인 된 유저의 정보를 반환 한다.
		* PlayerInfo 에는 playerToken 을 포함하고 있으며<br/>
		* 프로필 정보 (playerName, playerImageUrl) 까지 포함하고 있으나,<br/>
		* 요청시 실시간으로 서버에 요청하지 않고 로컬에 캐쉬되어 있는 프로필 정보를 반환 한다.<br/>
		* <br/>
		* getProfile 이나 showProfile 에서 다인-인 된 유저 자신일 경우 자동으로 캐쉬를 갱신한다..<br/>
		*
		* @return PlayerInfo 현재 사인-인 된 유저의 정보. 사인-인 되어있지 않을 경우 null.
		*
		*  \~english
		* @brief It returns information about the currently signed-in user.
		* PlayerInfo includes playerToken and profile information (playerName, playerImageUrl),<br/>
		* but it returns the locally cached profile information instead of requesting it to the server in real time upon request.<br/>
		* <br/>
		* When getProfile or showProfile is called, it is automatically updated profile information if a signed-in user is it self.<br/>
		*
		* @return PlayerInfo Information about the currently signed-in user. Null if not signed-in.
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static PlayerInfo getPlayerInfo() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "getPlayerInfo", null);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

			JSONObject playerInfoJson = resJsonObject.GetField("getPlayerInfo");


			if (playerInfoJson == null || playerInfoJson.ToString().Equals(JSONObject.emptyObject.ToString()))
			{
				return null;
			}
			else
			{
				return new PlayerInfo(playerInfoJson);
			}
		}

		/**
		*  \~korean
		* @brief 요청한 playerId 들의 프로필 정보를 반환한다.
		* plauerId 중 사인-인 된 자신이 있을 경우 playerName 과 playerImageUrl 을 동기화 한다.<br/>
		*
		* @param playerIdList ArrayList<Long>
		*
		* @param listener onAuthV4GetProfile
		*
		*  \~english
		* @brief it returns the profile information of the requested playerIds.
		* It update playerName and playerImageUrl when the playerId list contains the signed-in oneself.<br/>
		*
		* @param playerIdList ArrayList<Long>
		*
		* @param listener onAuthV4GetProfile
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void getProfile(List<Int64> playerIdList, onAuthV4GetProfile listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "getProfile", listener);

			JSONObject jsonArray = new JSONObject();

			if(playerIdList != null) {
				foreach(Int64 playerId in playerIdList) {

					jsonArray.Add(playerId.ToString());
				}
			}
			
			jsonParam.AddField ("playerIdList", jsonArray);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		*  \~korean
		* @brief 사인-인 을 할 수 있는 Provider 목록이 있는 UI 를 띄운다.
		* setup 에서 받은 ProviderList 와 동일한 목록을 가진 사인-인 가능한 UI 를 띄운다.<br/>
		* 네트워크 지연 등으로 인해 GUEST 버튼이 노출되지 않을 수 있다.<br/>
		* AUTO 는 지원하지 않는다. isAutoSignIn() 이 true 라면 바로 signIn(AUTO) 를 호출 해 주면 된다.<br/>
		*
		* @param listener onAuthV4SignIn
		*
		*  \~english
		* @brief It shows the UI with a list of providers that can sign-in.
		* It will show a UI with the same list of providers as the ProviderList you received from setup() call.<br/>
		* A GUEST button may not be exposed due to network delays.<br/>
		* AUTO is not supported. If isAutoSignIn () is true, call signIn(AUTO) immediately.<br/>
		*
		* @param listener onAuthV4SignIn
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void showSignIn(onAuthV4SignIn listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "showSignIn", listener);

			HIVEUnityPlugin.callNative (jsonParam);
			HIVEUnityPlugin.IMECompositionModeOn();
		}
		

		/**
		*  \~korean
		* @brief 충돌상태에서 유저를 선택하는 UI 를 띄운다.
		* connect 시 CONFLICT_PLAYER 에러가 발생한 경우에 호출 하여 유저 선택이 가능한 UI 를 띄운다.<br/>
		* 충돌난 playerId 에 해당하는 유저가 인식할 수 있는 정보 (닉네임, 레벨 등) 들을 포함하여 호출 하여야 한다.<br/>
		* playerData 에는 "player_id" 를 키로 playerId 를 넣어주어야 한다.<br/>
		* 그 외의 정보는 "game_data" 를 키로 Map<String, Object> 형태 값을 넣어 주면 된다.<br/>
		* "player_id" 와 "game_data" 의 키는 필수이며 변경하면 안된다.<br/>
		* <br/>
		* ex) {"player_id":123, "game_data":{"Name":"CurrentPlayer", "Level":52}}<br/>
		* <br/>
		* 충돌난 유저로 선택 된 경우 다시 게임데이터를 불러와야 한다.<br/>
		*
		* @param currentPlayerData 현재 사인-인 되어있는 유저의 정보
		*
		* @param conflictPlayerData connect 시 받은 conflictPlayer 정보
		*
		* @param listener onAuthV4SignIn
		*
		*  \~english
		* @brief It shows UI to select user in conflict state.
		* If a CONFLICT_PLAYER error occurs during connect() call, it shows the UI to display a user-selectable UI.<br/>
		* It should be called with information (nickname, level, etc.) that identifies the user corresponding to the conflicted playerId.<br/>
		* You should put the playerId in the "player_id" key in playerData.<br/>
		* For other information, add a value of type Map <String, Object> to the "game_data" key.<br/>
		* The keys name, "player_id" and "game_data" are required and should not be changed.<br/>
		* <br/>
		* ex) {"player_id":123, "game_data":{"Name":"CurrentPlayer", "Level":52}}<br/>
		* <br/>
		* If a conflicted user is selected, the game data corresponding to the user must be reloaded.<br/>
		*
		* @param currentPlayerData Information of the user who is currently signed in.
		*
		* @param conflictPlayerData ConflictPlayer information received at connect() call.
		*
		* @param listener onAuthV4SignIn
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void showConflictSelection(JSONObject currentPlayerData, JSONObject conflictPlayerData, onAuthV4SignIn listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "showConflictSelection", listener);
			jsonParam.AddField ("currentPlayerData", currentPlayerData);
			jsonParam.AddField ("conflictPlayerData", conflictPlayerData);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		*  \~korean
		* @brief 요청한 playerId 의 프로필 UI 를 표시한다.
		* playerId 가 사인-인 된 자신이며 HIVE 와 연동된 유저일 경우 이 UI 에서 프로필 사진과 닉네임을 변경할 수 있다.<br/>
		* 이 경우 창이 닫히면 playerName 과 playerImageUrl 를 로컬 정보와 동기화 한다.<br/>
		*
		* @param playerId 프로필 UI 를 띄울 playerId
		*
		* @param listener onAuthV4ShowProfile
		*
		*  \~english
		* @brief It display the profile UI of the requested playerId.
		* If the playerId is a sign-in user's one and the user is a member of HIVE Membership, the user can change the profile picture and nickname in this UI.<br/>
		* In this case, when the window is closed, the playerName and playerImageUrl are synchronized with the local information.<br/>
		*
		* @param playerId PlayerId to show the profile UI
		*
		* @param listener onAuthV4ShowProfile
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void showProfile(Int64 playerId, onAuthV4ShowProfile listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "showProfile", listener);
			jsonParam.AddField ("playerId", playerId);

			HIVEUnityPlugin.callNative (jsonParam);
			HIVEUnityPlugin.IMECompositionModeOn();
		}


		/**
		*  \~korean
		* @brief 1:1 문의 UI 를 띄운다.
		* HIVE Membership 연동 여부와 상관없이 사용할 수 있으며<br/>
		* 연동되어 있다면 e-mail 부분이 자동으로 기입되어 있다.<br/>
		*
		* @param listener onAuthV4ShowInquiry
		*
		*  \~english
		* @brief It shows the 1: 1 query UI.
		* It can be used regardless of whether HIVE Membership is linked or not,<br/>
		* If it is linked, the e-mail part is automatically filled in.<br/>
		*
		* @param listener onAuthV4ShowInquiry
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void showInquiry(onAuthV4ShowInquiry listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "showInquiry", listener);

			HIVEUnityPlugin.callNative (jsonParam);
			HIVEUnityPlugin.IMECompositionModeOn();
		}


		/**
		*  \~korean
		* @brief 내 문의 UI 를 띄운다.
		*
		* @param listener onAuthV4ShowInquiry
		*
		*  \~english
		* @brief It shows the my inquiry UI.
		*
		* @param listener onAuthV4ShowInquiry
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void showMyInquiry(onAuthV4ShowMyInquiry listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "showMyInquiry", listener);

			HIVEUnityPlugin.callNative (jsonParam);
			HIVEUnityPlugin.IMECompositionModeOn();
		}


		/**
		* \~korean HIVE 챗봇 1:1 문의 화면 호출<br/>
		*
		* @param additionalInfo
		*            챗봇 페이지 바로가기 호출시 전달받기로한 약속된 JSON 형식의 String 데이터
		* @param handler
		*            API 호출 결과 통지
		* \~english Show HIVE Chatbot 1:1 inquiry <br/>
		*
		* @param additionalInfo
		*            Promised String data (JSON format) when you call chatbot page open API
		* @param handler
		*            API call result handler
		* \~
		* @ingroup AuthV4
		*/
		public static void showChatbotInquiry(String additionalInfo, onAuthV4ShowChatbotInquiry listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "showChatbotInquiry", listener);
			jsonParam.AddField("additionalInfo", additionalInfo);

			HIVEUnityPlugin.callNative (jsonParam);
			HIVEUnityPlugin.IMECompositionModeOn();
		}

		/**
		*  \~korean
		* @brief HIVE 약관 정보를 표시한다.
		* 약관 정보를 표시한다.<br/>
		* SDK 초기화 시 사용자에게 약관 동의 절차를 거치게 된다.<br/>
		* 이후 게임에서는 설정 창 등에서 개인 정보 처리 방침 및 이용 약관 정보를 확인할 수 있는 웹뷰를 노출하도록 구성해야 한다.<br/>
		*
		* @param listener onAuthV4ShowTerms<br/>
		*                 API 호출 결과 통지
		*
		*  \~english
		* @brief  It shows HIVE Terms and Conditions.
		* It shows Terms and Conditions<br/>
		* At initialization of the SDK, the user will go through the agreement process.<br/>
		* After this, the game should be configured to expose the WebView os that user can check Privacy policy and Terms and Conditions in the game setting menu.<br/>
		*
		* @param listener onAuthV4ShowTerms<br/>
		*                 API callback
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void showTerms(onAuthV4ShowTerms listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "showTerms", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		*  \~korean
		* @brief 약관 동의 기록을 초기화한다.
		*
		*  \~english
		* @brief Initialize the terms agreement record.
		* 
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void resetAgreement()
		{
			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "resetAgreement", null);

			HIVEUnityPlugin.callNative (jsonParam);
		}


	    /**
	    *  \~korean
	    * @brief 성인 인증을 요청한다.
		* 고스톱 / 포커류와 같이 성인 인증이 필요한 일부 게임의 경우 성인 인증 기능을 제공한다.
		*
		* @param listener onAuthV4AdultConfirm<br/>
		*                 result : 사인-인 이 필요하기 때문에 사인-인 이 되어있지 않을 경우 INVALID_SESSION 이 올 수 있다.<br/>
		*                   성인인증에 정상적으로 진행되었을 경우 SUCCESS 가 된다.<br/>
		*
		*  \~english
		* @brief It request adult authentication.
		* For some games that require adult authentication, it provide adult authentication.
		*
		* @param listener onAuthV4AdultConfirm<br/>
		*                 result : INVALID_SESSION can be given if it is not signed-in because it requires a sign-in.<br/>
		*                   SUCCESS will be made if the adult is successfully certified.<br/>
		*
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void showAdultConfirm(onAuthV4AdultConfirm listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "showAdultConfirm", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		*  \~korean
		* @brief 요청한 Provider 의 상태를 체크 한다.
		* 요청한 Provider 로 Login 이 되어있지 않으면 Login 까지 시도한다.<br/>
		* Login 에 성공하면 providerUserId 까지 얻어 온다.<br/>
		* <br/>
		* 현재 playerId 와 connect 를 요청하지는 않는다.<br/>
		* <br/>
		* SUCCESS 는 조회에 성공했다는 뜻이며 providerInfo 에 providerUserId 여부로 로그인된 유저를 판단해야 한다.<br/>
		* 실제 provider 에 로그인 되어있다면 providerUserId 가 존재한다. 그 외 providerType 만 존재.<br/>
		* <br/>
		* Google Play Games, Apple Game Center 등 묵시적 사인-인 방식을 사용할 경우
		*
		* @param providerType 상태를 체크할 ProviderType
		*
		*  \~english
		* @brief It check the status of the requested Provider.
		* If the requested provider is not logged in, it tries to login.<br/>
		* If the login succeeds, it also brings providerUserId.<br/>
		* <br/>
		* It does not request connect with the current playerId.<br/>
		* <br/>
		* SUCCESS means that the query was successful and should determine whether or not the user is logged in with the presence of providerUserId in providerInfo.<br/>
		* If you are logged in to the actual provider, there is a providerUserId. Otherwise, only providerType exists.<br/>
		* <br/>
		* When using the implicit sign-in : Google Play Games, Apple Game Center etc.
		*
		* @param providerType ProviderType to check status
		*
		*  \~
		* @param listener onDeviceProviderInfo
		*
		* @ingroup AuthV4
		*
		*/
		public static void checkProvider(ProviderType providerType, onDeviceProviderInfo listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "checkProvider", listener);
			jsonParam.AddField("providerType", providerType.ToString());

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		*  \~korean
		* @brief 점검 상태 표시 및 데이터 반환한다.
		* Configuration 에 설정한 ServerId 로 점검상태 여부를 체크한다.<br/>
		* <br/>
		* SDK 의 초기화가 완료되면 서버 점검 및 업데이트 상태를 확인해야 한다.<br/>
		* 서버 점검 및 업데이트는 게임 클라이언트의 업데이트 후에 하위 버전을 차단하거나, 게임 서버의 점검 시간 동안 게임 접속을 차단할 수 있다.<br/>
		* HIVE는 백오피스에 설정된 정보에 따라 서버 점검 혹은 공지 팝업을 노출하는 기능을 제공한다.<br/>
		* <br/>
		* 요청시 isShow 가 true 였을 경우 점검 팝업 UI 가 노출된다.<br/>
		* <br/>
		* 결과가 SUCCESS 이지만 점검 공지 내용이 없다면 listener 의 maintenanceInfo 는 비어있게 된다.<br/>
		* 요청시 isShow 를 false 로 주었다면 listener 의 maintenanceInfo 정보를 가지고 직접 UI 를 구성하여 노출하여야 한다.<br/>
		*
		* @param isShow HIVE UI 를 사용할 지 여부
		* 
		*  \~english
		* @brief It display maintenance status and return its data.
		* It checks whether the server is in maintenance with the ServerId set in Configuration.<br/>
		* <br/>
		* Once the SDK is initialized, you should check the status of the server maintenance and update.<br/>
		* Server maintenance and Update can block lower version after update of game client, or block game connection during maintenance time of game server.<br/>
		* HIVE provides a function to expose server maintenance or update pop-up according to the information set in the back office.<br/>
		* <br/>
		* On request, a maintenance popup UI is exposed when isShow is true.<br/>
		* <br/>
		* If the result is SUCCESS but there is no maintenance notification, the maintenanceInfo of the listener will be empty.<br/>
		* If isShow is set to false on request, the UI should be configured and exposed by game developer with the maintenanceInfo information of the listener.<br/>
		*
		*  \~
		* @param listener onAuthV4Maintenance
		*
		* @ingroup AuthV4
		*
		*/
		public static void checkMaintenance(Boolean isShow, onAuthV4Maintenance listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "checkMaintenance", listener);
			jsonParam.AddField ("isShow", isShow);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		*  \~korean
		* @brief 현재 유저의 제재 상태를 체크한다.
		* 기본적으로 signIn, connect 시에 자동으로 체크되며 제재 팝압을 띄우나 <br/>
		* 앱 내에서 필요한 시점에 실시간으로 체크가 필요할 때 사용할 수 있다.<br/>
		* <br/>
		* 요청시 isShow 가 true 였을 경우 팝업 UI 가 노출된다.<br/>
		* <br/>
		* 결과가 SUCCESS 이지만 제재 내용이 없다면 listener 의 maintenanceInfo 는 비어있게 된다.<br/>
		* 요청시 isShow 를 false 로 주었다면 listener 의 maintenanceInfo 정보를 가지고 직접 UI 를 구성하여 노출하여야 한다.<br/>
		*
		* @param isShow HIVE UI 를 사용할 지 여부
		*
		*  \~english
		* @brief it check the restriction status of the current user.
		* It is automatically checked when signIn or connect is called and shows a restriction popup,<br/>
		*  but you can use it when you need to check in realtime in the app.<br/>
		* <br/>
		* On request, the UI is exposed when isShow is true<br/>
		* <br/>
		* If the result is SUCCESS but there is no restriction notification, the maintenanceInfo of the listener will be empty.<br/>
		* If isShow is set to false on request, the UI should be configured and exposed by game developer with the maintenanceInfo information of the listener.<br/>
		*
		* @param isShow Whether to use the HIVE UI
		*
		*  \~
		* @param listener onAuthV4Maintenance
		*
		* @ingroup AuthV4
		*
		*/
		public static void checkBlacklist(Boolean isShow, onAuthV4Maintenance listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "checkBlacklist", listener);
			jsonParam.AddField ("isShow", isShow);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		*  \~korean
		* @brief 연동된 Provider 의 정보가 바뀌었는지 통보 받는다.
		* Google Play Games, Apple Game Center 의 경우 앱 외에서 계정 정보가 변경 될 수 있다.<br/>
		* 그렇기 때문에 앱이 resume (onStart) 될 때 유저 정보가 바뀌었는지 체크하게 되고,<br/>
		* 바뀌었다면 해당 콜백이 불리게 된다.<br/>
		* <br/>
		* 콜백은 checkProvider() 이후나 connect 등으로 직접 연동 시도 된 이후부터 동작하게 된다.<br/>
		* <br/>
		* 묵시적 사인-인 방식으로 구현할 경우 signIn() 완료 시점에서 checkProvider() 를 통해 직접 비교 체크 하여야 한다.<br/>
		*
		*  \~english
		* @brief It set a listener to be notified whether the information of the linked Provider has changed.
		* For Google Play Games and Apple Game Center, account information may be changed outside of the app.<br/>
		* That's why it checks to see if the user information has changed when the app resumes (onStart),<br/>
		* and if so, the callback is called.<br/>
		* <br/>
		* Callbacks will work after checkProvider() or attempting to connect directly with connect().<br/>
		* <br/>
		* If you implement implicit sign-in, you need to check directly with checkProvider () at the completion of signIn().<br/>
		*
		*  \~
		* @param listener onDeviceProviderInfo
		*
		* @ingroup AuthV4
		*
		*/
		public static void setProviderChangedListener(onDeviceProviderInfo listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "setProviderChangedListener", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		
		/**
		 *  \~korean
		 * @brief COPPA 13세 나이 제한 여부를 반환한다.<br/>
		 * 기본값은 false이며, AuthV4.setup() 호출 시 COPPA 약관 동의에 따라 결정된다.<br/>
		 * 정상적인 값을 받으려면 AuthV4.setup() 이후에 호출하여야 한다.<br/>
		 *
		 * @return boolean COPPA 13세 나이 제한 여부
		 *
		 *  /~english
		 *  @brief Return the value whether COPPA limits users under 13 or not.<br/>
		 *  Default value is false, and it depends on the agreement on COPPA when calling AuthV4.setup() API.<br/>
		 *  Normal process is calling this API after AuthV4.setup() API is completed.<br/>
		 *
		 * @return boolean the value whether COPPA limits users under 13 or not
		 *  \~
		 * @ingroup AuthV4
		 *
		 */
		public static Boolean getAgeGateU13() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "getAgeGateU13", null);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

			Boolean ageGateU13 = false;
			resJsonObject.GetField (ref ageGateU13, "ageGateU13");
			
			return ageGateU13;
		}


		/**
		*  \~korean
		* @brief 인증 정보를 포함하여 SDK 에서 사용하는 모든 데이터 초기화 한다.
		* HIVE SDK 연동 및 테스트시에 사용된다.
		*
		*  \~english
		* @brief It initialize all data used by the SDK, including authentication information.
		* It is used for HIVE SDK implementation and testing.
		* 
		*  \~
		* @ingroup AuthV4
		*
		*/
		public static void reset() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "reset", null);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		*  \~korean
		* @brief 입력받은 providerType 에 해당하는 연결된 사용자의 친구들의 playerId 목록 정보를 얻어온다.
		* 요청한 providerType의 providerId와 연결된 사용자의 playerId를 페어로 얻어온다.<br/>
		* kHIVEProviderTypeFACEBOOK 의 경우 최대 5000명.<br/>
		* <br/>
		* getProfile 을 사용하여 친구의 PlayerInfo 를 조회할 수 있다.<br/>
		*
		* @param providerType	친구목록을 조회할 연결된 ProviderType 값.
		* 
		*  \~english
		* @brief It returns the playerId list information of the friends of the connected user corresponding to the input providerType..
		* It comes with a pair of information about the providerId of the requested providerType and the playerId of the associated user.<br/>
		* Up to 5000 people for kHIVEProviderTypeFACEBOOK.<br/>
		* <br/>
		* You can query your friend's PlayerInfo using getProfile.<br/>
		*
		* @param providerType	Connected ProviderType value to query friends list.
		*  \~
		* @param listener		AuthV4ProviderFriendsListener
		*
		* @ingroup AuthV4
		*
		*/
		public static void getProviderFriendsList(ProviderType providerType, onGetProviderFriendsList listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "getProviderFriendsList", listener);
			jsonParam.AddField("providerType", providerType.ToString());

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		*  \~korean
		* @brief conncet 로 인해 계정이 충돌 된 상황 (CONFLICT_PLAYER) 을 해제 한다.
		* conncet 로 인해 계정이 충돌 된 상황 (CONFLICT_PLAYER) 에서 자체 UI 를 구성하였을 경우에 사용<br/>
		* 충돌 된 상황 (CONFLICT_PLAYER) 을 해제 한다.<br/>
		* 호출시 해당하는 IDP (Provider) 를 로그아웃하게 된다.<br/>
		* 앱 내에서 로그아웃 할 수 없는 IDP (Apple GameCenter)의 경우 로그아웃은 불가능하고,<br/>
		* 내부 충돌 정보만 삭제 된다.
		*
		* @param listener AuthV4ResolveConflictListener<br/>
		*                 실제 IDP 로그아웃에 실패했어도 내부 충돌 정보는 삭제 된다.
		*
		*  \~english
		* @brief It resolve the account conflicts(CONFLICT_PLAYER) caused by connect() call.
		* This is used when the customized UI is used in the account conflict state caused by connect() call.<br/>
		* It resolve the account conflicts(CONFLICT_PLAYER).<br/>
		* Upon calling, the corresponding IdP (Provider) is logged out.<br/>
		* For an Apple GameCenter (IDP) that can not log out of the app, you can not sign out,<br/>
		* and only delete internal conflict information.
		*
		* @param listener AuthV4ResolveConflictListener<br/>
		*                 Even if the actual IdP logout fails, the internal conflict information is deleted.
		*
		*  \~
		* @see AuthV4.AuthV4ResolveConflictListener
		*
		* @ingroup AuthV4
		*
		*/
		public static void resolveConflict(onAuthV4ResolveConflict listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "resolveConflict", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		*  \~korean
		* @brief 기기 관리 서비스를 호출한다.
		* 서비스를 이용하지 않는 유저는 가입 안내가 표시되고<br/>
		* 서비스를 이용하는 유저는 기기 관리 목록이 표시된다.<br/>
		*
		* @param listener AuthV4ShowDeviceManagementListener<br/>
		*
		*  \~english
		* @brief Run the device management service.
		* Users who do not use the service, a service subscription guide is displayed.<br/>
		* A user who uses the service is displayed with a device management list.<br/>
		*
		* @param listener AuthV4ShowDeviceManagementListener<br/>
		*
		*  \~
		* @see AuthV4.AuthV4ShowDeviceManagementListener
		*
		* @ingroup AuthV4
		*/
		public static void showDeviceManagement(onAuthV4ShowDeviceManagement listener) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "showDeviceManagement", listener);
			HIVEUnityPlugin.callNative (jsonParam);
			HIVEUnityPlugin.IMECompositionModeOn();
		}

		/**
         *  \~korean
         * @brief {@link #getHiveTalkPlusLoginToken(AuthV4GetHiveTalkPlusLoginTokenListener)} 호출 결과 통지
         * AuthV4.getHiveTalkPlusLoginToken() 에서 사용<br/>
         * HiveTalkPlus 로그인시 필요한 로그인 토큰을 얻어온다.
         *
         *  \~english
         * @brief {@link #getHiveTalkPlusLoginToken(AuthV4GetHiveTalkPlusLoginTokenListener)} call
         * AuthV4.getHiveTalkPlusLoginToken() use it.<br/>
         * Get the required login token when logging in to HiveTalkPlus.
         *
         *  \~
         * @see #getHiveTalkPlusLoginToken(AuthV4GetHiveTalkPlusLoginTokenListener)
         * @ingroup AuthV4
         */
		 public static void getHiveTalkPlusLoginToken(onAuthV4GetHiveTalkPlusLoginToken listener) {
			 JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "getHiveTalkPlusLoginToken", listener);
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
		* @ingroup AuthV4
		*/
		public static void requestPermissionViewData(onAuthV4RequestPermissionViewData listener) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "requestPermissionViewData", listener);
			HIVEUnityPlugin.callNative (jsonParam);
		}
		/**
		*  \~korean
		* @brief Game Center 로그인창을 표시할 수 없는 경우, 해당 상태를 보여주고,<br/>
		* Game Center 로그인 방법을 안내하는 UI를 띄운다.
		*
		*  \~english
		* @brief If a sign-in window of Game Center is not displayed, show its status<br/>
		* and display a UI to inform how to sign in the Game Center.
		*  \~
		*
		* @ingroup AuthV4Helper
		*/
		public static void showGameCenterLoginCancelDialog(onAuthV4DialogDismiss listener) {
		    JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "showGameCenterLoginCancelDialog", listener);
		    HIVEUnityPlugin.callNative (jsonParam);
		}
		/**
		* @file	AuthV4.cs
		*
		*
		*  @date		2018-2022
		*  @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
		*  @author	    imsunghoon
		*  @since		4.7.0
		*
		*  @defgroup	AuthV4Helper
		*  @{
		*  \~korean
		* @brief HIVE 인증의 고도화 된 기능을 제공한다<br>
		* <br>
		* 이 클래스에서 제공하는 상세 기능은 다음과 같다. <br>
		*  - 사인-인/아웃<br>
		*  - Provider 연결/연결 해제<br>
		*  - 업적/리더보드 조회<br>
		*  - 계정 충돌 상태 조회/해결<br>
		*
		*  \~english
		* @brief Provides advanced features of HIVE Authentication.<br>
		* <br>
		* The detailed functions provided by this class are as follows. <br>
		*  - Sign-in/Sign-out<br>
		*  - Connect/Disconnect Providers<br>
		*  - Query Achievement/Leaderboard<br>
		*  - Query/Resolve account conflicts state<br>
		*  \~
		*/
		public static class Helper {

			/**
			*  \~korean
			* @brief 계정 충돌시 충돌 유저의 게임 정보를 같이 보여주기 위한 클래스<br>
			*
			*  \~english
			* @brief The class showing the conflicted account's game data when user accounts conflict<br>
			*  \~
			*/
			public class ConflictSingleViewInfo {
				private Dictionary<String, String> playerData = new Dictionary<String, String>();
				private Int64 playerId;

				public ConflictSingleViewInfo(Int64 playerId) {
					this.playerId = playerId;
				}

				public void setValue(String key, double value) {
					String valueStr = value.ToString();
					this.setValue(key, valueStr);
				}

				public void setValue(String key, String value) {
					if (playerData.ContainsKey (key)) {
						playerData.Remove(key);
					}
					playerData.Add(key, value);
				}

				public void setValue(String key, int value) {
					String valueStr = value.ToString();
					this.setValue(key, valueStr);
				}

				public JSONObject ToJSONObject() {
					JSONObject jsonParam = new JSONObject ();
					jsonParam.AddField ("player_id", this.playerId);

					JSONObject infoJsonParam  = new JSONObject();

					var enumerator = this.playerData.GetEnumerator();
					
					while(enumerator.MoveNext()) {
						infoJsonParam.AddField(enumerator.Current.Key, enumerator.Current.Value);
					}

					jsonParam.AddField("game_data", infoJsonParam);
					
					return jsonParam;
				}
			}

			/**
			*  \~korean
			* @brief AuthV4Helper API 요청 결과 통지
			* @param result result에 올 수 있는 ErrorCode와 Code는 아래와 같다.<br>
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
				</tr>

				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>계정 충돌이 없으며 요청한 작업이 정상적으로 수행됨</td>
				</tr>
				<tr>
					<td>NEED_INITIALIZE</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK가 초기화되지 않은 경우</td>
				</tr>
				<tr>
					<td>INVALID_SESSION</td>
					<td>AuthV4SigninFirst</td>
					<td>로그인 상태에서 사용 가능한 API를 로그인하지 않은 상태에서 호출한 경우</td>
				</tr>
				<tr>
					<td>INVALID_PARAM</td>
					<td>AuthV4AlreadyAuthorized</td>
					<td>로그인 상태에서 다시 로그인 API를 호출한 경우</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>Device에 로그인 된 계정과 현재 로그인 된 계정의 PGS/GameCenter 정보가 다르거나</br>
					Connect를 시도한 Provider의 Player ID가 이미 있는 경우
					</td>
				</tr>
				<tr>
					<td>PLAYER_CHANGE</td>
					<td>AuthV4PlayerChange</td>
					<td>계정 충돌이 발생한 후 계정 전환에 성공한 경우</td>
				</tr>
				<tr>
					<td>CANCELED</td>
					<td>AuthV4PlayerResolved</td>
					<td>계정 충돌이 발생한 후 현재 로그인 된 계정을 유지하는 경우</td>
				</tr>
				<tr>
					<td>INVALID_SESSION</td>
					<td>AuthV4HelperImplifiedLoginFail</td>
					<td>묵시적 로그인에 실패하여 명시적 로그인을 진행해야 하는 경우</td>
				</tr>
				<tr>
					<td>IN_PROGRESS</td>
					<td>AuthV4InProgressSignIn<br>
					AuthV4InProgressConnect<br>
					AuthV4InProgressShowLeaderboard<br>
					AuthV4InProgressShowAchievements</td>
					<td>요청한 작업이 아직 처리중인 경우</td>
				</tr>
			</table>
			*
			* @param playerInfo result에 따라 올 수 있는 사용자의 정보는 아래와 같다.<br>
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Player Info</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>현재 로그인 된 유저의 정보</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>충돌이 발생한 유저의 정보</td>
				</tr>
				<tr>
					<td>PLAYER_CHANGE</td>
					<td>AuthV4PlayerChange</td>
					<td>계정 전환에 성공한 유저의 정보</td>
				</tr>
				<tr>
					<td>CANCELED</td>
					<td>AuthV4PlayerResolved</td>
					<td>현재 유저의 정보</td>
				</tr>
			</table>
			*
			*  \~english
			* @brief AuthV4Helper API request result callback
			* @param result ErrorCodes and Codes available for result values are as follows.<br>
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
				</tr>

				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Complted the request without account conflicts</td>
				</tr>
				<tr>
					<td>NEED_INITIALIZE</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK not initialized</td>
				</tr>
				<tr>
					<td>INVALID_SESSION</td>
					<td>AuthV4SigninFirst</td>
					<td>Called the APIs required sign-in before sign-in</td>
				</tr>
				<tr>
					<td>INVALID_PARAM</td>
					<td>AuthV4AlreadyAuthorized</td>
					<td>Called sign-in API again after sign-in</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>The account signed in on user device is mismatched with the PGS/Game Center account on the game<br>
					or the player ID of the Provider to be connected already existed</td>
				</tr>
				<tr>
					<td>PLAYER_CHANGE</td>
					<td>AuthV4PlayerChange</td>
					<td>Succeeded to change the player after accounts conflict</td>
				</tr>
				<tr>
					<td>CANCELED</td>
					<td>AuthV4PlayerResolved</td>
					<td>Not changed the currently signed-in account after accounts conflict</td>
				</tr>
				<tr>
					<td>INVALID_SESSION</td>
					<td>AuthV4HelperImplifiedLoginFail</td>
					<td>Required Explicit Login due to failure of Implicit Login</td>
				</tr>
				<tr>
					<td>IN_PROGRESS</td>
					<td>AuthV4InProgressSignIn<br>
					AuthV4InProgressConnect<br>
					AuthV4InProgressShowLeaderboard<br>
					AuthV4InProgressShowAchievements</td>
					<td>The request is still in progress</td>
				</tr>
			</table>
			*
			* @param playerInfo Player information depending on results are as follows.<br>
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Player Info</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>The information of signed-in player</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>The information of conflicted player</td>
				</tr>
				<tr>
					<td>PLAYER_CHANGE</td>
					<td>AuthV4PlayerChange</td>
					<td>The information of the player who successfully changed its account</td>
				</tr>
				<tr>
					<td>CANCELED</td>
					<td>AuthV4PlayerResolved</td>
					<td>The information of current player</td>
				</tr>
			</table>
			*  \~
			* @see ResultAPI, PlayerInfo
			*
			* @ingroup AuthV4Helper			
			*/
			public delegate void onAuthV4Helper(ResultAPI result, PlayerInfo playerInfo); 

			//4.7.0_ADDED
			/**
			*  \~korean
			* @brief AuthV4 Game Center 로그인 안내 팝업 결과 통지<br>
			* <br>
			* @param isDismiss : 팝업이 정상적으로 닫힌 경우 true가 전달된다.
			*
			*  \~english
			* @brief AuthV4 Result callback after Game Center login pops up<br>
			* <br>
			* @param isDismiss : Returns true if the popup is successfully closed.
			*  \~
			*/
			public delegate void onAuthV4DialogDismiss(bool isDismiss);

			/**
			*  \~korean
			* @brief 제공된 API 외 경우에 계정의 충돌 여부를 확인할 수 있다.
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>계정 상태 정상</td>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK가 초기화되지 않은 경우</td>
					<td>AuthV4.setup() API로 HIVE SDK 초기화</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>로그인한 계정과 기기에 로그인된 PGS/GameCenter 계정이 다른 경우</td>
					<td>계정 충돌 상황 해결 방법 안내에 따라 해결</td>
				</tr>
				<tr>
					<td>NOT_SUPPORTED</td>
					<td>AuthV4NotSupportedProviderType</td>
					<td>PGS/GameCenter 외 Provider Type으로 계정 상태를 조회한 경우</td>
					<td>-</td>
				</tr>
			</table>
			*
			* @param providerType 동기화를 요청할 ProviderType
			*
			* @param listener AuthV4HelperListener AuthV4Helper Sync Account 결과 통지
			*
			*  \~english
			* @brief Helps to inform whether accounts conflict or not when executing the provided as well as other APIs.
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Account normal state</td>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK not initialized</td>
					<td>Initialize HIVE SDK by implementing AuthV4.setup() API</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>Mismatched the signed-in account with PGS/GameCenter account on the device</td>
					<td>Follow the resolution in accordance with the type of account conflicts</td>
				</tr>
				<tr>
					<td>NOT_SUPPORTED</td>
					<td>AuthV4NotSupportedProviderType</td>
					<td>Queried account state with other provider types except PGS/GameCenter</td>
					<td>-</td>
				</tr>
			</table>
			*
			* @param providerType ProviderType to request syncronization
			*
			* @param listener AuthV4HelperListener AuthV4Helper Sync Account result callback
			*  \~
			*
			* @ingroup AuthV4Helper
			*
			*/
			public static void syncAccount(ProviderType providerType, onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "syncAccount", listener);
				jsonParam.AddField("providerType", providerType.ToString());

				HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief 사용자 로그인
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			* <br>
			* <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>로그인 성공<br>
					<strong>playerInfo</strong>: 로그인 성공한 사용자의 정보</td>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK가 초기화되지 않은 경우</td>
					<td>AuthV4.setup() API로 HIVE SDK 초기화</td>
				</tr>
				<tr>
					<td>IN_PROGRESS</td>
					<td>AuthV4InProgressSignIn<br>
					<td>Sign In 처리 중 재요청이 들어온 경우</td>
					<td>기존 SignIn 요청 결과 대기</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>Device에 로그인 된 계정과 현재 로그인 된 계정의 PGS/GameCenter 정보가 다르거나<br>
					Connect를 시도한 Provider의 Player ID가 이미 있는 경우<br>
					<strong>playerInfo</strong>: 충돌 계정 정보
					</td>
					<td>계정 충돌 상황 해결 방법 안내에 따라 해결</td>
				</tr>
				<tr>
					<td>INVALID_SESSION</td>
					<td>AuthV4HelperImplifiedLoginFail</td>
					<td>묵시적 로그인에 실패하여 명시적 로그인을 진행해야 하는 경우</td>
					<td>AuthV4.Helper.getIDPList()로 지원 Provider 목록 조회 후 명시적 로그인 UI를 구현하거나, <br>
					AuthV4.showSignIn() HIVE UI를 이용하여 명시적 로그인 동작 수행</td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Sign In 결과 통지
			*
			*  \~english
			* @brief Player sign-in
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			* <br>
			* <br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Succeeded to sign in<br>
					<strong>playerInfo</strong>: The information of signed-in player</td>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK not initialized</td>
					<td>Initialize HIVE SDK by implementing AuthV4.setup() API</td>
				</tr>
				<tr>
					<td>IN_PROGRESS</td>
					<td>AuthV4InProgressSignIn<br>
					<td>Requested to process SignIn while sign-in is in progress</td>
					<td>Wait for the SignIn request in progress</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>The account signed in on user device is mismatched with the PGS/Game Center account on the game<br>
					or the player ID of the Provider to be connected already existed<br>
					<strong>playerInfo</strong>: The information of conflicted account
					</td>
					<td>Follow the resolution in accordance with the type of account conflicts</td>
				</tr>
				<tr>
					<td>INVALID_SESSION</td>
					<td>AuthV4HelperImplifiedLoginFail</td>
					<td>Required Explicit Login due to failure of Implicit Login</td>
					<td>Query the supported Provider lists by calling AuthV4.Helper.getIDPList() and implement Explicit Login UI, <br>
					or implement Explicit Login by calling AuthV4.showSignIn() of HIVE UI</td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Sign In result callback			
			*  \~
			*
			* @ingroup AuthV4Helper
			*
			*/
			public static void signIn(onAuthV4Helper listener) {
				
				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "signIn", listener);

				HIVEUnityPlugin.callNative (jsonParam);
				HIVEUnityPlugin.IMECompositionModeOn();
			}

			/**
			*  \~korean
			* @brief 사용자 로그아웃
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>로그아웃 성공<br>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK가 초기화되지 않은 경우</td>
					<td>AuthV4.setup() API로 HIVE SDK 초기화</td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Sign Out 결과 통지
			*
			*  \~english
			* @brief Player sign-out
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Succeeded to sign out<br>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK not initialized</td>
					<td>Initialize HIVE SDK by implementing AuthV4.setup() API</td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Sign Out result callback			
			*  \~
			*
			* @ingroup AuthV4Helper
			*
			*/
			public static void signOut(onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "signOut", listener);

				HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief 사용자 삭제
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 주요 결과 코드
			<table>
			<tr>
			<td><strong>ErrorCode</strong></td>
			<td><strong>Code</strong></td>
			<td><strong>Description</strong></td>
			<td><strong>Solution</strong></td>
			</tr>
			<tr>
			<td>SUCCESS</td>
			<td>Success</td>
			<td>로그아웃 성공<br>
			<td> - </td>
			</tr>
			<tr>
			<td>NEED_INITIALIZED</td>
			<td>AuthV4NotInitialized</td>
			<td>HIVE SDK가 초기화되지 않은 경우</td>
			<td>AuthV4.setup() API로 HIVE SDK 초기화</td>
			</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Delete 결과 통지
			*
			*  \~english
			* @brief Player delete
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Key result code
			<table>
			<tr>
			<td><strong>ErrorCode</strong></td>
			<td><strong>Code</strong></td>
			<td><strong>Description</strong></td>
			<td><strong>Solution</strong></td>
			</tr>
			<tr>
			<td>SUCCESS</td>
			<td>Success</td>
			<td>Succeeded to sign out<br>
			<td> - </td>
			</tr>
			<tr>
			<td>NEED_INITIALIZED</td>
			<td>AuthV4NotInitialized</td>
			<td>HIVE SDK not initialized</td>
			<td>Initialize HIVE SDK by implementing AuthV4.setup() API</td>
			</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Delete result callback
			*  \~
			*
			* @ingroup AuthV4Helper
			*
			*/
			public static void playerDelete(onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "playerDelete", listener);

				HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief Provider 연결
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Provider 연결 성공<br>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>	HIVE SDK가 초기화되지 않은 경우</td>
					<td>AuthV4.setup() API로 HIVE SDK 초기화</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>Connect를 시도한 Provider의 Player ID가 이미 있는 경우<br>
					<strong>playerInfo</strong>: 충돌 계정 정보
					</td>
					<td>계정 충돌 상황 해결 방법 안내에 따라 해결</td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Connect 결과 통지
			*
			*  \~english
			* @brief Provider Connection
			* <br>
			* ### Condition of use
			*   -# Compelte to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Succeeded to connect with Provider<br>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK not initialized</td>
					<td>Initialize HIVE SDK by implementing AuthV4.setup() API</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>the player ID of the Provider to be connected already existed<br>
					<strong>playerInfo</strong>: The information of conflicted account
					</td>
					<td>Follow the resolution in accordance with the type of account conflicts</td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Connect result callback			
			*  \~
			*
			* @ingroup AuthV4Helper
			*
			*/
			public static void connect(ProviderType providerType, onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "connect", listener);
				jsonParam.AddField("providerType", providerType.ToString());

				HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief Provider 연결
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Provider 연결 해제 성공<br>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK가 초기화되지 않은 경우</td>
					<td>AuthV4.setup() API로 HIVE SDK 초기화</td>
				</tr>
				<tr>
					<td>INVALID_PARAM</td>
					<td>AuthV4ProviderAlreadtDisconnected</td>
					<td>해제 요청받은 Provider가 해당 계정에 연결되지 않은 경우</td>
					<td> - </td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Disconnect 결과 통지
			*
			*  \~english
			* @brief Connects Provider
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Completed to disconnect with Provider<br>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK not initialized</td>
					<td>Initialize HIVE SDK by implementing AuthV4.setup() API</td>
				</tr>
				<tr>
					<td>INVALID_PARAM</td>
					<td>AuthV4ProviderAlreadtDisconnected</td>
					<td>The Provider requested to disconnect was not connected with the account</td>
					<td> - </td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Disconnect result callback
			*  \~
			*
			* @ingroup AuthV4Helper
			*/
			public static void disconnect(ProviderType providerType, onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "disconnect", listener);
				jsonParam.AddField("providerType", providerType.ToString());

				HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief 리더보드 조회
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 특이 사항
			*   -# 로그인 되어있는 계정이 PGS/GameCenter에 연결되어있지 않은 경우 자동으로 해당 서비스로 연결을 시도
			* <br>
			* <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>리더보드 조회 요청 성공<br>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK가 초기화되지 않은 경우</td>
					<td>AuthV4.setup() API로 HIVE SDK 초기화</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>Device에 로그인 된 계정과 현재 로그인 된 계정의 PGS/GameCenter 정보가 다르거나<br>
					Connect를 시도한 Provider의 Player ID가 이미 있는 경우
					</td>
					<td>계정 충돌 상황 해결 방법 안내에 따라 해결</td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Show leaderboard 결과 통지
			*
			*  \~english
			* @brief Queries leaderboard
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Refernce
			*   -# It tries to execute the service automatically if the signed-in account is diconnected with PGS/GameCenter.
			* <br>
			* <br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Succeeded to request leaderboards<br>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK not initialized</td>
					<td>Initialize HIVE SDK by implementing AuthV4.setup() API</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>The account signed in on user device is mismatched with the PGS/Game Center account on the game<br>
					or the player ID of the Provider to be connected already existed</td>
					<td>Follow the resolution in accordance with the type of account conflicts</td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Show leaderboard result callback			
			*  \~
			*
			* @ingroup AuthV4Helper
			*/
			public static void showLeaderboard(onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "showLeaderboard", listener);

				HIVEUnityPlugin.callNative (jsonParam);
			}
			
			/**
			*  \~korean
			* @brief 업적 조회
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 특이 사항
			*   -# 로그인 되어있는 계정이 PGS/GameCenter에 연결되어있지 않은 경우 자동으로 해당 서비스로 연결을 시도
			* <br>
			* <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>업적 조회 요청 성공<br>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK가 초기화되지 않은 경우</td>
					<td>AuthV4.setup() API로 HIVE SDK 초기화</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>Device에 로그인 된 계정과 현재 로그인 된 계정의 PGS/GameCenter 정보가 다르거나<br>
					Connect를 시도한 Provider의 Player ID가 이미 있는 경우
					</td>
					<td>계정 충돌 상황 해결 방법 안내에 따라 해결</td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Show achievements 결과 통지
			*
			*  \~english
			* @brief Queries Acheivements
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Reference
			*   -# It tries to execute the service automatically if the signed-in account is diconnected with PGS/GameCenter.
			* <br>
			* <br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Succeeded to request Acheivement<br>
					<td> - </td>
				</tr>
				<tr>
					<td>NEED_INITIALIZED</td>
					<td>AuthV4NotInitialized</td>
					<td>HIVE SDK not initialized</td>
					<td>Initialize HIVE SDK by implementing AuthV4.setup() API</td>
				</tr>
				<tr>
					<td>CONFLICT_PLAYER</td>
					<td>AuthV4ConflictPlayer</td>
					<td>The account signed in on user device is mismatched with the PGS/Game Center account on the game<br>
					or the player ID of the Provider to be connected already existed</td>
					<td>Follow the resolution in accordance with the type of account conflicts</td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Show achievements result callback
			*  \~
			*
			* @ingroup AuthV4Helper
			*/
			public static void showAchievements(onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "showAchievements", listener);

				HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief 충돌 상태를 알려주고 사용할 계정을 선택하는 HIVE UI 를 보여준다. <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
				</tr>
				<tr>
					<td>PLAYER_CHANGE</td>
					<td>AuthV4PlayerChange</td>
					<td>계정 충돌이 발생한 후 계정 전환에 성공한 경우<br>
				</tr>
				<tr>
					<td>INVALID_PARAM</td>
					<td>AuthV4InvalidConflictInfo</td>
					<td>계정 충돌이 발생하지 않은 상태에서 계정 충돌 해결을 요청한 경우</td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Show Conflict 결과 통지
			*
			*  \~english
			* @brief Displays conflict status, and HIVE UI avilable to select an account. <br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
				</tr>
				<tr>
					<td>PLAYER_CHANGE</td>
					<td>AuthV4PlayerChange</td>
					<td>Succeeded to change the player after accounts conflict<br>
				</tr>
				<tr>
					<td>INVALID_PARAM</td>
					<td>AuthV4InvalidConflictInfo</td>
					<td>Requested to resolve account conflicts not happened</td>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Show Conflict result callback
			*  \~
			*
			* @ingroup AuthV4Helper
			*/
			public static void showConflict(onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "showConflict", listener);

				HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief 충돌 상태를 알려주고 사용할 계정을 선택할 수 있고, 게임 정보를 같이 표현할 수 있는 HIVE UI 를 보여준다. <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
				</tr>
				<tr>
					<td>PLAYER_CHANGE</td>
					<td>AuthV4PlayerChange</td>
					<td>계정 충돌이 발생한 후 계정 전환에 성공한 경우<br>
				</tr>
				<tr>
					<td>INVALID_PARAM</td>
					<td>AuthV4InvalidConflictInfo</td>
					<td>계정 충돌이 발생하지 않은 상태에서 계정 충돌 해결을 요청한 경우</td>
				</tr>
			</table>
			*
			* @param conflictData HIVEConflictSingleViewInfo 충돌이 발생한 사용자의 게임 데이터
			*
			* @param listener AuthV4HelperListener AuthV4Helper Show Conflict 결과 통지
			*
			*  \~english
			* @brief Displays conflict status, and HIVE UI which is available to select an account as well as shows game information.<br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
				</tr>
				<tr>
					<td>PLAYER_CHANGE</td>
					<td>AuthV4PlayerChange</td>
					<td>Succeeded to change the player after accounts conflict<br>
				</tr>
				<tr>
					<td>INVALID_PARAM</td>
					<td>AuthV4InvalidConflictInfo</td>
					<td>Requested to resolve account conflicts not happened</td>
				</tr>
			</table>
			*
			* @param conflictData HIVEConflictSingleViewInfo Game information of the account-conflicted player
			*
			* @param listener AuthV4HelperListener AuthV4Helper Show Conflict result callback
			*  \~
			*
			* @see ConflictSingleViewInfo
			*
			* @ingroup AuthV4Helper
			*/
			public static void showConflict(ConflictSingleViewInfo viewInfo, onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "showConflict", listener);
				jsonParam.AddField("playerData", viewInfo.ToJSONObject());
					Debug.Log(jsonParam.ToString());
				HIVEUnityPlugin.callNative(jsonParam);
			}

			/**
			*  \~korean
			* @brief 계정 Conflict가 발생한 경우 현재 로그인 된 사용자를 로그아웃 시키고, <br>
			* 기기에 연결된 계정으로 로그인을 시도한다.<br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
				</tr>
				<tr>
					<td>PLAYER_CHANGE</td>
					<td>AuthV4PlayerChange</td>
					<td>계정 충돌이 발생한 후 계정 전환에 성공한 경우<br>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Switch Account 결과 통지
			*
			*  \~english
			* @brief If accounts conflict, sign out the current player<br>
			* and try to sign in with the account signed in on the user device.<br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
				</tr>
				<tr>
					<td>PLAYER_CHANGE</td>
					<td>AuthV4PlayerChange</td>
					<td>Succeeded to change the player after accounts conflict<br>
				</tr>
			</table>
			*
			* @param listener AuthV4HelperListener AuthV4Helper Switch Account result callback
			*  \~
			*
			* @ingroup AuthV4Helper
			*/
			public static void switchAccount(onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "switchAccount", listener);

				HIVEUnityPlugin.callNative (jsonParam);
			}
			
			/**
			*  \~korean
			* @brief 계정 Conflict가 발생한 경우 기존 사용자를 유지하는 경우 사용한다. <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
				</tr>
				<tr>
					<td>CANCELED</td>
					<td>AuthV4PlayerResolved</td>
					<td>계정 충돌이 발생한 후 현재 로그인 된 계정을 유지하는 경우</td>
				</tr>
			</table>
			*
			*
			* @param listener AuthV4HelperListener AuthV4Helper Resolve Conflict결과 통지
			*
			*  \~english
			* @brief In case it is not changed the currently signed-in account after accounts conflict.<br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
				</tr>
				<tr>
					<td>CANCELED</td>
					<td>AuthV4PlayerResolved</td>
					<td>Not changed the currently signed-in account after accounts conflict</td>
				</tr>
			</table>
			*
			*
			* @param listener AuthV4HelperListener AuthV4Helper Resolve Conflict result callback
			*  \~
			*
			* @ingroup AuthV4Helper
			*/
			public static void resolveConflict(onAuthV4Helper listener) {
				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "resolveConflict", listener);

				HIVEUnityPlugin.callNative(jsonParam);
			}

			/**
			*  \~korean
			* @brief SDK에서 제공하는 Provider의 목록을 받는다. <br>
			* <br>
			*
			*  \~english
			* @brief Receives the Provider lists sent by SDK. <br>
			* <br>
			*  \~
			*
			* @ingroup AuthV4Helper
			*/
			public static List<ProviderType> getIDPList() {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "getIDPList", null);

				JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

				List<ProviderType> providerTypeList = new List<ProviderType> ();

				JSONObject jsonArray = resJsonObject.GetField("idpList");
				if (jsonArray != null && jsonArray.count > 0) {
					List<JSONObject> jsonList = jsonArray.list;
					foreach (JSONObject jsonItem in jsonList) {

						String providerTypeName = jsonItem.stringValue;
						ProviderType providerType = getProviderType(providerTypeName);

						providerTypeList.Add(providerType);
					}
				}

				return providerTypeList;
			}

			/**
			*  \~korean
			* @brief Game Center 로그인창을 표시할 수 없는 경우, 해당 상태를 보여주고,<br/>
			* Game Center 로그인 방법을 안내하는 UI를 띄운다.
			*
			*  \~english
			* @brief If a sign-in window of Game Center is not displayed, show its status<br/>
			* and display a UI to inform how to sign in the Game Center.
			*  \~
			*
			* @ingroup AuthV4Helper
			*/
			public static void showGameCenterLoginCancelDialog(onAuthV4DialogDismiss listener) {
				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "showGameCenterLoginCancelDialog", listener);
				HIVEUnityPlugin.callNative (jsonParam);
			}
///@cond INTERNAL
#if !UNITY_EDITOR && UNITY_ANDROID
///@endcond
			/**
			*  \~korean
			* @brief 리더보드 점수를 갱신.<br/>
			* @details leaderboardId 에 해당하는 기록에 score 수치로 갱신된다.<br/>
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 특이 사항
			*   -# 로그인 되어있는 계정이 PGS/GameCenter에 연결되어있지 않은 경우 자동으로 해당 서비스로 연결을 시도
			* <br>
			* <br>
			*
			* @param leaderboardId 리더보드 키 값
			* @param score 리더보드 점수
			*
			*  \~english
			* @brief It update the leaderboard score.<br/>
			* @details The score corresponding to the leaderboardId is updated with the score value.<br/>
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Reference
			*   -# It tries to execute the service automatically if the signed-in account is diconnected with PGS/GameCenter.
			* <br>
			* <br>
			*
			* @param leaderboardId leaderboard's key value
			* @param score Leaderboard score
			*
			* @ingroup AuthV4Helper
			*/
			public static void leaderboardsSubmitScore(String leaderboardId, long score) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "leaderboardsSubmitScore", null);
				jsonParam.AddField ("leaderboardId", leaderboardId);
				jsonParam.AddField ("score", score);

				JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief 리더보드 점수를 갱신.<br/>
			* @details leaderboardId 에 해당하는 기록에 score 수치로 갱신된다.<br/>
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 특이 사항
			*   -# 로그인 되어있는 계정이 PGS/GameCenter에 연결되어있지 않은 경우 자동으로 해당 서비스로 연결을 시도
			* <br>
			* <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>리더보드 점수 갱신 요청 성공<br>
					<td> - </td>
				</tr>
				<tr>
					<td>RESPONSE_FAIL</td>
					<td>AuthV4GoogleResponseFailLeaderboardsSubmitScore</td>
					<td>PGS API 함수 호출이 실패한 경우</td>
					<td> - </td>
				</tr>
			</table>
			*
			* @param leaderboardId 리더보드 키 값
			* @param score 리더보드 점수
			* @param listener onAuthV4Helper LeaderboardsSubmitScore 결과 통지
			*
			*  \~english
			* @brief It update the leaderboard score.<br/>
			* @details The score corresponding to the leaderboardId is updated with the score value.<br/>
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Reference
			*   -# It tries to execute the service automatically if the signed-in account is diconnected with PGS/GameCenter.
			* <br>
			* <br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Succeeded to request LeaderboardsSubmitScore<br>
					<td> - </td>
				</tr>
				<tr>
					<td>RESPONSE_FAIL</td>
					<td>AuthV4GoogleResponseFailLeaderboardsSubmitScore</td>
					<td>Failed to request LeaderboardsSubmitScore</td>
					<td> - </td>
				</tr>
			</table>
			*
			* @param leaderboardId leaderboard's key value
			* @param score Leaderboard score
			* @param listener onAuthV4Helper LeaderboardsSubmitScore result callback
			*
			* @ingroup AuthV4Helper
			*/
			public static void leaderboardsSubmitScore(String leaderboardId, long score, onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "leaderboardsSubmitScore", listener);
				jsonParam.AddField ("leaderboardId", leaderboardId);
				jsonParam.AddField ("score", score);

				JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief 숨겨진 업적 공개.
			* @details 업적이 0% 로 공개만 될 뿐 달성 되지는 않는다.
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 특이 사항
			*   -# 로그인 되어있는 계정이 PGS/GameCenter에 연결되어있지 않은 경우 자동으로 해당 서비스로 연결을 시도
			* <br>
			* <br>
			*
			* @param achievementId 업적 키 값
			*
			*  \~english
			* @brief Reveal hidden achievement.
			* @details Achievements are only revealed at 0%, not achieved.
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Reference
			*   -# It tries to execute the service automatically if the signed-in account is diconnected with PGS/GameCenter.
			* <br>
			* <br>
			*
			* @param achievementId achievements's key value
			*
			* @ingroup AuthV4Helper
			*/
			public static void achievementsReveal(String achievementId) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "achievementsReveal", null);
				jsonParam.AddField ("achievementId", achievementId);

				JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief 숨겨진 업적 공개.
			* @details 업적이 0% 로 공개만 될 뿐 달성 되지는 않는다.
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 특이 사항
			*   -# 로그인 되어있는 계정이 PGS/GameCenter에 연결되어있지 않은 경우 자동으로 해당 서비스로 연결을 시도
			* <br>
			* <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>숨겨진 업적 공개 요청 성공<br>
					<td> - </td>
				</tr>
				<tr>
					<td>RESPONSE_FAIL</td>
					<td>AuthV4GoogleResponseFailAchievementsReveal</td>
					<td>PGS API 함수 호출이 실패한 경우</td>
					<td> - </td>
				</tr>
			</table>
			*
			* @param achievementId 업적 키 값
			* @param listener onAuthV4Helper achievementsReveal 결과 통지
			*
			*  \~english
			* @brief Reveal hidden achievement.
			* @details Achievements are only revealed at 0%, not achieved.
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Reference
			*   -# It tries to execute the service automatically if the signed-in account is diconnected with PGS/GameCenter.
			* <br>
			* <br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Succeeded to request achievementsReveal<br>
					<td> - </td>
				</tr>
				<tr>
					<td>RESPONSE_FAIL</td>
					<td>AuthV4GoogleResponseFailAchievementsReveal</td>
					<td>Failed to request achievementsReveal</td>
					<td> - </td>
				</tr>
			</table>
			*
			* @param achievementId achievements's key value
			* @param listener onAuthV4Helper achievementsReveal result callback
			*
			* @ingroup AuthV4Helper
			*/
			public static void achievementsReveal(String achievementId, onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "achievementsReveal", listener);
				jsonParam.AddField ("achievementId", achievementId);

				JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief 업적 달성.
			* @details 숨겨져 있거나 공개된 여부와 상관없이 업적이 100% 로 달성 된다.
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 특이 사항
			*   -# 로그인 되어있는 계정이 PGS/GameCenter에 연결되어있지 않은 경우 자동으로 해당 서비스로 연결을 시도
			* <br>
			* <br>
			*
			* @param achievementId 업적 키 값
			*
			*  \~english
			* @brief Unlock achievement.
			* @details Whether hidden or open, achievement is achieved at 100%.
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Reference
			*   -# It tries to execute the service automatically if the signed-in account is diconnected with PGS/GameCenter.
			* <br>
			* <br>
			*
			* @param achievementId achievements's key value
			*
			* @ingroup AuthV4Helper
			*/
			public static void achievementsUnlock(String achievementId) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "achievementsUnlock", null);
				jsonParam.AddField ("achievementId", achievementId);

				JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief 업적 달성.
			* @details 숨겨져 있거나 공개된 여부와 상관없이 업적이 100% 로 달성 된다.
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 특이 사항
			*   -# 로그인 되어있는 계정이 PGS/GameCenter에 연결되어있지 않은 경우 자동으로 해당 서비스로 연결을 시도
			* <br>
			* <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>업적 달성 요청 성공<br>
					<td> - </td>
				</tr>
				<tr>
					<td>RESPONSE_FAIL</td>
					<td>AuthV4GoogleResponseFailAchievementsUnlock</td>
					<td>PGS API 함수 호출이 실패한 경우</td>
					<td> - </td>
				</tr>
			</table>
			*
			* @param achievementId 업적 키 값
			* @param listener onAuthV4Helper achievementsUnlock 결과 통지
			*
			*  \~english
			* @brief Unlock achievement.
			* @details Whether hidden or open, achievement is achieved at 100%.
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Reference
			*   -# It tries to execute the service automatically if the signed-in account is diconnected with PGS/GameCenter.
			* <br>
			* <br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Succeeded to request achievementsUnlock<br>
					<td> - </td>
				</tr>
				<tr>
					<td>RESPONSE_FAIL</td>
					<td>AuthV4GoogleResponseFailAchievementsUnlock</td>
					<td>Failed to request achievementsUnlock</td>
					<td> - </td>
				</tr>
			</table>
			*
			* @param achievementId achievements's key value
			* @param listener onAuthV4Helper achievementsUnlock result callback
			*
			* @ingroup AuthV4Helper
			*/
			public static void achievementsUnlock(String achievementId, onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "achievementsUnlock", listener);
				jsonParam.AddField ("achievementId", achievementId);

				JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief 업적 수치 증가.<br/>
			* @details 숨겨져 있거나 공개된 여부와 상관없이 업적이 100% 로 달성 된다.<br/>
			* 총 합산이 Max 가 될 경우 자동으로 업적이 달성되며, 계속 호출하여도 무방하다.<br/>
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 특이 사항
			*   -# 로그인 되어있는 계정이 PGS/GameCenter에 연결되어있지 않은 경우 자동으로 해당 서비스로 연결을 시도
			* <br>
			* <br>
			*
			* @param achievementId 업적 키 값
			* @param value 증가 값
			*
			*  \~english
			* @brief Increases achievement figures.<br/>
			* @details Achievement figures is added as much as value set by the API call, not by setting.<br/>
			* If the total sum is Max, the achievement is automatically accomplished.<br/>
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Reference
			*   -# It tries to execute the service automatically if the signed-in account is diconnected with PGS/GameCenter.
			* <br>
			* <br>
			*
			* @param achievementId achievements's key value
			* @param value value
			*
			* @ingroup AuthV4Helper
			*/
			public static void achievementsIncrement(String achievementId, int value) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "achievementsIncrement", null);
				jsonParam.AddField ("achievementId", achievementId);
				jsonParam.AddField ("value", value);

				JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);
			}

			/**
			*  \~korean
			* @brief 업적 수치 증가.<br/>
			* @details 숨겨져 있거나 공개된 여부와 상관없이 업적이 100% 로 달성 된다.<br/>
			* 총 합산이 Max 가 될 경우 자동으로 업적이 달성되며, 계속 호출하여도 무방하다.<br/>
			* <br>
			* ### 사용 조건
			*   -# HIVE SDK 초기화
			*   -# HIVE SignIn 완료
			* <br>
			* <br>
			* ### 특이 사항
			*   -# 로그인 되어있는 계정이 PGS/GameCenter에 연결되어있지 않은 경우 자동으로 해당 서비스로 연결을 시도
			* <br>
			* <br>
			* ### 주요 결과 코드
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>업적 수치 증가 요청 성공<br>
					<td> - </td>
				</tr>
				<tr>
					<td>RESPONSE_FAIL</td>
					<td>AuthV4GoogleResponseFailAchievementsIncrement</td>
					<td>PGS API 함수 호출이 실패한 경우</td>
					<td> - </td>
				</tr>
			</table>
			*
			* @param incrementalAchievementId 업적 키 값
			* @param value 증가 값
			* @param listener onAuthV4Helper achievementsIncrement 결과 통지
			*
			*  \~english
			* @brief Increases achievement figures.<br/>
			* @details Achievement figures is added as much as value set by the API call, not by setting.<br/>
			* If the total sum is Max, the achievement is automatically accomplished.<br/>
			* <br>
			* ### Condition of use
			*   -# Complete to initialize HIVE SDK
			*   -# Complete HIVE SignIn
			* <br>
			* <br>
			* ### Reference
			*   -# It tries to execute the service automatically if the signed-in account is diconnected with PGS/GameCenter.
			* <br>
			* <br>
			* ### Key result code
			<table>
				<tr>
					<td><strong>ErrorCode</strong></td>
					<td><strong>Code</strong></td>
					<td><strong>Description</strong></td>
					<td><strong>Solution</strong></td>
				</tr>
				<tr>
					<td>SUCCESS</td>
					<td>Success</td>
					<td>Succeeded to request achievementsIncrement<br>
					<td> - </td>
				</tr>
				<tr>
					<td>RESPONSE_FAIL</td>
					<td>AuthV4GoogleResponseFailAchievementsIncrement</td>
					<td>Failed to request achievementsIncrement</td>
					<td> - </td>
				</tr>
			</table>
			*
			* @param incrementalAchievementId achievements's key value
			* @param value value
			* @param listener onAuthV4Helper achievementsIncrement result callback
			*
			* @ingroup AuthV4Helper
			*/
			public static void achievementsIncrement(String incrementalAchievementId, int value, onAuthV4Helper listener) {

				JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4Helper", "achievementsIncrement", listener);
				jsonParam.AddField ("incrementalAchievementId", incrementalAchievementId);
				jsonParam.AddField ("value", value);

				JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);
			}
///@cond INTERNAL			
#endif
///@endcond

			public static void executeEngine(JSONObject resJsonObject) {

				String methodName = null;
				resJsonObject.GetField (ref methodName, "method");

				int handlerId = -1;
				resJsonObject.GetField (ref handlerId, "handler");

				// object handler = null;
				// if ("engagement".Equals (methodName) == false) {
				// 	handler = (object)HIVEUnityPlugin.popHandler (handlerId);
				// }

				object handler = HIVEUnityPlugin.popAuthV4Handler(resJsonObject);

				if (handler == null) return;

				if ("syncAccount".Equals (methodName) 
				|| "signIn".Equals (methodName)
				|| "signOut".Equals (methodName)
				|| "playerDelete".Equals (methodName)
				|| "connect".Equals (methodName)
				|| "disconnect".Equals (methodName)
				|| "showLeaderboard".Equals (methodName)
				|| "leaderboardsSubmitScore".Equals (methodName)
				|| "showAchievements".Equals (methodName)
				|| "achievementsReveal".Equals (methodName)
				|| "achievementsUnlock".Equals (methodName)
				|| "achievementsIncrement".Equals (methodName)
				|| "showConflict".Equals (methodName)
				|| "switchAccount".Equals (methodName)
				|| "resolveConflict".Equals (methodName)
				|| "showDeviceManagement".Equals (methodName)
				|| "getHiveTalkPlusLoginToken".Equals (methodName)) {
					if ("signIn".Equals(methodName))
					{
						HIVEUnityPlugin.IMECompositionModeRestore();
					}
					onAuthV4Helper listener = (onAuthV4Helper)handler;

					JSONObject playerInfoJson = resJsonObject.GetField ("playerInfo");

					if (playerInfoJson == null || playerInfoJson.count <= 0) {
						listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), null);
					}
					else {
						listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), 
								new PlayerInfo (playerInfoJson));
					}
				} else if("showGameCenterLoginCancelDialog".Equals(methodName)){
					onAuthV4DialogDismiss listener = (onAuthV4DialogDismiss)handler;
					bool isDismiss = resJsonObject.GetField ("isDismiss");

					if(listener != null) {
						listener(isDismiss);
					}
				}
			}
		}

		public static ProviderType getProviderType(String providerTypeName) {
			
			if ("GUEST".Equals (providerTypeName)) {
				return ProviderType.GUEST;
			}
			else if ("HIVE".Equals (providerTypeName)) {
				return ProviderType.HIVE;
			}
			else if ("FACEBOOK".Equals (providerTypeName)) {
				return ProviderType.FACEBOOK;
			}
			else if ("GOOGLE".Equals (providerTypeName)) {
				return ProviderType.GOOGLE;
			}
			else if ("QQ".Equals (providerTypeName)) {
				return ProviderType.QQ;
			}
			else if ("WEIBO".Equals (providerTypeName)) {
				return ProviderType.WEIBO;
			}
			else if ("VK".Equals (providerTypeName)) {
				return ProviderType.VK;
			}
			else if ("WECHAT".Equals (providerTypeName)) {
				return ProviderType.WECHAT;
			}
			else if ("APPLE".Equals (providerTypeName)) {
				return ProviderType.APPLE;
			}
			else if ("SIGNIN_APPLE".Equals (providerTypeName)) {
				return ProviderType.SIGNIN_APPLE;
			}
			else if ("LINE".Equals (providerTypeName)) {
				return ProviderType.LINE;
			}
			else if ("TWITTER".Equals (providerTypeName)) {
				return ProviderType.TWITTER;
			}
			else if ("WEVERSE".Equals (providerTypeName)) {
				return ProviderType.WEVERSE;
			}
			else if ("NAVER".Equals(providerTypeName))
			{
				return ProviderType.NAVER;
			}
			else if ("GOOGLE_PLAY_GAMES".Equals(providerTypeName))
			{
				return ProviderType.GOOGLE_PLAY_GAMES;
			}
			else if ("HUAWEI".Equals(providerTypeName))
			{
				return ProviderType.HUAWEI;
			}
			else if ("CUSTOM".Equals (providerTypeName)) {
				return ProviderType.CUSTOM;
			}
			else if ("AUTO".Equals (providerTypeName)) {
				return ProviderType.AUTO;
			}

			return ProviderType.GUEST;
		}

		// Native 영역에서 호출된 요청을 처리하기 위한 플러그인 내부 코드
		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			resJsonObject.GetField (ref handlerId, "handler");

			// object handler = null;
			// if ("engagement".Equals (methodName) == false) {
			// 	handler = (object)HIVEUnityPlugin.popHandler (handlerId);
			// }

			object handler = (object)HIVEUnityPlugin.popAuthV4Handler (resJsonObject);

			if (handler == null) return;

			if ("setup".Equals (methodName)) {

				Boolean isAutoSignIn = false;
				resJsonObject.GetField (ref isAutoSignIn, "isAutoSignIn");

				String did = "";
				resJsonObject.GetField (ref did, "did");

				List<ProviderType> providerTypeList = new List<ProviderType> ();

				JSONObject jsonArray = resJsonObject.GetField ("providerTypeList");
				if (jsonArray != null && jsonArray.count > 0) {
					List<JSONObject> jsonList = jsonArray.list;
					foreach (JSONObject jsonItem in jsonList) {

						String providerTypeName = jsonItem.stringValue;
						ProviderType providerType = getProviderType(providerTypeName);

						providerTypeList.Add(providerType);
					}
				}

				onAuthV4Setup listener = (onAuthV4Setup)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), isAutoSignIn, did, providerTypeList);
			}
			else if ("signIn".Equals (methodName) 
				|| "signInWithAuthKey".Equals (methodName)
				|| "showSignIn".Equals (methodName)
				|| "selectConflict".Equals (methodName)
				|| "showConflictSelection".Equals (methodName)) {
				if ("signIn".Equals(methodName)
				|| "showSignIn".Equals(methodName)){
					HIVEUnityPlugin.IMECompositionModeRestore();
				}
				onAuthV4SignIn listener = (onAuthV4SignIn)handler;

				JSONObject playerInfoJson = resJsonObject.GetField ("playerInfo");

				if (playerInfoJson == null || playerInfoJson.count <= 0) {
					listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), null);
				}
				else {
					listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), 
							new PlayerInfo (playerInfoJson));
				}
			}
			else if ("signOut".Equals (methodName)
				|| "playerDelete".Equals (methodName)) {

				onAuthV4SignOut listener = (onAuthV4SignOut)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("connect".Equals (methodName)
				|| "connectWithAuthKey".Equals(methodName)) {

				onAuthV4Connect listener = (onAuthV4Connect)handler;
				JSONObject conflictPlayerJson = resJsonObject.GetField ("conflictPlayer");

				if (conflictPlayerJson == null || conflictPlayerJson.count <= 0)
					listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), null);
				else
					listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), 
							new PlayerInfo (conflictPlayerJson));
			}
			else if ("disconnect".Equals (methodName)
								|| "disconnectWithName".Equals(methodName) ) {

				onAuthV4Disconnect listener = (onAuthV4Disconnect)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("getProfile".Equals (methodName)) {

				List<ProfileInfo> profileInfoList = new List<ProfileInfo> ();

				JSONObject jsonArray = resJsonObject.GetField ("profileInfoList");
				if (jsonArray != null && jsonArray.count > 0) {
					List<JSONObject> jsonList = jsonArray.list;
					foreach (JSONObject jsonItem in jsonList) {
						ProfileInfo profileInfo = new ProfileInfo(jsonItem);
						profileInfoList.Add(profileInfo);
					}
				}

				onAuthV4GetProfile listener = (onAuthV4GetProfile)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), profileInfoList);
			}
			else if ("showProfile".Equals (methodName)) {
				HIVEUnityPlugin.IMECompositionModeRestore();
				onAuthV4ShowProfile listener = (onAuthV4ShowProfile)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("showInquiry".Equals (methodName)) {
				HIVEUnityPlugin.IMECompositionModeRestore();
				onAuthV4ShowInquiry listener = (onAuthV4ShowInquiry)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("showMyInquiry".Equals (methodName)) {
				HIVEUnityPlugin.IMECompositionModeRestore();
				onAuthV4ShowMyInquiry listener = (onAuthV4ShowMyInquiry)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			} 
			else if ("showChatbotInquiry".Equals (methodName)) {
				HIVEUnityPlugin.IMECompositionModeRestore();
				onAuthV4ShowChatbotInquiry listener = (onAuthV4ShowChatbotInquiry)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("showTerms".Equals (methodName)) {

				onAuthV4ShowTerms listener = (onAuthV4ShowTerms)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("showAdultConfirm".Equals (methodName)) {

				onAuthV4AdultConfirm listener = (onAuthV4AdultConfirm)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("checkProvider".Equals (methodName)) {

				onDeviceProviderInfo listener = (onDeviceProviderInfo)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), 
						new ProviderInfo (resJsonObject.GetField ("providerInfo")));
			}
			else if ("checkMaintenance".Equals (methodName)) {

				List<AuthV4MaintenanceInfo> maintenanceInfoList = new List<AuthV4MaintenanceInfo> ();
				
				JSONObject jsonArray = resJsonObject.GetField ("AuthV4MaintenanceInfoList");
				if (jsonArray != null && jsonArray.count > 0) {
					List<JSONObject> jsonList = jsonArray.list;
					foreach (JSONObject jsonItem in jsonList) {
						
						AuthV4MaintenanceInfo maintenanceInfo = new AuthV4MaintenanceInfo(jsonItem);
						maintenanceInfoList.Add(maintenanceInfo);
					}
				}

				onAuthV4Maintenance listener = (onAuthV4Maintenance)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), maintenanceInfoList);
			}
			else if ("checkBlacklist".Equals (methodName)) {

				List<AuthV4MaintenanceInfo> maintenanceInfoList = new List<AuthV4MaintenanceInfo> ();
				
				JSONObject jsonArray = resJsonObject.GetField ("AuthV4MaintenanceInfoList");
				if (jsonArray != null && jsonArray.count > 0) {
					List<JSONObject> jsonList = jsonArray.list;
					foreach (JSONObject jsonItem in jsonList) {
						
						AuthV4MaintenanceInfo maintenanceInfo = new AuthV4MaintenanceInfo(jsonItem);
						maintenanceInfoList.Add(maintenanceInfo);
					}
				}

				onAuthV4Maintenance listener = (onAuthV4Maintenance)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), maintenanceInfoList);
			}
			else if ("setProviderChangedListener".Equals (methodName)) {

				onDeviceProviderInfo listener = (onDeviceProviderInfo)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), 
						new ProviderInfo (resJsonObject.GetField ("providerInfo")));
			}
			else if ("getProviderFriendsList".Equals (methodName)) {

				String providerTypeName = "";
				resJsonObject.GetField (ref providerTypeName, "providerType");
				ProviderType providerType = getProviderType(providerTypeName);

				Dictionary<String, Int64> providerUserIdList = new Dictionary<String, Int64> ();
				
				JSONObject jsonObject = resJsonObject.GetField ("providerUserIdList");
				if (jsonObject != null && jsonObject.count > 0) {	

					foreach (string key in jsonObject.keys) {

						Int64 providerUserId = 0;
						jsonObject.GetField(ref providerUserId, key);

						providerUserIdList.Add(key, providerUserId);
					}
				}

				onGetProviderFriendsList listener = (onGetProviderFriendsList)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), providerType, providerUserIdList);
			}
			else if("resolveConflict".Equals (methodName)) {

				onAuthV4ResolveConflict listener = (onAuthV4ResolveConflict)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if("showDeviceManagement".Equals (methodName)) {
				HIVEUnityPlugin.IMECompositionModeRestore();
				onAuthV4ShowDeviceManagement listener = (onAuthV4ShowDeviceManagement)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if("getHiveTalkPlusLoginToken".Equals (methodName)) {
				onAuthV4GetHiveTalkPlusLoginToken listener = (onAuthV4GetHiveTalkPlusLoginToken)handler;
				String loginToken = "";
				resJsonObject.GetField(ref loginToken, "loginToken");
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), loginToken);
			}
			else if("requestPermissionViewData".Equals(methodName)) {

				onAuthV4RequestPermissionViewData listener = (onAuthV4RequestPermissionViewData)handler;
				JSONObject permissionData = resJsonObject.GetField("data");
				listener(new ResultAPI (resJsonObject.GetField ("resultAPI")),permissionData == null ? null : new PermissionViewData(permissionData));
			} else if("showGameCenterLoginCancelDialog".Equals(methodName)){
                    onAuthV4DialogDismiss listener = (onAuthV4DialogDismiss)handler;
                    bool isDismiss = resJsonObject.GetField ("isDismiss");

                    if(listener != null) {
                        listener(isDismiss);
                    }
                }
		}
	}
}

/** @} */