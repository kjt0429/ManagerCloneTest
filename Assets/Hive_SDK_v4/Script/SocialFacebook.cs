/**
 * @file    SocialFacebook.cs
 *
 *  @date		2016-2022
 *  @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 *  @author		ryuvsken, nanomech
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
 * @defgroup SocialFacebook
 * @ingroup Social 
 * @addgroup SocialFacebook
 * @{
 */

namespace hive
{

	/**
	 * @ingroup SocialFacebook
	 * @author		ryuvsken, nanomech
	 * @since		4.0.0
	 * \~korean HIVE 유저는 자신의 프로필 정보를 조회 하거나 수정할 수 있고 친구를 맺어서 친구와 함께 게임을 즐길 수 있는 소셜 게임 서비스를 제공한다.<br/>
	 * HIVE 는 Facebook, 주소록 등의 외부 유저 정보를 이용하여 HIVE 친구를 맺고 목록의 동기화를 수행할 수 있다.<br/>
	 * SocialFacebook 클래스는 Facebook 프로필, Facebook 친구 목록, Facebook 글 게시 기능을 제공한다.<br/><br/>
	 *
	 * \~english HIVE users can view and modify their profile information and provide a social game service where they can make friends and play games with their friends together.<br/>
	 * HIVE can make HIVE friends and synchronize the list by using external user information such as Facebook and contacts on your device.<br/>
	 * The SocialFacebook class provides Facebook profile, Facebook friend list, and posting.<br/><br/>
	 */
	public class SocialFacebook {

		/**
	     * \~korean HIVE 프로필 / 친구 정보 결과 통지
	     *
	     * @param result		API 호출 결과
	     * @param profileList	Facebook 유저의 프로필 목록.<br/><br/>(자신의 프로필일 경우 단건, 친구 목록일 경우 여러건이 반환된다.)
	     * \~english Returns HIVE Profile / Friend Information 
	     *
	     * @param result		API call result
	     * @param profileList	Facebook user's profile list.<br/><br/>(If you ask for your profile, one result is returned, and if you  ask for friends list, a list of friends is returned.)
		 * \~
		 * @ingroup SocialFacebook
	     */
		public delegate void onProfileFacebook(ResultAPI result, List<ProfileFacebook> profileList);


		/**
	     * \~korean Facebook 메시지 전송 결과 통지
	     *
	     * @param result		API 호출 결과
	     * \~english Returns Facebook message transmission result
	     *
	     * @param result		API call result
		 * \~
		 * @ingroup SocialFacebook
	     */
		public delegate	void onSendMessageFacebook(ResultAPI result);


		/**
	     * \~korean Facebook 유저에게 친구 초대 요청 결과 통지
	     *
	     * @param result			API 호출 결과
	     * @param invitedUserList	초대된 Facebook 유저의 ID 목록
	     * \~english Returns Facebook user invite to friend
	     *
	     * @param result			API call result
	     * @param invitedUserList	List of IDs of invited Facebook users
		 * \~
		 * @ingroup SocialFacebook
	     */
		public delegate	void onShowInvitationDialogFacebook(ResultAPI result, List<String> invitedUserList);


		/**
	     * \~korean Facebook 게시글 등록 요청 결과 통지
	     *
	     * @param result			API 호출 결과
	     * \~english Returns Facebook posting result
	     *
	     * @param result			API call result
		 * \~
		 * @ingroup SocialFacebook
	     */
		public delegate void onPostFacebook(ResultAPI result);




