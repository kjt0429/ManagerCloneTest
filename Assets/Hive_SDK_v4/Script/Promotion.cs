/**
 * @file    Promotion.cs
 * 
 *  @date		2016-2022
 *  @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 *  @author		ryuvsken
 *  @since		4.0.0
 * 
 */
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

/**
 * @defgroup Promotion
 * @{
 * \~korean  HIVE 프로모션은 게임을 이용하는 유저에게 게임의 새로운 소식이나 이벤트를 효과적으로 노출하는 기능을 제공한다<br/>
 * 프로모션 뷰 (공지, 이벤트), 보상 (쿠폰, 딥링크), 종료 팝업 (안드로이드), 무료 충전소, 리뷰 유도 팝업<br/><br/>
 * \~english HIVE Promotion gives users who use the game the ability to effectively expose new news or events in the game.<br/>
 * Promotion view (notice, event), reward (coupon, deep link), exit popup (Android), free charging station(Offerwall)<br/><br/>
 */

namespace hive
{
	/**
	 * \~korean  HIVE 프로모션은 게임을 이용하는 유저에게 게임의 새로운 소식이나 이벤트를 효과적으로 노출하는 기능을 제공한다<br/>
	 * 프로모션 뷰 (공지, 이벤트), 보상 (쿠폰, 딥링크), 종료 팝업 (안드로이드), 무료 충전소, 리뷰 유도 팝업<br/><br/>
	 * 
	 * \~english HIVE Promotion provides you with the ability to effectively expose new news or events to the game user.<br/>
	 * Promotion view (notice, event), reward (coupon, deep link), exit popup (Android), free charging station(Offerwall)<br/><br/>
	 * \~
	 * @author ryuvsken
	 * @since		4.0.0
	 * @ingroup Promotion
	 */
	public class Promotion {

		/**
		 * \~korean  프로모션 뷰 API 결과 통지
		 * 
		 * @param result				API 호출 결과
		 * @param promotionEventType	프로모션 창 이벤트 형태
		 * 
		 * \~english Promotion View API Result 
		 * 
		 * @param result				API call result 
		 * @param promotionEventType	Promotion event type
		 * \~
		 * @ingroup Promotion
		 */
		public delegate void onPromotionView(ResultAPI result, PromotionEventType promotionEventType);

		/**
		 * \~korean  HIVE 프로모션 웹 뷰의 UI 를 게임 UI 의 컨셉에 맞추기 위해서 프로모션 웹 뷰를 게임에서 직접 구현하기 위한 데이터 반환
		 * 
		 * @param result	API 호출 결과
		 * @param viewInfo	프로모션 웹 뷰 정보
		 * \~english Returns HIVE Promotion Webview information so that your UI of webview is configured according to the concept of game UI.
		 * 
		 * @param result	API call result 
		 * @param viewInfo	HIVE Promotion Webview information
		 * \~
		 * @ingroup Promotion
		 */
		public delegate void onPromotionViewInfo (ResultAPI result, List<PromotionViewInfo> viewInfo);

		/**
		 * \~korean  HIVE 프로모션 뱃지 정보 반환
		 * 
		 * @param result	API 호출 결과
		 * @param badgeInfoList		프로모션 뱃지 정보
		 * \~english Returns HIVE Promotion badge information.
		 * 
		 * @param result	API call result 
		 * @param badgeInfoList		HIVE Promotion badge information
		 * \~
		 * @ingroup Promotion
		 */
		public delegate void onPromotionBadgeInfo (ResultAPI result, List<PromotionBadgeInfo> badgeInfoList);

        /**
         *  \~korean 프로모션 배너 정보 API 호출에 대한 결과 통지
         *
         * @param result    API 호출 결과
         * @param bannerInfos   프로모션 배너 정보
         *  \~english HIVE Promotion banner information.
		 *
		 * @param result	API call result 
		 * @param bannerInfoList		HIVE Promotion banner information
		 * \~
		 * @ingroup Promotion
         */
        public delegate void onPromotionBannerInfo(ResultAPI result, List<PromotionBannerInfo> bannerInfoList);

        /**
		 * \~korean  앱 초대 (UserAcquisition) 정보 요청에 대한 정보 반환
		 * 
		 * @param result	API 호출 결과
		 * @param appInvitationData		앱 초대 정보
		 * \~english Return information about request for user invite (UserAcquisition)
		 * 
		 * @param result	API call result 
		 * @param appInvitationData		User invite information.
		 * \~
		 * @ingroup Promotion
		 */
		public delegate void onAppInvitationData (ResultAPI result, AppInvitationData appInvitationData);


		/**
		 * \~korean  SDK 가 특정한 조건에서 클라이언트에 개입 (Engagement) 하기 위한 이벤트 리스너.<br>
		 * 여기서 특정한 조건은 모바일 메시지 (SMS), 푸시 알림 (Push Notification) 으로 전송된 메시지의 URL 클릭이나 프로모션 뷰에서 사용자 동작 등이 있다.
		 *
		 * @param result					API 호출 결과
		 * @param engagementEventType		이벤트 타입
		 * @param engagementEventState		이벤트 상태
		 * @param param						이벤트 정보
		 *
		 * \~english  An event listener for the SDK to engage clients in certain conditions.<br>
		 * The specific conditions are, for example, a mobile message (SMS), a URL click on a message in a push notification, or a user action in a promotional view.
		 *
		 * @param result					Result of API call
		 * @param engagementEventType		Event type
		 * @param engagementEventState		Event status
		 * @param param						Event information
		 * \~
		 * @ingroup Promotion
		 */
		public delegate void onEngagement(ResultAPI result, EngagementEventType engagementEventType, EngagementEventState engagementEventState, JSONObject param);

        public delegate void onPromotionShare (ResultAPI result);

		/**
		* \~korean UA를 통해 자신을 앱으로 최초로 초대 성공한 유저의 정보 반환 한다.
		*
		* @param result API 호출 결과
		* @param senderInfo 자신을 앱으로 최초로 초대 성공한 유저의 정보
		*
		* \~english First sender's information who shared UA invitation.
		*
		* @param result Result of API call
		* @param senderInfo First sender's userInfo
		* \~
		* @ingroup Promotion
		*/
		public delegate void onAppInvitationSenderInfo(ResultAPI result, AppInvitationSenderInfo senderInfo);


