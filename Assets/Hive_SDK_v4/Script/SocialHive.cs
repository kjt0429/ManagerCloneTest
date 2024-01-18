/**
 * @file    SocialHive.cs
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
 * @defgroup Social
 *   
 * \~korean HIVE 유저는 자신의 프로필 정보를 조회 하거나 수정할 수 있고 친구를 맺어서 친구와 함께 게임을 즐길 수 있는 소셜 게임 서비스를 제공한다.<br/>
 * HIVE 는 Facebook, 주소록 등의 외부 유저 정보를 이용하여 HIVE 친구를 맺고 목록의 동기화를 수행할 수 있다.<br/>
 * SocialHive 클래스는 HIVE 유저의 프로필, 친구 목록, 소셜 웹뷰 기능을 제공한다.<br/><br/>
 * \~english HIVE users can view and modify their profile information and provide a social game service where they can make friends and play games with their friends together.<br/>
 * HIVE can make HIVE friends and synchronize the list by using external user information such as Facebook and contacts on your device.<br/>
 * SocialHive class provides HIVE user profile, friend list and social web <br/><br/>
 * \~
 * @defgroup SocialHive
 * @ingroup Social 
 * @addgroup SocialHive
 * @{
 */

namespace hive
{

	/**
	 * \~korean HIVE 유저는 자신의 프로필 정보를 조회 하거나 수정할 수 있고 친구를 맺어서 친구와 함께 게임을 즐길 수 있는 소셜 게임 서비스를 제공한다.<br/>
	 * HIVE 는 Facebook, 주소록 등의 외부 유저 정보를 이용하여 HIVE 친구를 맺고 목록의 동기화를 수행할 수 있다.<br/>
	 * SocialHive 클래스는 HIVE 유저의 프로필, 친구 목록, 소셜 웹뷰 기능을 제공한다.
	 *
	 * \~english HIVE users can view and modify their profile information and provide a social game service where they can make friends and play games with their friends together.<br/>
	 * HIVE can make HIVE friends and synchronize the list by using external user information such as Facebook and contacts on your device.<br/>
	 * SocialHive class provides HIVE user profile, friend list and social web 
	 * \~
	 * @ingroup SocialHive
	 */
	public class SocialHive {


		/**
	     * \~korean HIVE 유저의 프로필 / 친구 목록 결과 통지
	     *
	     * @param result		API 호출 결과
	     * @param profileList	HIVE 유저의 프로필 목록.<br/><br/>(자신의 프로필일 경우 단건, 친구 목록일 경우 여러건이 반환된다.)
	     * \~english Returns HIVE Profile / Friend Information 
	     *
	     * @param result		API call result
	     * @param profileList	HIVE user's profile list.<br/><br/>(If you ask for your profile, one result is returned, and if you  ask for friends list, a list of friends is returned.)
		 * \~
		 * @ingroup SocialHive
	     */
		public delegate void onProfileHive(ResultAPI result, List<ProfileHive> profileList);


		/**
	     * \~korean HIVE 친구 메시지 전송 결과 통지
	     *
	     * @param result		API 호출 결과
	     * \~english Returns HIVE message transmission result
	     *
	     * @param result		API call result
		 * \~
		 * @ingroup SocialHive
	     */
		public delegate	void onSendMessageHive(ResultAPI result);


		/**
	     * \~korean HIVE 웹뷰 대화상자 결과 통지
	     *
	     * @param result		API 호출 결과
	     * \~english Returns showing HIVE Social dialog result
	     *
	     * @param result		API call result
		 * \~
		 * @ingroup SocialHive
	     */
		public delegate void onShowHiveDialog(ResultAPI result);


		/**
		 * \~korean HIVE Social Badge 정보
		 * 
		 * @param result		API 호출 결과
		 * @param badge			HIVE-SocialBadge정보
		 * \~english HIVE Social Badge information
		 * 
		 * @param result		API call result
		 * @param badge			HIVE-Social Badge information
		 * \~
		 * @ingroup SocialHive
		 */
		public delegate void onSocialBadge(ResultAPI result, SocialBadge badge);


