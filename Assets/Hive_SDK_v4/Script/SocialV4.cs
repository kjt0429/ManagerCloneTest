/**
 * @file    SocialV4.cs
 * 
 *  @date		2016-2022
 *  @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 *  @author		disker
 *  @since		4.15.4
 */
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @defgroup	SocialV4
 */
namespace hive
{
	/**
	 *
	 * @author disker
	 * @since		4.15.4
	 * @ingroup SocialV4
	 *
	 */
	public class SocialV4 {

		/**
		*  \~korean
		* @brief SocialV4 API 호출 타입
		*
		*  \~english
		* @brief SocialV4 API call type
		*/
		public enum ProviderType {
			HIVE
			, FACEBOOK
		}

		/**
		*  \~korean
		* @brief SocialV4 API 호출 타입
		*
		*  \~english
		* @brief SocialV4 API call type
		*/
		public enum ViewType {
			FullScreen
			, Frame
		}

		/**
		*  \~korean
		* @brief 커뮤니티 페이지 요청 결과 통지.
		* result : API 호출 결과.<br>
		*
		*  \~english
		* @brief Community page request result callback
		* result : Result of API.<br/>
		*
		*  \~
		* @ingroup SocialV4
		*
		*/
		public delegate void onShowCommunity(ResultAPI result);

		/**
		*  \~korean
		* @brief 사진 컨텐츠 공유 결과 통지.
		* result : API 호출 결과.<br>
		*
		*  \~english
		* @brief Photo content sharing result callback
		* result : Result of API.<br/>
		*
		*  \~
		* @ingroup SocialV4
		*
		*/
		public delegate void onSharePhoto(ResultAPI result);


		/**
		*  \~korean
		* @brief 커뮤니티 페이지 요청 API.
		* @param providerType 요청 타입
		* @param listener showCommunity 결과 통지
		*
		*  \~english
		* @brief 커뮤니티 페이지 요청 API.
		* @param providerType request type
		* @param listener showCommunity result callback
		*
		*  \~
		* @ingroup SocialV4
		*
		*/
		public static void showCommunity(ProviderType providerType, onShowCommunity listener) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialV4", "showCommunity", listener);
			jsonParam.AddField("providerType", providerType.ToString());

			HIVEUnityPlugin.callNative (jsonParam);
			HIVEUnityPlugin.IMECompositionModeOn();
		}


		/**
		*  \~korean
		* @brief 커뮤니티 페이지 요청 API.
		* @param providerType 요청 타입
		* @param viewType 뷰 타입
		* @param listener showCommunity 결과 통지
		*
		*  \~english
		* @brief 커뮤니티 페이지 요청 API.
		* @param providerType request type
		* @param viewType view type
		* @param listener showCommunity result callback
		*
		*  \~
		* @ingroup SocialV4
		*
		*/
		public static void showCommunity(ProviderType providerType, ViewType viewType, onShowCommunity listener) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialV4", "showCommunity", listener);
			jsonParam.AddField("providerType", providerType.ToString());
			jsonParam.AddField("viewType", viewType.ToString());

			HIVEUnityPlugin.callNative (jsonParam);
			HIVEUnityPlugin.IMECompositionModeOn();
		}


		/**
		*  \~korean
		* @brief 사진 컨텐츠 공유 API.
		* @param providerType 요청 타입
		* @param listener sharePhotoContent 결과 통지
		*
		*  \~english
		* @brief Photo Content Share API.
		* @param providerType request type
		* @param listener sharePhotoContent result callback
		*
		*  \~
		* @ingroup SocialV4
		*
		*/
		public static void sharePhoto(ProviderType providerType, onSharePhoto listener) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("SocialV4", "sharePhoto", listener);
			jsonParam.AddField("providerType", providerType.ToString());

			HIVEUnityPlugin.callNative (jsonParam);
		}


        // Native 영역에서 호출된 요청을 처리하기 위한 플러그인 내부 코드
        public static void executeEngine(JSONObject resJsonObject)
        {
            String methodName = null;
            resJsonObject.GetField(ref methodName, "method");

            int handlerId = -1;
            resJsonObject.GetField(ref handlerId, "handler");

            object handler = (object)HIVEUnityPlugin.popHandler(handlerId);

            if (handler == null) return;

            switch (methodName)
            {
                case "showCommunity":
					HIVEUnityPlugin.IMECompositionModeRestore();
					onShowCommunity onShowCommunityResult = (onShowCommunity)handler;
                    onShowCommunityResult(new ResultAPI(resJsonObject.GetField("resultAPI")));
                    break;
				case "sharePhoto":
					onSharePhoto onSharePhotoResult = (onSharePhoto)handler;
					onSharePhotoResult(new ResultAPI(resJsonObject.GetField("resultAPI")));
					break;
                default:
                    break;
            }
        }
		
	}
}

/** @} */