		/**
		 * \~korean  게임의 새로운 이벤트나 새로운 게임 소개등의 배너 화면을 노출
		 * 
		 * @param promotionType		프로모션 뷰 창의 형태
		 * @param isForced			type이 "notice" 와 "event" 일 때만 동작
		 * 
		 * 							true일 경우 24시간 다시보지 않기를 무시한다. 
		 * 							(주의:24시간 다시보기가 적용되지 않으므로 버튼 액션으로 보는 등 특정 액션에서만 사용해야 함.)
		 * 
		 * 							false이거나 생략할 경우 24시간 안보기가 정상동작함.
		 * @param listener			API 결과 통지
		 * \~english Shows banner such as new event of game or introduce new game
		 * 
		 * @param promotionType		Promotion View type
		 * @param isForced			If ture, it only works when promotionType is "notice" and "event"
		 * 
		 * 							and it ignore 'Not seeing it again for 24 hours'. 
		 * 							(Note: Since 'Not seeing it again for 24 hours' does not apply, you should only use certain actions, such as viewing as a button action.)
		 * 
		 * 							If false or omitted, 'Not seeing it again for 24 hours' will operate normally.
		 * @param listener			API call result listener
		 * \~
		 * @ingroup Promotion
		 */
		public static void showPromotion(PromotionType promotionType, Boolean isForced, onPromotionView listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "showPromotion", listener);
			jsonParam.AddField ("promotionType", promotionType.ToString());
			jsonParam.AddField ("isForced", isForced);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean 외부 컨텐츠를 사용하기 위해서 커스텀 웹 뷰 노출
		 * @param promotionCustomType		PromotionCustomType Type
		 * @param contentsKey				백오피스에 설정된 사용자 정의 페이지의 id
		 * @param listener 					API 결과 통지 리스너
		 *
		 *  \~english Show custom Webview to use external content.
		 * @param promotionCustomType		PromotionCustomType Type
		 * @param contentsKey				ID of the custom web page set in the back office.
		 * @param listener					API call result listener
		 * \~
		 * @ingroup Promotion
		 */
		public static void showCustomContents(PromotionCustomType customType, String contentsKey, onPromotionView listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "showCustomContents", listener);
			jsonParam.AddField ("customType", customType.ToString());
			jsonParam.AddField ("contentsKey", contentsKey);

			HIVEUnityPlugin.callNative (jsonParam);
			HIVEUnityPlugin.IMECompositionModeOn();
		}


		/**
		 * \~korean 외부 컨텐츠를 사용하기 위해서 커스텀 웹 뷰 노출
		 * @param promotionCustomType		PromotionCustomType Type
		 * @param contentsKey				백오피스에 설정된 사용자 정의 페이지의 id
		 * @param listener 					API 결과 통지 리스너
		 *
		 *  \~english Show custom Webview to use external content.
		 * @param promotionCustomType		PromotionCustomType Type
		 * @param contentsKey				ID of the custom web page set in the back office.
		 * @param listener					API call result listener
		 * \~
		 * @ingroup Promotion
		 */
		public static void showCustomContentsOnGameWindow(PromotionCustomType customType, String contentsKey, onPromotionView listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "showCustomContentsOnGameWindow", listener);
			jsonParam.AddField ("customType", customType.ToString());
			jsonParam.AddField ("contentsKey", contentsKey);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		* \~korean  게임의 새로운 이벤트나 새로운 게임 소개등의 새소식 화면을 노출
		* @param menu             최초 노출시 활성화할 메뉴 관리 명
		* @param handler          API 결과 통지
		* \~english Shows banner such as new event of game or introduce new game
		*
		* @param menu             Menu Management Name
		* @param giftPidList	  gift icon
		* @param handler          API call result handler
		* \~
		* @ingroup Promotion
		*/
		public static void showNews(String menu, List<int> giftPidList, onPromotionView listener) {

			JSONObject giftPidArray = new JSONObject();
			if (giftPidList != null)
            {
				foreach (int giftPid in giftPidList)
				{
					giftPidArray.Add(giftPid);
				}
			}

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "showNews", listener);
			jsonParam.AddField ("menu", menu);
			jsonParam.AddField ("giftPidList", giftPidArray);