		/**
	     * \~korean HIVE 유저 자신의 프로필 정보 조회
	     *
	     * @param listener	API 결과 통지
	     * \~english Request profile information of HIVE certified users
	     *
	     * @param listener	API call result listener
		 * \~
		 * @ingroup SocialHive
	     */
		public static void getMyProfile(onProfileHive listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialHive", "getMyProfile", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
	     * \~korean HIVE 유저 자신의 프로필 정보 설정
	     *
	     * @param displayName	유저의 상태 메시지
	     * @param listener		API 결과 통지
	     * \~english Set profile information of HIVE certified users
	     *
	     * @param displayName	Message of user's status
	     * @param listener		API call result listener
		 * \~
		 * @ingroup SocialHive
	     */
		public static void setMyProfile(String comment, onProfileHive listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialHive", "setMyProfile", listener);
			jsonParam.AddField("comment", comment);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean HIVE 사용자 정보를 조회한다.
		 * 
		 * @param vidList 조회하고자 하는 사용자의 VID 목록
		 * @param listener API 결과 통지
		 * \~english HIVE 사용자 정보를 조회한다.
		 * 
		 * @param vidList 조회하고자 하는 사용자의 VID 목록
		 * @param listener API 결과 통지
		 * \~
		 * @ingroup SocialHive
		 */
		 public static void getProfiles(String[] vidList, onProfileHive listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialHive", "getProfiles", listener);

			JSONObject jsonArray = new JSONObject();

			if(vidList != null) {
				foreach(String vid in vidList) {
					jsonArray.Add(vid);
				}
			}

			jsonParam.AddField("vidList", jsonArray);

			HIVEUnityPlugin.callNative (jsonParam);
		 }

		/**
		 * \~korean  HIVE 유저의 친구 정보 조회<br/>
		 * 친구의 형태는 게임을 같이하고 있는 친구, 게임을 같이하고 있지 않은 친구, 게임을 초대한 친구, 모든 친구가 있고 FriendType 라는 enum 으로 정의되어 있다
		 * 
		 * @param friendType	친구 목록 조회 형태
		 * @param listener		API 결과 통지
		 * \~english Request HIVE Profile / Friend Information 
	     * In the form of friends, there are friends who play games together, friends who do not play games, friends who invite games, all friends, and these are defined by an enum called FriendType
	     *
	     * @param friendType Types of Friend list
	     * @param listener		API call result listener
		 * \~
		 * @ingroup SocialHive
		 */
		public static void getFriends(FriendType friendType, onProfileHive listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialHive", "getFriends", listener);
			jsonParam.AddField("friendType", friendType.ToString());

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
	     * \~korean HIVE 친구에게 메시지 전송 요청<br/>
	     * HIVE 친구에게 메시지를 발송하면 상대방이 메시지를 수신한 것을 알 수 있도록 푸시 알림이 발송된다.<br/>
	     * 푸시 알림이 발송되는 매체는 발신자가 이용 중인 게임과 동일한 게임 또는 최근에 이용한 게임이 된다.<br/>
	     * 수신자가 푸시 알림을 받고 메시지를 터치하면 해당 게임이 실행 되므로, 게임 내에 HIVE 메시지로 연결되는 UI를 포함해야 한다
	     *
	     * @param messageContent	HIVE 메시지를 전송 할 대상의 정보
	     * @param listener			API 결과 통지
	     * \~english Send message to HIVE friend <br/>
	     * If you send a message to a HIVE friend, a push notification will be sent to the recipient who has received the message.<br/>
	     * The medium to which the push notification is sent is the same game as the game that the sender is using, or a game that has been used recently.<br/>
	     * When the recipient receives the push notification and touches the message, the game is executed, so the UI that leads to the HIVE message should be included in the game.
	     *
	     * @param messageContent	Information to send HIVE message
	     * @param listener			API call result listener
		 * \~
		 * @ingroup SocialHive
	     */
		public static void sendMessage(MessageContent messageContent, onSendMessageHive listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialHive", "sendMessage", listener);
			jsonParam.AddField("messageContent", messageContent.toJson());

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
	     * \~korean HIVE 유저에게 초대 메시지 전송 요청
	     *
	     * @param messageContent	HIVE 초대 메시지를 전송 할 대상의 정보
	     * @param listener			API 결과 통지
	     * \~english Send a invite message to HIVE user
	     *
	     * @param messageContent	Information to send HIVE invite message
	     * @param listener			API call result listener
		 * \~
		 * @ingroup SocialHive
	     */
		public static void sendInvitationMessage(MessageContent messageContent, onSendMessageHive listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialHive", "sendInvitationMessage", listener);
			jsonParam.AddField("inviteContents", messageContent.toJson());

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
	     * \~korean HIVE 웹뷰 대화상자 호출<br/>
	     * (- HIVE의 기본 첫 화면 (피드){@inheritDoc HiveDialogType#HOME}<br/>
	     * - 자신의 프로필 페이지{@inheritDoc HiveDialogType#USER}<br/>
	     * - 게임 페이지{@inheritDoc HiveDialogType#GAME}<br/>
	     * - 1:1 문의 하기 {@inheritDoc HiveDialogType#INQUIRY})
		 * - HIVE 쪽지 페이지. {@inheritDoc HiveDialogType#MESSAGE}
		 * - 내 문의 하기 {@inheritDoc HiveDialogType#MYINQUIRY}
	     *
	     * @param hiveDialogType
	     *            {@link HiveDialogType} HIVE 웹뷰 대화상자 형태
	     * @param vid
	     *            친구의 프로필 페이지로 바로 갈 경우 친구의 vid 를 설정한다
	     * @param listener
	     *            API 호출 결과 통지
	     * \~english Show HIVE WebView dialog<br/>
	     * (- HIVE Social's default first screen (feed){@inheritDoc HiveDialogType#HOME}<br/>
	     * - Profile page{@inheritDoc HiveDialogType#USER}<br/>
	     * - Games{@inheritDoc HiveDialogType#GAME}<br/>
	     * - 1:1 Contact us {@inheritDoc HiveDialogType#INQUIRY})
		 * - HIVE Note. {@inheritDoc HiveDialogType#MESSAGE}
		 * - My Inquiry {@inheritDoc HiveDialogType#MYINQUIRY}
	     *
	     * @param hiveDialogType
	     *            {@link HiveDialogType} Type of HIVE WebView Dialog
	     * @param vid
	     *            If you go directly to your friend's profile page, set your friend's vid
	     * @param listener
	     *            API call result listener
		 * \~
		 * @ingroup SocialHive
	     */
		public static void showHiveDialog(HiveDialogType hiveDialogType, String vid, onShowHiveDialog listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialHive", "showHiveDialog", listener);
			jsonParam.AddField("hiveDialogType", hiveDialogType.ToString());
			jsonParam.AddField("vid", vid);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		* \~korean HIVE 웹뷰 대화상자 호출<br/>
		* (- HIVE의 기본 첫 화면 (피드){@inheritDoc HiveDialogType#HOME}<br/>
		* - 자신의 프로필 페이지{@inheritDoc HiveDialogType#USER}<br/>
		* - 게임 페이지{@inheritDoc HiveDialogType#GAME}<br/>
		* - 1:1 문의 하기 {@inheritDoc HiveDialogType#INQUIRY})
		* - HIVE 쪽지 페이지. {@inheritDoc HiveDialogType#MESSAGE}
		* - 챗봇 1:1 문의 하기 {@inheritDoc HiveDialogType#CHATBOT}
		* - 내 문의 하기 {@inheritDoc HiveDialogType#MYINQUIRY}
		*
		* @param hiveDialogType
		*            {@link HiveDialogType} HIVE 웹뷰 대화상자 형태
		* @param vid
		*            친구의 프로필 페이지로 바로 갈 경우 친구의 vid 를 설정한다
		* @param additionalInfo
		*            챗봇 페이지를 바로가기 위해  전달받기로한 약속된 JSON 형식의 String 데이터
		* @param handler
		*            API 호출 결과 통지
		* \~english Show HIVE WebView dialog<br/>
		* (- HIVE Social's default first screen (feed){@inheritDoc HiveDialogType#HOME}<br/>
		* - Profile page{@inheritDoc HiveDialogType#USER}<br/>
		* - Games{@inheritDoc HiveDialogType#GAME}<br/>
		* - 1:1 Contact us {@inheritDoc HiveDialogType#INQUIRY})
		* - HIVE Note. {@inheritDoc HiveDialogType#MESSAGE}
		* - Chatbot 1:1 Contact us {@inheritDoc HiveDialogType#CHATBOT})
		* - My Inquiry {@inheritDoc HiveDialogType#MYINQUIRY}
		*
		* @param hiveDialogType
		*            {@link HiveDialogType} Type of HIVE WebView Dialog
		* @param vid
		*            If you go directly to your friend's profile page, set your friend's vid
		* @param additionalInfo
		*            Promised String data (JSON format) when you want to open chatbot page directly
		* @param handler
		*            API call result handler
		* \~
		* @ingroup SocialHive
		*/
		public static void showHiveDialog(HiveDialogType hiveDialogType, String vid, String additionalInfo, onShowHiveDialog listener) {
						
			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialHive", "showHiveDialog", listener);
			jsonParam.AddField("hiveDialogType", hiveDialogType.ToString());
			jsonParam.AddField("vid", vid);
			jsonParam.AddField("additionalInfo", additionalInfo);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		* \~korean HIVE Social Badge 정보를 조회.
		* 
		* @param listener
		*            {@link SocialBadgeListener} HIVE SocialBadge 정보 조회 결과 통지
		* 
		* \~english Request HIVE Social Badge info.
		* 
		* @param listener
		*            {@link SocialBadgeListener} HIVE SocialBadge info result listener
		 * \~
		 * @ingroup SocialHive
		*/
		public static void getBadgeInfo(onSocialBadge listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialHive", "getBadgeInfo", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		// \internal
		// \~korean Native 영역에서 호출된 요청을 처리하기 위한 플러그인 내부 코드
		// \~english Internal code to handle requests invoked from the native code
		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			resJsonObject.GetField (ref handlerId, "handler");
			object handler = (object)HIVEUnityPlugin.popHandler (handlerId);

			if (handler == null) return;

			if ("getMyProfile".Equals (methodName) ||
				"getProfiles".Equals(methodName) ||
				"getFriends".Equals (methodName)) {

				List<ProfileHive> profiles = new List<ProfileHive> ();
				JSONObject profileListJson = resJsonObject.GetField("profileList");
				if (profileListJson != null && profileListJson.count > 0) {

					List<JSONObject> jsonList = profileListJson.list;
					foreach (JSONObject jsonItem in jsonList) {
						ProfileHive newProfile = new ProfileHive (jsonItem);
						profiles.Add (newProfile);
					}
				}

				onProfileHive listener = (onProfileHive)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), profiles);
			}
			else if ("setMyProfile".Equals(methodName)) {
					
				onProfileHive listener = (onProfileHive)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), null);
			}
			else if ("sendMessage".Equals (methodName)) {

				onSendMessageHive listener = (onSendMessageHive)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("sendInvitationMessage".Equals (methodName)) {

				onSendMessageHive listener = (onSendMessageHive)handler;
				listener(new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("showHiveDialog".Equals (methodName)) {

				onShowHiveDialog listener = (onShowHiveDialog)handler;
				listener(new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if( "getBadgeInfo".Equals (methodName)) {
				
				onSocialBadge listener = (onSocialBadge)handler;
				listener(new ResultAPI (resJsonObject.GetField ("resultAPI")), new SocialBadge(resJsonObject.GetField("socialBadge")));
			}
		}

	}		// end of public class SocialHive




	/**
	 * \~korean 친구 목록 조회 형태
	 *
	 * \~english Types of Friend list
	 * \~
	 * @ingroup SocialHive
	 */
	public enum FriendType {

		IN_GAME			///< \~korean 게임을 같이하고 있고, HIVE 상에서 친구 관계인 경우  \~english  If you are playing a game and they are friends on HIVE.
		, OUT_GAME		///< \~korean 게임을 같이하고 있지 않지만, HIVE 상에서 친구 관계인 경우  \~english  If you are not playing games, but they are friends on HIVE.
		, INVITED		///< \~korean HIVE 소셜에서 초대한 친구  \~english  Friends invited by HIVE Social.
		, ALL_GAME		///< \~korean HIVE 상에서 친구 관계인 경우  \~english  All friends on HIVE.
	}


	/**
     * \~korean HIVE 웹뷰 대화상자의 형태
     *
     * \~english Types of HIVE Social WebView Dialog 
	 * \~
	 * @ingroup SocialHive
     */
	public enum HiveDialogType {

		HOME			///< \~korean Hive 소셜 웹 뷰 홈 페이지 \~english  Hive Social home.
		, GAME			///< \~korean Hive 게임 목록 페이지 \~english  Hive Social game list.
		, USER			///< \~korean Hive 유저 프로필 페이지. \~english  Hive Social user profile.
		// , CAFE			///< \~korean Hive 카페 페이지. \~english  Hive Social Cafe.
		, INQUIRY		///< \~korean Hive 1:1 문의하기 페이지. \~english  Hive 1:1 Inquiry.
		, MESSAGE		///< \~korean Hive 쪽지 페이지. Hive 유저만 사용 가능. \~english  Hive Note. Only for Hive user.
		, CHATBOT		///< \~korean Hive 챗봇 1:1 문의하기 페이지. \~english Hive Chatbot 1:1 Inquiry.
		, MYINQUIRY		///< \~korean Hive 내 문의하기 페이지. \~english Hive My Inquiry.
	}


	/**
     * \~korean HIVE 친구 관계가 맺어진 경로 형태.
     *
     * \~english HIVE The path form of friendship.
	 * \~
	 * @ingroup SocialHive
     */
	public enum HiveRelationRoute
	{
		DEFAULT         ///< not set
		, HIVE          ///< \~korean HIVE Social 로 맺어짐. \~english  Through HIVE Social.
		, FACEBOOK      ///< \~korean Facebook으로 맺어짐. \~english  Through Facebook.
		, GAME          ///< \~korean Game에서 맺어짐. \~english  Through Game.
		, CONTACT       ///< \~korean 주소록으로 맺어짐. \~english  Through Contacts.
	}




	/**
     * \~korean HIVE 유저의 프로필 정보<br>
     *
     * \~english HIVE user profile information<br>
	 * \~
	 * @ingroup SocialHive
     */
	public class ProfileHive {

		public String vid;				///< \~korean HIVE 로그인을 수행하면 게임별로 발급되는 사용자의 고유 ID<br/><br/>게임에서는 vid 기준으로 유저 정보를 관리한다  \~english  The unique ID of the user issued per game when performing a HIVE login <br/> <br/> The game manages user information by vid.

		public String uid;				///< \~korean HIVE Social 에서 사용하는 게임과 별개인 사용자 고유의 ID (Big Integer형태)  \~english  Unique ID (Big Integer type) used in HIVE Social

		public String identifier;		///< \~korean HIVE 로그인 ID (max 12)  \~english  HIVE Login ID (max 12)

		public String userName;			///< \~korean 사용자가 입력한 HIVE 닉네임 또는 facebook name (max 128)  \~english  HIVE nickname or facebook name entered by the user (max 128)

		public String facebookId;		///< \~korean 페이스북 계정이 연결된 경우 페이스북 Id, 없으면 null  \~english  Facebook Id if Facebook account is connected, null if not.

		public String googleplusId;		///< \~korean 구글플러스 계정이 연결된 경우 구글플러스 Id, 추후 PGS PlayerId변경 가능, 없으면 null (Android only.)  \~english  If you have a Google Sign-in account linked or null if none (Android only.)

		public String profileImageUrl;	///< \~korean 프로필 이미지 URL  \~english  Profile image URL

		public String country;			///< \~korean 유저 선택에 따른 국가 코드  \~english  Country code according to user selection

		public String comment;			///< \~korean 유저의 상태 메시지  \~english  The user's status message (a "word" entered by the user)

		public Boolean cached;			///< \~korean 데이터 캐싱 여부(테스트 필드)  \~english  Whether data is cached (test field)

		public Boolean testAccount;		///< \~korean 테스트 계정 여부, true/false  \~english  Whether it is a test account, true/false

		
		// 이하 세개 필드는 HIVE 접속 유저의 추가 정보.
		// @author nanomech

		public String	email;			///< \~korean 로그인 메일 주소. 없으면 null  \~english  Login email address. Null if none

		public String	birthday;		///< \~korean 생년월일 , 설정 안되었으면 null  \~english  Birthday , Null if none

		public String	gender;			///< \~korean 성별 , M or F , 설정 안되었으면 null  \~english  Gender , Null if none
		
		// 이하 두개 필드는 HIVE 친구 추가정보.
		// @author nanomech

		public Boolean	gameFriend;		///< \~korean 게임을 같이 하는 게임 친구 여부, true/false  \~english  Whether in game friend, true/false

		public String	assnet;			///< \~korean HIVE 회원 전환 여부, C: Com2us, G : Com2us Holdings, H : HIVE, or null  \~english  HIVE Membership, C: Com2us, G : Com2us Holdings, H : HIVE, or null
		
		// HIVE SDK 1.10. 피처내용 적용. 친구 맺음 경로.
		// @author nanomech
		public HiveRelationRoute	relationRoute;		///< \~korean 최초로 친구 관계가 된 경로 정보. \~english A route that you became friend for the first time.

		public ProfileHive() {
		}

		public ProfileHive(JSONObject resJsonParam) {

			if (resJsonParam == null || resJsonParam.count <= 0)
				return;

			resJsonParam.GetField (ref this.vid, "vid");
			resJsonParam.GetField (ref this.uid, "uid");
			resJsonParam.GetField (ref this.identifier, "identifier");
			resJsonParam.GetField (ref this.userName, "userName");
			resJsonParam.GetField (ref this.facebookId, "facebookId");
			resJsonParam.GetField (ref this.googleplusId, "googleplusId");
			resJsonParam.GetField (ref this.profileImageUrl, "profileImageUrl");
			resJsonParam.GetField (ref this.country, "country");
			resJsonParam.GetField (ref this.comment, "comment");
			resJsonParam.GetField (ref this.cached, "cached");
			resJsonParam.GetField (ref this.testAccount, "testAccount");

			resJsonParam.GetField (ref this.email, "email");
			resJsonParam.GetField (ref this.birthday, "birthday");
			resJsonParam.GetField (ref this.gender, "gender");

			resJsonParam.GetField (ref this.gameFriend, "gameFriend");
			resJsonParam.GetField (ref this.assnet, "assnet");
		}


		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append ("ProfileHive {");
			sb.Append (" vid = ");
			sb.Append (this.vid);
			sb.Append (", uid = ");
			sb.Append (this.uid);
			sb.Append (", identifier = ");
			sb.Append (this.identifier);
			sb.Append(", userName = ");
			sb.Append(this.userName);
			sb.Append(", facebookId = ");
			sb.Append(this.facebookId);
			sb.Append(", googleplusId = ");
			sb.Append(this.googleplusId);
			sb.Append(", profileImageUrl = ");
			sb.Append(this.profileImageUrl);
			sb.Append(", country = ");
			sb.Append(this.country);
			sb.Append(", comment = ");
			sb.Append(this.comment);
			sb.Append(", cached = ");
			sb.Append(this.cached);
			sb.Append(", testAccount = ");
			sb.Append(this.testAccount);

			sb.Append(", email = ");
			sb.Append(this.email);
			sb.Append(", birthday = ");
			sb.Append(this.birthday);
			sb.Append(", gender = ");
			sb.Append(this.gender);

			sb.Append(", gameFriend = ");
			sb.Append(this.gameFriend);
			sb.Append(", assnet = ");
			sb.Append(this.assnet);
			sb.Append (" }\n");

			return sb.ToString();
		}
	}


	/**
     * \~korean 메시지 전송 내용
     *
     * \~english Message information
	 * \~
	 * @ingroup SocialHive
     */
	public class MessageContent {

		public String vid;				///< \~korean 쪽지를 전송할 상대의 vid, uid가 null인경우, 필수 파라매터.  \~english  The vid of the person to whom you want to send the note. it is required if uid is null.
		public String uid;				///< \~korean 쪽지를 전송할 상대의 uid, null이거나 빈문자열인 경우, vid에 해당하는 uid를 사용한다.  \~english The uid of the recipient of the note.
		public String message;			///< \~korean 쪽지 메시지  \~english Message
		public String imageUrl;			///< \~korean 첨부 이미지 URL  \~english Attached image URL
		public String thumbnailUrl;		///< \~korean 첨부 이미지의 썸네일 URL  \~english Thumbnail URL of attached image
		public Boolean usePush;			///< \~korean 쪽지 받는 사람이 접속한 디바이스에 쪽지 내용을 Push 로 보내는지 여부  \~english Whether to send the contents of the note to the device to which the recipient has connected.

		public MessageContent() {
		}

		public MessageContent(String vid, String uid, String message, String imageUrl, String thumbnailUrl, Boolean usePush) {

			this.vid = vid;
			this.uid = uid;
			this.message = message;
			this.imageUrl = imageUrl;
			this.thumbnailUrl = thumbnailUrl;
			this.usePush = usePush;
		}


		public JSONObject toJson() {

			JSONObject resJson = new JSONObject ();
			resJson.AddField ("vid", this.vid);
			resJson.AddField ("uid", this.uid);
			resJson.AddField ("message", this.message);
			resJson.AddField ("imageUrl", this.imageUrl);
			resJson.AddField ("thumbnailUrl", this.thumbnailUrl);
			resJson.AddField ("usePush", this.usePush);

			return resJson;
		}


		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append ("MessageContent {");
			sb.Append (this.toJson ().ToString ());
			sb.Append (" }\n");

			return sb.ToString();
		}
	}

	/**
	 * \~korean HIVE Social Badge 정보.
	 * 
	 * \~english HIVE Social Badge information.
	 * \~
	 * @ingroup SocialHive
	 *
	 */
	public class SocialBadge{

		public int messageCount;		///< \~korean 쪽지 개수  \~english The number of message

		public SocialBadge() {
		}

		public SocialBadge(JSONObject resJsonParam) {

			if (resJsonParam == null || resJsonParam.count <= 0)
				return;

			resJsonParam.GetField (ref this.messageCount, "messageCount");

		}

		public JSONObject toJson() {

			JSONObject resJson = new JSONObject ();
			resJson.AddField ("messageCount", this.messageCount);

			return resJson;
		}


		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append ("SocialBadge {");
			sb.Append (this.toJson ().ToString ());
			sb.Append (" }\n");

			return sb.ToString();
		}
	}

}		// end of namespace hive


/** @} */