		/**
	     * \~korean Facebook 인증 사용자의 프로필 정보 조회
	     *
	     * @param listener	API 호출 결과 통지
	     * \~english Request profile information of Facebook certified users
	     *
	     * @param listener	API call result listener
		 * \~
		 * @ingroup SocialFacebook
	     */
		public static void getMyProfile(onProfileFacebook listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialFacebook", "getMyProfile", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
	     * \~korean Facebook 친구 목록 조회
	     *
	     * @param listener	API 호출 결과 통지
	     * \~english Request Facebook friends list
	     *
	     * @param listener	API call result listener 
		 * \~
		 * @ingroup SocialFacebook
	     */
		public static void getFriends(onProfileFacebook listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialFacebook", "getFriends", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
	     * \~korean Facebook 친구에게 메시지 전송
	     *
	     * @param messageContents		Facebook 메시지를 전송할 정보
	     * @param listener 		API 호출 결과 통지
	     * \~english Send message to Facebook friend
	     *
	     * @param messageContents		Facebook message to be sent
	     * @param listener 		API call result listener
		 * \~
		 * @ingroup SocialFacebook
	     */
		public static void sendMessageFacebook(FacebookMessage messageContents, onSendMessageFacebook listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialFacebook", "sendMessageFacebook", listener);
			jsonParam.AddField ("messageContents", messageContents.toJson());

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
	     * \~korean Facebook 유저에게 친구 초대 요청 대화상자 호출<br/>
	     * (일반적으로 소셜의 친구를 늘리기 위해서 게임 친구 초대에 대한 보상을 수행하도록 운영하지만 Facebook 에서는 이런 운영 방식을 정책적으로 막고 있으니 주의해야 한다.)
	     *
	     * @param inviteContents		Facebook 초대 메시지를 전송할 정보
	     * @param listener		API 호출 결과 통지
	     * \~english Show Facebook friend invite dialog to Facebook user <br/>
	     * (Note: Generally, to increase the number of social friends, you can give a reward for inviting friends. but Facebook prohibited this.)
	     *
	     * @param inviteContents		Facebook invite message
	     * @param listener		API call result listener
		 * \~
		 * @ingroup SocialFacebook
	     */
		public static void showInvitationDialog(FacebookMessage inviteContents, onShowInvitationDialogFacebook listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialFacebook", "showInvitationDialog", listener);
			jsonParam.AddField ("inviteContents", inviteContents.toJson());

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
	     * \~korean 게임의 소식을 흥미 있게 전하기 위해서 메시지와 이미지를 Facebook 에 포스팅하여 글쓴이의 타임라인에 글을 노출 시키는 기능을 제공한다
	     *
	     * @param contentURL		Facebook 게시글 정보 URL
	     * @param listener		API 호출 결과 통지
	     * \~english Post messages and images on Facebook timeline.
	     *
	     * @param contentURL		Facebook posting information URL.
	     * @param listener		API call result
		 * \~
		 * @ingroup SocialFacebook
	     */
		public static void postFacebookWithContentURL(String contentURL, onPostFacebook listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialFacebook", "postFacebookWithContentURL", listener);
			jsonParam.AddField ("contentURL", contentURL);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		\~korean 사용자의 Facebook 세션 정보가 client에 있는 여부를 반환한다.
		
		@return Boolean. YES이면 사용자의 Facebook 세션 정보가 client에 있음. 아니면 NO.

		\~english 사용자의 Facebook 세션 정보가 client에 있는 여부를 반환한다.
		
		@return Boolean. YES이면 사용자의 Facebook 세션 정보가 client에 있음. 아니면 NO.
		 * \~
		 * @ingroup SocialFacebook
		*/
		public static Boolean isLogin() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialFacebook", "isLogin", null);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

			Boolean isLogin = false;
			resJsonObject.GetField (ref isLogin, "isLogin");
			return isLogin;
		}


		// \internal
		// \korean Native 영역에서 호출된 요청을 처리하기 위한 플러그인 내부 코드
		// \english Plug-in internal code to handle requests invoked from the native code
		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			resJsonObject.GetField (ref handlerId, "handler");
			object handler = (object)HIVEUnityPlugin.popHandler (handlerId);

			if (handler == null) return;

			if ("getMyProfile".Equals (methodName) ||
				"getFriends".Equals (methodName)) {

				List<ProfileFacebook> profiles = new List<ProfileFacebook> ();
				JSONObject profileListJson = resJsonObject.GetField("profileList");
				if (profileListJson != null && profileListJson.count > 0) {

					List<JSONObject> jsonList = profileListJson.list;
					foreach (JSONObject jsonItem in jsonList) {
						ProfileFacebook newProfile = new ProfileFacebook (jsonItem);
						profiles.Add (newProfile);
					}
				}

				onProfileFacebook listener = (onProfileFacebook)handler;
				listener (new ResultAPI (resJsonObject.GetField("resultAPI")), profiles);
			}
			else if ("sendMessageFacebook".Equals (methodName)) {

				onSendMessageFacebook listener = (onSendMessageFacebook)handler;
				listener (new ResultAPI (resJsonObject.GetField("resultAPI")));
			}
			else if ("showInvitationDialog".Equals (methodName)) {

				List<String> invitedUserList = new List<String> ();

				JSONObject listJson = resJsonObject.GetField("invitedUserList");
				if (listJson != null && listJson.count > 0) {

					List<JSONObject> jsonList = listJson.list;
					foreach (JSONObject jsonItem in jsonList) {
						invitedUserList.Add(jsonItem.stringValue);
					}
				}

				onShowInvitationDialogFacebook listener = (onShowInvitationDialogFacebook)handler;
				listener (new ResultAPI (resJsonObject.GetField("resultAPI")), invitedUserList);
			}
			else if ("postFacebook".Equals (methodName)
			|| "postFacebookWithContentURL".Equals (methodName)) {

				onPostFacebook listener = (onPostFacebook)handler;
				listener (new ResultAPI (resJsonObject.GetField("resultAPI")));
			}
		}
	}




	/**
     * \~korean Facebook 유저의 프로필 정보
     *
     * \~english Facebook user profile information
	 * \~
	 * @ingroup SocialFacebook
     */
	public class ProfileFacebook {

		public String uid;			///< \~korean 페이스북 사용자 AppScoped uid. \~english Facebook AppScoped uid.
		public String email;		///< \~korean 페이스북 사용자 email (권한을 허가한 경우) \~english Facebook user email (If permission granted)
		public String gender;		///< \~korean 페이스북 사용자의 표기된 성별. \~english Gender on Facebook.
		public String language;		///< \~korean 페이스북 사용자의 로케일 식별자로부터 파싱한 언어. \~english Language parsed from Facebook user's locale identifier.
		public String country;		///< \~korean 페이스북 사용자의 로케일 식별자로부터 파싱한 국적. \~english Country parsed from Facebook user's locale identifier.
		public String username;		///< \~korean 페이스북 사용자의 이름. \~english Facebook user name.
		public String bio;			///< \~korean 페이스북 사용자의 바이오그라피. \~english Self introduction information of Facebook users.
		public String profileImageUrl;	///< \~korean 페이스북 사용자의 프로파일 이미지 url. \~english Facebook user profile image url.


		public ProfileFacebook() {
		}

		public ProfileFacebook(String uid, String email, String gender, String language, String country, String username, String bio, String profileImageUrl) {

			this.uid = uid;
			this.email = email;
			this.gender = gender;
			this.language = language;
			this.country = country;
			this.username = username;
			this.bio = bio;
			this.profileImageUrl = profileImageUrl;
		}


		public ProfileFacebook(JSONObject resJsonParam) {

			if (resJsonParam == null || resJsonParam.count <= 0)
				return;

			resJsonParam.GetField (ref this.uid, "uid");
			resJsonParam.GetField (ref this.email, "email");
			resJsonParam.GetField (ref this.gender, "gender");
			resJsonParam.GetField (ref this.language, "language");
			resJsonParam.GetField (ref this.country, "country");
			resJsonParam.GetField (ref this.username, "username");
			resJsonParam.GetField (ref this.bio, "bio");
			resJsonParam.GetField (ref this.profileImageUrl, "profileImageUrl");
		}


		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append ("ProfileFacebook {");
			sb.Append ("uid = ");
			sb.Append (this.uid);
			sb.Append (", email = ");
			sb.Append (this.email);
			sb.Append (", gender = ");
			sb.Append (this.gender);
			sb.Append (", language = ");
			sb.Append (this.language);
			sb.Append (", country = ");
			sb.Append (this.country);
			sb.Append (", username = ");
			sb.Append (this.username);
			sb.Append (", bio = ");
			sb.Append (this.bio);
			sb.Append (", profileImageUrl = ");
			sb.Append (this.profileImageUrl);
			sb.Append (" }\n");

			return sb.ToString();
		}
	}


	/**
	 *  \~korean Facebook 메시지를 전송 할 대상 정보
	 * 
	 * @see <a href="https://developers.facebook.com/docs/games/services/gamerequests"> Facebook-AppRequest </a>
	 * @see <a href="https://developers.facebook.com/docs/reference/dialogs/requests/"> Facebook-Request </a>
	 * 
	 * @see <a href="https://developers.facebook.com/docs/app-invites/overview">Facebook-AppInvite </a>
	 * 
	 *  \~english Facebook message destinations
	 *
	 * @see <a href="https://developers.facebook.com/docs/games/services/gamerequests"> Facebook-AppRequest </a>
	 * @see <a href="https://developers.facebook.com/docs/reference/dialogs/requests/"> Facebook-Request </a>
	 *
	 * @see <a href="https://developers.facebook.com/docs/app-invites/overview">Facebook-AppInvite </a>
	 * \~
	 * @ingroup SocialFacebook
	 */
	public class FacebookMessage {
		public List<String>	recipients;			///< \~korean 수신받을 사람들의 Facebook ID 목록.<br/><br/>(초대 용도로 쓰는 경우 무시됨) \~ List of Facebook IDs of people who received message.<br/><br/>(Ignored if used for invitation purposes)
		public String	dialogTitle;		///< \~korean 메시지 전송 대화 상자의 제목. Max 50. \~english Title. Max 50.
		public String	message;			///< \~korean 메시지 내용 \~english Message
		public String	data;				///< \~korean 전달할 숨김데이터. Max 255. \~english Hidden data to send. Max 255.

		public FacebookMessage () {
		}

		public FacebookMessage (List<String> recipients, String dialogTitle, String message, String data) {
			this.recipients = recipients;
			this.dialogTitle = dialogTitle;
			this.message = message;
			this.data = data;
		}

		public JSONObject toJson () {

			JSONObject resJsonParam = new JSONObject ();

			JSONObject recipientsJson = new JSONObject ();
			foreach(String recipientId in this.recipients) {
				recipientsJson.Add (recipientId);
			}
			resJsonParam.AddField ("recipients", recipientsJson);
			resJsonParam.AddField ("dialogTitle", this.dialogTitle);
			resJsonParam.AddField ("message", this.message);
			resJsonParam.AddField ("data", this.data);
			return resJsonParam;
		}

		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("FacebookMessage { recipients = ");
			sb.Append(this.recipients);
			sb.Append(", dialogTitle = ");
			sb.Append(this.dialogTitle);
			sb.Append(", message = ");
			sb.Append(this.message);
			sb.Append(", data = ");
			sb.Append(this.data);
			sb.Append (" }\n");

			return sb.ToString();
		}
	}

}


/** @} */