			HIVEUnityPlugin.callNative (jsonParam);

		}

		/**
		 * \~korean  게임내에서 무료 충전소를 노출하기 위한 버튼 UI 는 보여지거나 숨길 수 있도록 구성해야 한다.<br/>
		 * 이 메서드는 게임내에서 오퍼월(무료 충전소) 을 호출 할 수 있는 버튼 노출 가능 여부를 반환한다.
		 * 
		 * @return 오퍼월(무료 충전소) 을 호출 할 수 있는 버튼 노출 가능 여부
		 * \~english Returns whether the button is available to invoke an offerwall (free recharging station) within the game.<br/>
		 * The button UI for exposing free recharge stations within the game must be configured to be visible or hidden.
		 * 
		 * @return whether the button is available.
		 * \~
		 * @ingroup Promotion
		 */
		public static OfferwallState getOfferwallState() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "getOfferwallState", null);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

			String value = "";
			resJsonObject.GetField (ref value, "offerwallState");

			if ("ENABLED".Equals (value.ToUpper ()))
				return OfferwallState.ENABLED;
			else if ("DISABLED".Equals (value.ToUpper ()))
				return OfferwallState.DISABLED;
			else
				return OfferwallState.UNKNOWN;
		}


		/**
		 * \~korean  무료 충전소 화면 노출 (Android only.)<br/>
		 * 무료 충전소는 HIVE 게임 간의 광고 네트워크로 기존 HIVE 유저들이 새로운 HIVE 게임을 이용할 수 있도록 유도하는 기능이다.<br/>
		 * 유저가 다운로드 받을 수 있는 게임을 목록으로 노출하고 게임을 다운로드 받아 실행하면 매체가 되는 게임에서 보상이 제공된다.<br/>
		 * 
		 * @param listener	API 결과 통지
		 * \~english Show Offerwall(Free recharging station) (Android only.)<br/>
		 * The Offerwall (free recharge station) is an advertising network between HIVE games, which enables existing HIVE users to use the new HIVE game.<br/>
		 * A list of the games that the user can download is exposed, and the game is downloaded and executed, and reward is provided in the game as the medium.<br/>
		 * 
		 * @param listener	API call result listener
		 * \~
		 * @ingroup Promotion
		 */
		public static void showOfferwall(onPromotionView listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "showOfferwall", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}
		
		/**
		* \~korean  리뷰 유도 팝업 노출<br/>
		* 게임 유저들의 긍정적인 평점 및 사용 후기는 다른 유저들이 게임을 이용하는데 영향을 미치게 된다.<br/>
		* 리뷰 유도 팝업을 제공하면 유저의 참여 건수가 5~10배 증가하는 것으로 알려져 있다.<br/>
		* iOS 10.3 이상의 기기에서는 앱 내부에서 평점 및 리뷰를 작성할 수 있는 팝업이 노출된다.
		*
		* @param listener API 결과 통지
		* 
		* \~english Show review popup<br/>
		* Positive ratings and reviews of game users will affect other users' use of the game.<br/>
		* It is known that the number of user participation increases by 5 ~ 10 times when the review popup is provided.
		* Review popup on the device with iOS 10.3 and later is available to rate and write reviews in apps. 
		* 
		* @param listner API result callback
		* Added in HIVE SDK 4.10.0
		* \~
		* @ingroup Promotion
		*/
		public static void showNativeReview() {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "showNativeReview", null);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		* \~korean  리뷰 유도 팝업 노출<br/>
		* 게임 유저들의 긍정적인 평점 및 사용 후기는 다른 유저들이 게임을 이용하는데 영향을 미치게 된다.<br/>
		* 리뷰 유도 팝업을 제공하면 유저의 참여 건수가 5~10배 증가하는 것으로 알려져 있다.<br/>
		* iOS 10.3 이상의 기기에서는 앱 내부에서 평점 및 리뷰를 작성할 수 있는 팝업이 노출된다.
		*
		* @param listener API 결과 통지
		* 
		* \~english Show review popup<br/>
		* Positive ratings and reviews of game users will affect other users' use of the game.<br/>
		* It is known that the number of user participation increases by 5 ~ 10 times when the review popup is provided.
		* Review popup on the device with iOS 10.3 and later is available to rate and write reviews in apps. 
		* 
		* @param listner API result callback
		* Added in HIVE SDK 4.10.0
		* \~
		* @ingroup Promotion
		*/
		public static void showReview(onPromotionView listener) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "showReview", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		 * \~korean  종료 확인 팝업 노출 (Android only.)<br/>
		 * 이용 중인 게임을 종료 하려는 유저에게 새로운 HIVE 게임의 다운로드를 유도하기 위해 '더 많은 게임'이란 버튼을 노출한다
		 * 
		 * @param listener API 결과 통지
		 * \~english Show exit popup (Android only)<br/>
		 * Expose exit popup which include a button called "More games" to lead users to download a new HIVE game.
		 * 
		 * @param listener API call result listener
		 * \~
		 * @ingroup Promotion
		 */
		public static void showExit(onPromotionView listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "showExit", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean  HIVE 프로모션 웹 뷰의 UI 를 게임 UI 의 컨셉에 맞추기 위해서 프로모션 웹 뷰를 게임에서 직접 구현 할 수 있다<br/>
		 * 이 메서드는 게임에서 HIVE 프로모션 웹 뷰를 커스터 마이징하기 위한 정보를 반환한다.
		 * 
		 * @param listener API 결과 통지
		 * \~english Request HIVE Promotion Webview information so that your UI of webview is configured according to the concept of game UI<br/>
		 * 
		 * @param listener API call result listener
		 * \~
		 * @ingroup Promotion
		 */
		public static void getViewInfo(PromotionCustomType customType, String contentsKey, onPromotionViewInfo listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "getViewInfo", listener);
			jsonParam.AddField ("customType", customType.ToString());
			jsonParam.AddField ("contentsKey", contentsKey);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean  뱃지 정보 조회<br/>
		 * 프로모션 뱃지는 유저에게 새로운 프로모션 내용이 등록 되었음을 알려주기 위해서 게임의 버튼 UI 를 부각하는 정보이다
		 * 
		 * @param listener API 결과 통지
		 * \~english Request badge information<br/>
		 * The promotional badge is information that highlights the button UI of the game to inform the user that a new promotion has been registered.
		 * 
		 * @param listener API call result listener
		 * \~
		 * @ingroup Promotion
		 */
		public static void getBadgeInfo(onPromotionBadgeInfo listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "getBadgeInfo", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}

        /**
         * \~korean  배너 정보 조회<br/>
         * 특정 프로모션에 대한 배너 정보를 요청한다. 게임 서버에서 배너 정보를 조회하기 위해 사용된다.
         *
         * @param campaignType "event", "notice", "all", "cross"
         * @param handler API 결과 통지
         *
         * \~english Request banner information<br/>
         * Request banner information for a specific promotion. Used to retrieve banner information from the game server.
         *
         * @param campaignType "event", "notice", "all", "cross"
         * @param handler API call result listener
         * \~
         * @ingroup Promotion
         */
        public static void getBannerInfo(PromotionCampaignType campaignType, PromotionBannerType bannerType, onPromotionBannerInfo listener)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "getBannerInfo", listener);
            jsonParam.AddField("campaignType", campaignType.ToString().ToLower());
            jsonParam.AddField("bannerType", bannerType.ToString().ToLower());

            HIVEUnityPlugin.callNative(jsonParam);
        }

		/**
         * \~korean  배너 정보 조회<br/>
         * 특정 프로모션에 대한 배너 정보를 요청한다. 게임 서버에서 배너 정보를 조회하기 위해 사용된다.
         *
         * @param campaignType campaignType
		 * @param bannerType bannerType
         * @param handler API 결과 통지
         *
         * \~english Request banner information<br/>
         * Request banner information for a specific promotion. Used to retrieve banner information from the game server.
         *
         * @param campaignType campaignType
		 * @param bannerType bannerType
         * @param handler API call result listener
         * \~
         * @ingroup Promotion
         */
		public static void getBannerInfoString(String campaignType, String bannerType, onPromotionBannerInfo listener)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "getBannerInfoString", listener);
            jsonParam.AddField("campaignType", campaignType);
            jsonParam.AddField("bannerType", bannerType);

            HIVEUnityPlugin.callNative(jsonParam);
        }

		/**
		 * \~korean  앱 초대를 위한 데이터 조회<br/>
		 * 게임에서 더 많은 사용자를 유치하기 위해서 앱 설치 유도 기능을 제공한다.<br/>
		 * 앱 설치 유도는 유저 애퀴지션 (User Acquisition) 이라고도 부른다.<br/>
		 * 앱의 초대를 위한 데이터는 QR Code, 초대링크, 캠페인 등이 있다.<br/>
		 * 캠페인은 초대에 대한 보상을 달상하기 위한 조건을 명시한 데이터이다.<br/>
		 * 초대 URL 또는 QR코드를 통해 초대받은 유저가 게임을 설치하고, 특정 레벨 달성 등과 같은 조건(백오피스에서 조건 및 보상 설정 가능)을 달성했을 때 초대한 유저와 초대받은 유저 모두에게 보상을 제공한다.
		 * 
		 * @param listener	API 호출 결과.
		 * \~english Request user invite information<br/>
		 * User acquisition information is provided to attract more users in the game.<br/>
		 * Data for user acquisition include QR Code, invite links, and campaigns.<br/>
		 * A campaign is data that specifies the conditions for rewarding an invite.<br/>
		 * The invite URL or QR code provides rewards to both the invited and invited users when the invited user achieves the conditions such as installing the game and achieving a certain level (condition and reward can be set in the back office).
		 * 
		 * @param listener	API call result.
		 * \~
		 * @ingroup Promotion
		 */
		public static void getAppInvitationData(onAppInvitationData listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "getAppInvitationData", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}

        /**
         * \~korean 외부 SNS를 통해 UA url을 공유하기 위한 다이얼로그 노출
         * @param inviteMessage				UA 초대 메시지
         * @param inviteLink				UA 초대 링크
         * @param listener 					API 결과 통지 리스너
         *
         *  \~english Show dialog to share UA url via SNS.
         * @param inviteMessage				UA invite message
         * @param inviteLink				UA invite link
         * @param listener					API call result listener
         * \~
         * @ingroup Promotion
         */
		public static void showUAShare(String inviteMessage, String inviteLink, onPromotionShare listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "showUAShare", listener);
			jsonParam.AddField ("inviteMessage", inviteMessage);
			jsonParam.AddField ("inviteLink", inviteLink);

			HIVEUnityPlugin.callNative (jsonParam);
		}


		/**
		 * \~korean  프로모션 뷰의 특정 이벤트 페이지를 노출하거나 프로모션 서버에 부가 정보를 전달하고 싶을 때 JSON 포맷의 문자열을 설정할 수 있다<br/>
		 *
		 * (필수)서버군이 따로 없는 단일 서버군이라도 아래 예제대로 넣어야 함<br/>
		 * ex)"addtionalInfo":"{"server":"0","character":"0"}"
		 * 
		 * @param additionalInfo	(필수) JSON 포맷의 부가 정보 데이터 
		 * \~english You can set a string in JSON format when you want to expose a specific event page in the Promotion View or want to pass additional information to the Promotion Server.<br/>
		 *
		 * (Required) Even a single server group without a server group should be properly put in the following example<br/>
		 * ex)"addtionalInfo":"{"server":"0","character":"0"}"
		 * 
		 * @param additionalInfo	(Required) Additional information data in JSON format
		 * \~
		 * @ingroup Promotion
		 */
		public static void setAdditionalInfo(String additionalInfo) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "setAdditionalInfo", null);
			jsonParam.AddField ("setAdditionalInfo", additionalInfo);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		* \~korean  Engagement 이벤트 처리가 가능한지(게임 서버에 DB가 생성된 경우 등) 여부를 설정한다.
		* true로 설정하려는 경우, <로그인 이후 & 리스너가 등록된 이후>의 조건을 만족한 상태여야 정상적으로 설정되며,
		* false로 설정하려는 경우는 항상 설정 가능하다.
		*
		* @param bReady Enganement 이벤트 처리 가능 여부.
		* @return API 호출 성공 여부.
		 * \~english It sets whether Engagement event handling is enabled.(Such as when a DB is created in the game server)
		* If you want to set it to true, it must be in a state that satisfies the condition of <after login & after registering the listener>, 
		* if you want to set it to false, you can always set it.
		*
		* @param bReady Whether Engagement events can be processed.
		* @return Whether the API call was successful.
		 * \~
		 * @ingroup Promotion
		*/
		public static ResultAPI setEngagementReady(Boolean isReady) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "setEngagementReady", null);
			jsonParam.AddField ("isReady", isReady);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

			ResultAPI result = new ResultAPI(resJsonObject.GetField ("resultAPI"));
			return result;
		}

		/**
		 * \~korean  Engagement 리스너를 등록한다.
		 * \~english  It register the Engagement listener.
		 * \~
		 * @ingroup Promotion
		 * @param listener
		 */
		public static void setEngagementListener(onEngagement listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "setEngagementHandler", listener);
			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
    	 *  \~korean URI를 파싱하여 Event 타입으로 만든다.
    	 *
    	 * @param URI 파싱할 URI
    	 * @return 파싱에 문제가 없으면 true, 아니면 false.
    	 *  \~english Parse the URI to make it an Event type.
    	 *
    	 * @param URI URI to parse
    	 * @return True if there is no problem parsing, false otherwise.
		 * \~
		 * @ingroup Promotion
    	 */
		public static Boolean processURI(String uri) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "processURI", null);
			jsonParam.AddField ("URI", uri);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);

			Boolean result = false;
			resJsonObject.GetField (ref result, "processURI");
			return result;
		}

		/**
		* /~korean Promotion 정보 갱신
		* Promotion 새소식 페이지 등의 노출에 필요한 정보를 갱신한다.
		*
		* /~english Update information of Promotion
		* Update promotion data to show Promotion UI.
		* \~
		* @ingroup Promotion
		*/
		public static void updatePromotionData() {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "updatePromotionData", null);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		* \~korean UA를 통해 자신을 앱으로 최초 초대한 유저의 정보 반환 한다.
		* SDK 초기화, 로그인, setEngagementReady(true) 호출 이후 최초 초대자 정보 전달이 가능하다.
		* @param listener API 결과 통지 리스터
		*
		* \~english return first sender's information who shared UA invitation
		* First sender's information can be returned after SDK initialize, login and setEngagementReady(true)
		* @param listener API call result listener
		* \~
		* @ingroup Promotion
		*/
		public static void getAppInvitationSenderInfo(onAppInvitationSenderInfo listener) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Promotion", "getAppInvitationSenderInfo", listener);
			
            HIVEUnityPlugin.callNative(jsonParam);
		}
		public static EngagementEventType strToEngagementEventType(String strValue) {

			EngagementEventType engagementEventType = EngagementEventType.EVENT_TYPE;

			try {
				engagementEventType = (EngagementEventType) Enum.Parse(typeof(EngagementEventType), strValue, true);

			} catch (Exception) {
				
			}
			return engagementEventType;
		}

		public static EngagementEventState strToEngagementEventState(String strValue) {

			EngagementEventState engagementEventState = EngagementEventState.BEGIN;

			try {
				engagementEventState = (EngagementEventState) Enum.Parse(typeof(EngagementEventState), strValue, true);
				
			} catch (Exception) {
				
			}

			return engagementEventState;
		}

		// \internal
		// Native 영역에서 호출된 요청을 처리하기 위한 플러그인 내부 코드
		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			object handler = (object)HIVEUnityPlugin.popPromotionHandler (resJsonObject);

			if (handler == null) return;

			// JSONObject offerwallRewardJson = resJsonObject.GetField ("offerwallReward");
			// OfferwallReward offerwallReward = new OfferwallReward(offerwallRewardJson);

			if ("showPromotion".Equals (methodName) ||
			    "showCustomContents".Equals (methodName) ||
				"showCustomContentsOnGameWindow".Equals (methodName) ||
			    "showOfferwall".Equals (methodName) ||
				"showNews".Equals (methodName) ||
			    "showMoreGames".Equals (methodName) ||
				"showReview".Equals (methodName) ||
			    "showExit".Equals (methodName)) {
				if ("showCustomContents".Equals(methodName))
				{
					HIVEUnityPlugin.IMECompositionModeRestore();
				}
				String eventValue = "";
				resJsonObject.GetField (ref eventValue, "promotionEventType");

				PromotionEventType promotionEventType = PromotionEventType.OPEN;
				if ("CLOSE".Equals (eventValue.ToUpper ())) {
					promotionEventType = PromotionEventType.CLOSE;
				}
				else if ("START_PLAYBACK".Equals (eventValue.ToUpper ())) {
					promotionEventType = PromotionEventType.START_PLAYBACK;
				}
				else if ("FINISH_PLAYBACK".Equals (eventValue.ToUpper ())) {
					promotionEventType = PromotionEventType.FINISH_PLAYBACK;
				}
				else if ("EXIT".Equals (eventValue.ToUpper ())) {
					promotionEventType = PromotionEventType.EXIT;
				}
				else if ("GOBACK".Equals (eventValue.ToUpper ())) {
					promotionEventType = PromotionEventType.GOBACK;
				}

				onPromotionView listener = (onPromotionView)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), promotionEventType);
			}
			else if ("getViewInfo".Equals (methodName)) {

				List<PromotionViewInfo> viewInfoList = new List<PromotionViewInfo> ();

				JSONObject listJson = resJsonObject.GetField ("viewInfoList");
				if (listJson != null && listJson.count > 0) {

					List<JSONObject> jsonList = listJson.list;
					foreach (JSONObject jsonItem in jsonList) {

						PromotionViewInfo promotionViewInfo = new PromotionViewInfo (jsonItem);
						viewInfoList.Add (promotionViewInfo);
					}
				}

				onPromotionViewInfo listener = (onPromotionViewInfo)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), viewInfoList);
			}
			else if ("getBadgeInfo".Equals (methodName)) {

				List<PromotionBadgeInfo> badgeInfoList = new List<PromotionBadgeInfo> ();

				JSONObject listJson = resJsonObject.GetField ("badgeInfoList");
				if (listJson != null && listJson.count > 0) {

					List<JSONObject> jsonList = listJson.list;
					foreach (JSONObject jsonItem in jsonList) {

						PromotionBadgeInfo promotionBadgeInfo = new PromotionBadgeInfo (jsonItem);
						badgeInfoList.Add (promotionBadgeInfo);
					}
				}

				onPromotionBadgeInfo listener = (onPromotionBadgeInfo)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), badgeInfoList);
			}
            else if ("getBannerInfo".Equals(methodName) || 
					"getBannerInfoString".Equals(methodName)) {

                List<PromotionBannerInfo> bannerInfoList = new List<PromotionBannerInfo>();

                JSONObject listJson = resJsonObject.GetField("bannerInfoList");
                if (listJson != null && listJson.count > 0) {

                    List<JSONObject> jsonList = listJson.list;
                    foreach (JSONObject jsonItem in jsonList) {

                        PromotionBannerInfo promotionBannerInfo = new PromotionBannerInfo(jsonItem);

                        bannerInfoList.Add(promotionBannerInfo);
                    }
                }

                onPromotionBannerInfo listener = (onPromotionBannerInfo)handler;
                listener(new ResultAPI(resJsonObject.GetField("resultAPI")), bannerInfoList);
            }

			else if ("getAppInvitationData".Equals (methodName)) {

				AppInvitationData appInvitationData = new AppInvitationData (resJsonObject.GetField ("appInvitationData"));

				onAppInvitationData listener = (onAppInvitationData)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), appInvitationData);
			}
			else if ("setEngagementHandler".Equals (methodName)) {
				
				// object handler = HIVEUnityPlugin.getEngagementHandler();

				// Engagement Event Type
				String strValue = "";
				resJsonObject.GetField (ref strValue, "engagementEventType");
				EngagementEventType engagementEventType = strToEngagementEventType(strValue);

				resJsonObject.GetField (ref strValue, "engagementEventState");
				EngagementEventState engagementEventState = strToEngagementEventState(strValue);

				// Parameter by Engagement Event Type
				JSONObject param = resJsonObject.GetField ("param");
			
				onEngagement listener = (onEngagement)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), engagementEventType, engagementEventState, param);
			}
			else if ("showUAShare".Equals (methodName)) {
                onPromotionShare listener = (onPromotionShare)handler;
                listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("showNativeReview".Equals (methodName)) {
				return;	
			}
			else if ("updatePromotionData".Equals(methodName)) {
				return;
			}
			else if ("getAppInvitationSenderInfo".Equals(methodName)) {
				onAppInvitationSenderInfo listener = (onAppInvitationSenderInfo)handler;

				AppInvitationSenderInfo senderInfo = new AppInvitationSenderInfo(resJsonObject.GetField("senderInfo"));

				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), senderInfo);
				return;
			}
		}
	}




	/**
	 * \~korean  프로모션 뷰 창의 형태
	 * 
	 * \~english Types of Promotion view
	 * \~
	 * @ingroup Promotion
	 * @author ryuvsken
	 */
	public enum PromotionType {
		BANNERLEGACY = -1,  ///< \~korean 전면배너 (웹뷰)  \~english  Full Banner (WebView)
		BANNER              ///< \~korean 전면배너  \~english  Full Banner
    	, NEWS              ///< \~korean 새소식		\~english News
    	, NOTICE            ///< \~korean 공지사항		\~english Notice
	}

	/**
	 * \~korean  커스텀 컨텐츠의 형태
	 *
	 * \~english Types of Promotion CustomView
	 * \~
	 * @ingroup Promotion
	 * @author kjkim
	 */
	public enum PromotionCustomType
	{
		VIEW            ///< \~korean 커스텀 뷰	\~english  Customized View
		, BOARD         ///< \~korean 커스텀 보드	\~english  Customized Board
		, SPOT          ///< \~korean 스팟 배너	\~english  Spot Banner
		, DIRECT        ///< \~korean 다이렉트 캠페인	\~english  Direct Campaign Banner
		
	};


	/**
	 * \~korean  프로모션 뷰 결과 통지시 이벤트 형태
	 *
	 * \~english Promotion view event types
	 * \~
	 * @ingroup Promotion
	 * @author ryuvsken
	 */
	public enum PromotionEventType {

		OPEN					///< \~korean 프로모션 뷰 창이 열렸을 때	\~english  When the Promotion View window opens.
		, CLOSE					///< \~korean 프로모션 뷰 창이 닫혔을 때	\~english  When the Promotion View window is closed.
		, START_PLAYBACK		///< \~korean 영상 재생이 시작되었을 때	\~english  When Playback starts.
		, FINISH_PLAYBACK		///< \~korean 영상 재생이 완료되었을 때	\~english  When Playback is finished.
		, EXIT					///< \~korean 종료(더 많은 게임 보기) 팝업에서 종료하기를 선택했을 때	\~english  When you choose to quit from the Quit (see more games) popup.
		, GOBACK				///
	}

    /**
    * \~korean  롤링 배너 데이터 요청시 선택한 캠페인 타입
    *
    * \~english Campaign type selected before requesting rolling banner data.
    * \~
    * @author Seokjin Yong
    */
    public enum PromotionCampaignType
    {
        EVENT,
        NOTICE,
		ALL,
		CROSS
    };

    /**
     * \~korean  롤링 배너 데이터 요청시 선택한 배너 타입
     *
     * \~english Banner type selected before requesting rolling banner data.
     * \~
     * @author Seokjin Yong
     */
    public enum PromotionBannerType
    {
        GREAT,
        SMALL,
        ROLLING
    };


    /**
	 * \~korean  프로모션 뱃지 정보가 표시될 대상 정보<br/>
	 * 만약 NOTICE 이면 공지사항 관련 뱃지를 표시해야한다
	 * 
	 * \~english Information on which promotional badge is displayed<br/>
	 * If NOTICE, it should indicate the badge related to the notice.
	 * \~
	 * @ingroup Promotion
	 */
	public enum PromotionBadgeTarget {

		NEWS
		, NOTICE
		, CUSTOMVIEW
		, CUSTOMBOARD
	}


	/**
	 * \~korean  게임내 오퍼월(무료 충전소) 을 호출 할 수 있는 버튼 노출 가능 상태 정보 
	 * 
	 * \~english Information of Offerwall(free charging station) button
	 * \~
	 * @ingroup Promotion
	 */
	public enum OfferwallState {

		ENABLED					///< \~korean 게임내 무료 충전 버튼 노출 가능	\~english  Offerwall Button can be exposed.
		, DISABLED				///< \~korean 게임내 무료 충전 버튼 노출 불가능	\~english  Offerwall Button can not be exposed.
		, UNKNOWN				///< \~korean 상태를 알 수 없음	\~english  Status unknown.

	}




	/**
	 * \~korean  Promotion 컨텐츠 화면 내용<br>
	 * (HIVE 프로모션 웹 뷰의 UI 를 게임 UI 의 컨셉에 맞추기 위해서 프로모션 웹 뷰를 게임에서 직접 표시하기 위한 정보)
	 * 
	 * \~english Information of Promotion webview<br>
	 * (Information to customize your promotion Webview UI)
	 * \~
	 * @ingroup Promotion
	 */
	public class PromotionViewInfo {

		/**
		 * \~korean 프로모션 뷰를 로드하기 위한 웹뷰의 정보
		 * \~english Information to customize your promotion Webview UI
		 */
		 public String url;
		 public String postString;
		 
		public PromotionViewInfo(String url, String postString)
		{
			this.url = url;
			this.postString = postString;
		}

		public PromotionViewInfo(JSONObject resJsonParam) {

			if (resJsonParam == null || resJsonParam.count <= 0)
				return;

			resJsonParam.GetField (ref this.url, "url");
			resJsonParam.GetField (ref this.postString, "postString");
		}

		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("PromotionViewInfo { url = ");
			sb.Append(this.url);
			sb.Append(", postString = ");
			sb.Append(this.postString);
			sb.Append (" }\n");
			sb.Append (" }\n");

			return sb.ToString();
		}
	}


	/**
	 * \~korean  유저에게 새로운 프로모션 내용이 등록 되었음을 알려주기 위한 정보
	 * 
	 * \~english Badge information to inform users that new promotions are registered
	 * \~
	 * @ingroup Promotion
	 * @author Joosang Kang
	 */
	public class PromotionBadgeInfo {

		/**
		 * \~korean 뱃지를 표시해줘야하는 타겟<br/>
		 * 예를 들어서 NOTICE 이면 공지사항 관련 뱃지를 표시해야한다<br/>
		 * (NEWS, NOTICE, CUSTOMVIEW, CUSTOMBOARD 이 올 수 있다.)
		 * \~english Target to mark badge<br/>
		 * For example, if NOTICE, you should mark the badge associated with the notice<br/>
		 * (Can be FULLBANNER, EVENT, NOTICE, CUSTOM )
		 */
		public PromotionBadgeTarget target = PromotionBadgeTarget.CUSTOMBOARD;

		public String contentsKey;			///< \~korean target 이 커스텀 컨텐츠일 경우 백오피스에 등록된 프로모션 고유 ID

		public String badgeType;		///< \~korean 뱃지 종류 ("new" or "none" 이 올 수 있다)

		public PromotionBadgeInfo(JSONObject resJsonParam) {

			if (resJsonParam == null || resJsonParam.count <= 0)
				return;

			String target = "CUSTOMBOARD";
			resJsonParam.GetField (ref target, "target");
			if ("NEWS".Equals (target))
				this.target = PromotionBadgeTarget.NEWS;
			else if ("NOTICE".Equals (target))
				this.target = PromotionBadgeTarget.NOTICE;
			else if ("CUSTOMVIEW".Equals (target))
				this.target = PromotionBadgeTarget.CUSTOMVIEW;
			else
				this.target = PromotionBadgeTarget.CUSTOMBOARD;

			resJsonParam.GetField (ref this.contentsKey, "contentsKey");
			resJsonParam.GetField (ref this.badgeType, "badgeType");
		}

		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append("PromotionBadgeInfo { target = ");
			sb.Append(this.target.ToString());
			sb.Append(", contentsKey = ");
			sb.Append(this.contentsKey);
			sb.Append(", badgeType = ");
			sb.Append(this.badgeType);
			sb.Append (" }\n");

			return sb.ToString();
		}
	}

	/**
	* \~korean 특정 프로모션에 대한 배너 정보
	*
	* \~english Banner information for a specific promotion
	* \~
	* @ingroup Promotion
	*/
    public class PromotionBannerInfo
    {
        public int pid = 0;            ///< \~korean 프로모션 ID \~english Promotion ID
        public String imageUrl;         ///< \~korean 이미지 URL \~english Image Url
        public String linkUrl;          ///< \~korean 배너 클릭 시 이동 URL \~english Banner click Url
        public String displayStartDate; ///< \~korean "2016-11-01 10:00:00" \~english "2016-11-01 10:00:00"
        public String displayEndDate;   ///< \~korean "2016-11-31 10:00:00" \~english "2016-11-31 10:00:00"
        public long utcStartDate = 0;   ///< \~korean 프로모션 시작 시간 (Unixtimestamp) \~english Promotion start time (Unixtimestamp)
        public long utcEndDate = 0;     ///< \~korean 프로모션 종료 시간 (Unixtimestamp) \~english Promotion end time (Unixtimestamp)
        public String typeLink;         ///< \~korean "webview", "webbrowser", "market", "notice", "text", "none" \~english "webview", "webbrowser", "market", "notice", "text", "none"
        public String typeBanner;       ///< \~korean "great", "small", "rolling" \~english "great", "small", "rolling"
		public String typeCampaign;			///< \~korean 프로모션 캠페인 타입 "all", "event", "notice", "permit" \~english Promotion campaign type "all", "event", "notice", "permit"
		public String interworkData;	///< \~korean 롤링배너 클릭 시 게임의 특정위치로 이동하기 위해 게임에서 입력한 인터워크 정보"

		public PromotionBannerInfo() {}
        public PromotionBannerInfo(JSONObject resJsonParam)
        {
            if (resJsonParam == null || resJsonParam.count <= 0)
                return;

			resJsonParam.GetField(ref pid, "pid");
			resJsonParam.GetField(ref imageUrl, "imageUrl");
			resJsonParam.GetField(ref linkUrl, "linkUrl");
			resJsonParam.GetField(ref displayStartDate, "displayStartDate");
			resJsonParam.GetField(ref displayEndDate, "displayEndDate");
			resJsonParam.GetField(ref utcStartDate, "utcStartDate");
			resJsonParam.GetField(ref utcEndDate, "utcEndDate");
			resJsonParam.GetField(ref typeLink, "typeLink");
			resJsonParam.GetField(ref typeBanner, "typeBanner");
			resJsonParam.GetField(ref typeCampaign, "typeCampaign");
			resJsonParam.GetField(ref interworkData, "interworkData");

			interworkData = interworkData.ToLower().Equals("null") ? "" : interworkData;
        }

        public String toString()
        {
			StringBuilder sb = new StringBuilder();

            sb.Append("PromotionBannerInfo = {");
            sb.Append("pid = " + pid + '\n');
			sb.Append("imageUrl = " + imageUrl + '\n');
			sb.Append("linkUrl = " + linkUrl + '\n');
			sb.Append("displayStartDate = " + displayStartDate + '\n');
			sb.Append("displayEndDate = " + displayEndDate + '\n');
			sb.Append("utcStartDate = " + utcStartDate + '\n');
			sb.Append("utcEndDate = " + utcEndDate + '\n');
			sb.Append("typeLink = " + typeLink + '\n');
			sb.Append("typeBanner = " + typeBanner + '\n');
			sb.Append("typeCampaign = " + typeCampaign + '\n');
			sb.Append("interworkData = " + interworkData + '\n');
            sb.Append(" }\n");

            return sb.ToString();
        }
    }


	/**
	 * \~korean  앱 초대 (UserAcquisition)를 위한 정보.
	 * 
	 * @see Promotion.getAppInvitationData()
	 *
	 * \~english Invite information for UserAcquisition.
	 * 
	 * @see Promotion.getAppInvitationData()
	 * \~
	 * @ingroup Promotion
	 */
	public class AppInvitationData {

		public byte[] qrcode;										///< \~korean 앱의 초대 정보가 포함된 QR Code 이미지 데이터 \~english QR Code image data with app invite information
		public String inviteCommonLink;								///< \~korean 기본 초대 링크 \~english Default invite link
		public String inviteHivemsgLink;							///< \~korean HIVE 인증 사용자용 초대 링크 \~english Invite link for HIVE certified users.
		public String inviteFacebookLink;							///< \~korean Facebook 인증 사용자용 초대 링크 \~english Invite link for Facebook certified users.
		public List<AppInvitationCampaign> 		eachCampaignList;	///< \~korean 캠페인 완료 발생시, 매번 보상을 지급하는 캠페인 목록 \~english List of campaigns that will be rewarded every time when a campaign is completed.
		public List<AppInvitationCampaignStage> stageCampaignList;	///< \~korean 캠페인 완료 목표 초대 수를 달성했을 때 보상을 지급하는 캠페인 목록 \~english List of campaigns that will be rewarded when you reach the campaign completion goal of invites.
		public String inviteMessage;								///< \~korean 초대 문구 (HIVE SDK v4.11.4+) \~english Invite message (HIVE SDK v4.11.4+)


		public AppInvitationData(JSONObject jsonParam) {

			if (jsonParam == null || jsonParam.count <= 0)
				return;

			String hexString = null;
			jsonParam.GetField (ref hexString, "qrcode");

			if (hexString != null) {
				this.qrcode = hexToByteArray (hexString);
			}

			jsonParam.GetField (ref this.inviteCommonLink, "inviteCommonLink");
			jsonParam.GetField (ref this.inviteHivemsgLink, "inviteHivemsgLink");
			jsonParam.GetField (ref this.inviteFacebookLink, "inviteFacebookLink");
			jsonParam.GetField (ref this.inviteMessage, "inviteMessage");

			this.eachCampaignList = createCampaignList(jsonParam.GetField("eachCampaignList"));
			this.stageCampaignList = createCampaignStageList(jsonParam.GetField("stageCampaignList"));
			jsonParam.GetField (ref this.inviteFacebookLink, "inviteFacebookLink");

		}

		List<AppInvitationCampaign> createCampaignList(JSONObject jsonArray) {

			List<AppInvitationCampaign> arrayOfCampaign = new List<AppInvitationCampaign>();

			if (jsonArray == null || jsonArray.count <= 0)
				return arrayOfCampaign;

			List<JSONObject> jsonList = jsonArray.list;
			foreach (JSONObject jsonItem in jsonList) {

				AppInvitationCampaign newObject = new AppInvitationCampaign (jsonItem);
				arrayOfCampaign.Add (newObject);
			}

			return arrayOfCampaign;
		}

		List<AppInvitationCampaignStage> createCampaignStageList(JSONObject jsonArray) {

			List<AppInvitationCampaignStage> arrayOfCampaignStage = new List<AppInvitationCampaignStage>();

			if (jsonArray == null || jsonArray.count <= 0)
				return arrayOfCampaignStage;

			List<JSONObject> jsonList = jsonArray.list;
			foreach (JSONObject jsonItem in jsonList) {

				AppInvitationCampaignStage newObject = new AppInvitationCampaignStage (jsonItem);
				arrayOfCampaignStage.Add (newObject);
			}

			return arrayOfCampaignStage;
		}

		byte[] hexToByteArray(String hex) {
			
			int NumberChars = hex.Length;
			byte[] bytes = new byte[NumberChars / 2];
			for (int i = 0; i < NumberChars; i += 2) {
				bytes [i / 2] = Convert.ToByte (hex.Substring(i, 2), 16);
			}
			return bytes;
		}

		String byteArrayToHex(byte[] data) {

			StringBuilder hex = new StringBuilder(data.Length * 2);
			foreach (byte b in data)
				hex.AppendFormat("{0:x2}", b);
			
			return hex.ToString();
		}

		public JSONObject toJson() {

			JSONObject resJson = new JSONObject();
			resJson.AddField("inviteCommonLink", this.inviteCommonLink);
			resJson.AddField("inviteHivemsgLink", this.inviteHivemsgLink);
			resJson.AddField("inviteFacebookLink", this.inviteFacebookLink);
			resJson.AddField("qrcode", this.byteArrayToHex(qrcode));
			resJson.AddField("inviteMessage", this.inviteMessage);

			JSONObject eachCampaignJsonArray = new JSONObject();
			foreach(AppInvitationCampaign each in this.eachCampaignList) {
				eachCampaignJsonArray.Add (each.toJson());
			}
			resJson.AddField ("eachCampaignList", eachCampaignJsonArray);

			JSONObject stateCampaignJsonArray = new JSONObject ();
			foreach (AppInvitationCampaignStage each in this.stageCampaignList) {
				stateCampaignJsonArray.Add (each.toJson());
			}
			resJson.AddField ("stageCampaignList", stateCampaignJsonArray);

			return resJson;
		}

		public String toString() {

			StringBuilder sb = new StringBuilder();
			sb.Append("AppInvitationData ").Append(this.toJson().ToString()).Append("\n");
			return sb.ToString();
		}
	}


	/**
	 * \~korean  앱 초대를 위한 캠패인 정보<br>
	 * 캠페인은 초대에 대한 보상을 달상하기 위한 조건을 명시한 데이터.
	 * 
	 * @see AppInvitationData
	 * 
	 * \~english Campaign information for invite<br>
	 * Campaigns are data that specifies the conditions for rewarding invite.
	 * 
	 * @see AppInvitationData
	 * \~
	 * @ingroup Promotion
	 */
	public class AppInvitationCampaign {

		public String title;			///< \~korean 캠페인 타이틀 \~english Title
		public String description;		///< \~korean 캠페인 설명 \~english Description
		public String imageUrl;			///< \~korean 캠페인 이미지 URL \~english Image URL
		public int order;				///< \~korean 캠페인 순서 \~english Order
		public JSONObject item;			///< \~korean 완료 보상 정보 ("key" 는 캠페인 설정에 따라 상이함) \~english Complete reward information ("key" depends on campaign settings)
		public int count;				///< \~korean 초대 인원 중. 캠페인을 완료한 수 \~english Number of invitees who completed the campaign
		public int limit;				///< \~korean 캠페인 최대 인원 수 \~english Maximum number of user of campaigns

		public AppInvitationCampaign(JSONObject jsonParam) {

			if(jsonParam == null || jsonParam.count <= 0)
				return;
			jsonParam.GetField("title", value => { this.title = value.stringValue; }, name => { this.title = ""; });
			jsonParam.GetField("description", value => { this.description = value.stringValue; }, name => { this.description = ""; });
			jsonParam.GetField("imageUrl", value => { this.imageUrl = value.stringValue; }, name => { this.imageUrl = ""; });   //	string Type
            jsonParam.GetField("order", value => { this.order = (int)value.floatValue; }, name => { this.order = 0; });            // double Type
			jsonParam.GetField("item", value => { this.item = value; } , name => { this.item = null; } );                       //	JSONOBJECT type.
            jsonParam.GetField("count", value => { this.count = (int)value.floatValue; }, name => { this.count = 0; });
            jsonParam.GetField("limit", value => { this.limit = (int)value.floatValue; }, name => { this.limit = 0; });

		}

		public virtual JSONObject toJson() {

			JSONObject resJson = new JSONObject();

			resJson.AddField("title", this.title);
			resJson.AddField("description", this.description);
			resJson.AddField("imageUrl", this.imageUrl);
			resJson.AddField("order", this.order);
			resJson.AddField("item", this.item);
			resJson.AddField("count", this.count);
			resJson.AddField("limit", this.limit);
			return resJson;
		}

		public virtual String toString() {

			StringBuilder sb = new StringBuilder();
			sb.Append("AppInvitationCampaign ").Append(this.toJson().ToString()).Append("\n");
			return sb.ToString();
		}
	}

	public class AppInvitationCampaignStage : AppInvitationCampaign {

		public int goalCount;		///< \~korean 목표 달성 정보(모든 단계를 완료한 인원 수) \~english Goal achievement information (the number of player who completed the all stage)
		public int goalTotal;		///< \~korean 목표 달성 정보(보상을 받기 위해 모든 단계를 완료해야 하는 인원 수) \~english Goal achievement information (the number of player required to complete the all stage to get rewards)

		public AppInvitationCampaignStage(JSONObject jsonParam) 
		: base(jsonParam) {

			if (jsonParam == null || jsonParam.count <= 0)
				return;
			jsonParam.GetField("goalCount", value => { this.goalCount = (int)value.floatValue; }, name => { this.goalCount= 0; });
			jsonParam.GetField("goalTotal", value => { this.goalTotal = (int)value.floatValue; }, name => { this.goalTotal= 0; });
		}

		public override JSONObject toJson() {

			JSONObject resJson = base.toJson();

			resJson.AddField("goalCount", this.goalCount);
			resJson.AddField("goalTotal", this.goalTotal);

			return resJson;
		}

		public override String toString() {

			StringBuilder sb = new StringBuilder();
			sb.Append("AppInvitationCampaignStage ").Append(this.toJson().ToString()).Append("\n");
			return sb.ToString();
		}
	}

	/**
	* \~korean  UA를 통해 자신을 앱으로 최초 초대한 유저의 정보
	*
	* @see getAppInvitationSenderInfo(onAppInvitationSenderInfo)
	*
	* \~english  First sender's userInfo who sent UA share invitation
	*
	* @see getAppInvitationSenderInfo(onAppInvitationSenderInfo)
	* \~
	* @ingroup Promotion
	*/
	public class AppInvitationSenderInfo {
		public string vid;

		public AppInvitationSenderInfo(JSONObject jsonParam) {

			if(jsonParam == null || jsonParam.count <= 0)
				return;
			
			jsonParam.GetField(ref this.vid, "vid");
		}

		public JSONObject toJson() {

			JSONObject resJson = new JSONObject();

			resJson.AddField("vid", this.vid);

			return resJson;
		}

		public String toString() {

			StringBuilder sb = new StringBuilder();
			sb.Append("AppInvitationSenderInfo ").Append(this.toJson().ToString()).Append("\n");
			return sb.ToString();
		}
	}

	public enum EngagementEventType {
		EVENT_TYPE				/// < \~korean Engagement의 전체 시작과 끝을 알리는 경우. \~english Notifying the beginning and end of the engagement.
		
		, PROMOTION_VIEW  		///< \~korean Engagement에 의해 처리되는 PromotionView인 경우.  \~english PromotionView handled by Engagement.
		, OFFERWALL_VIEW
		, USERACQUISTION
		, COUPON                  ///<  \~korean Engagement에 의해 처리된 쿠폰 소모에 대한 결과.  \~english Results for consumption of coupons processed by Engagement.

		, AUTH_LOGIN_VIEW          ///<  \~korean 유저(클라이언트)에 의해 열리지 않은 로그인 프로세스에 의한 결과를 받는 콜백.  \~english  A callback that receives the results of a login process which is not opened by the user (client).

		, SOCIAL_INQUIRY_VIEW

		, EVENT                     ///<  \~korean Engagement에 의해 처리될 수 없는 이벤트(host가 game인 경우)를 전달해주는 콜백.  \~english   Callback that delivers events that can not be handled by Engagement (for example, the host is game).

		, IAP_UPDATED				/// IAP
		, IAP_PURCHASE   			 ///<  \~korean 아이템 구매 시도시 불리게 되는 콜백.  \~english  Callbacks that are called when an item is purchased.
		, IAP_PROMOTE				///<  \~korean 앱이 시작 혹은 실행 중일 때, 앱 외에서 상품을 구매시도시 불리게 되는 콜백. 혹은 Interwork를 통해 아이템 구매 시도시 아이템 정보를 전달하는 콜백 . \~english  Callbacks that are called when trying to buy items outside of the app or a callback that delivers item information when an item is purchased through an interwork.
		, COMPANION					/// Promotion CPA Link recevied. SDK Will be send promotion companion.
		, SOCIAL_MYINQUIRY_VIEW		///< 내 문의 내역을 오픈
		, SOCIAL_PROFILE_VIEW		///< 프로필 페이지 오픈
		, COMMUNITY_VIEW			///< 커뮤니티 페이지 오픈
		, APPUPDATE_DOWNLOADED      ///< In-App Update Downloaded.
	}



	public enum EngagementEventState {
		BEGIN
		, FINISH
		, START				///<  \~korean Engagement에 의해 다른 기능이 수행되기 시작함을 알림.  \~english Notice that other functions are started by Engagement.
		, END					///<  \~korean Engagement에 의한 다른 기능 수행이 종료됨을 알림.  \~english Notice that other functions performed by Engagement are terminated.
	}
}


/** @} */



