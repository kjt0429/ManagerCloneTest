/**
 * @file    ProviderGoogle.cs
 * 
 *  @date		2016-2022
 *  @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 *  @author		hife
 *  @since		4.3.0 
 */
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

/**
 *  @defgroup	Provider
 *  @defgroup	ProviderGoogle
 *  @ingroup 	Provider
 *  @addtogroup ProviderGoogle
 *  @{
 *  \~korean
 * Google Play Games 의 기능 중 업적과 리더보드를 사용할 수 있다.<br/>
 * 연결되어있지 않을 경우 API 를 호출 할 때 자동으로 연결을 시도 한다.<br/>
 * AuthV4 의 Connect 와는 무관하게 독립적으로 사용할 수 있다.<br/>
 * 자동 연결이 되어도 현재 playerid 와 Connect 되는것은 아니다.
 *
 *  \~english
 * You can use achievements and leaderboards within Google Play Games.<br/>
 * If it is not connected to Google Play Games, it will try to connect automatically when calling API.<br/>
 * It can be used independently regardless of AuthV4 Connect.<br/>
 * Even if automatic connection is made, it is not connected with the current playerId.
 * 
 *  \~
 *
 * Created by hife on 2017. 6. 7.
 *
 * @author hife
 * @since		4.3.0 
 */

namespace hive
{
    /**
	 *  \~korean
	 * Google Play Games 의 기능 중 업적과 리더보드를 사용할 수 있다.<br/>
	 * 연결되어있지 않을 경우 API 를 호출 할 때 자동으로 연결을 시도 한다.<br/>
	 * AuthV4 의 Connect 와는 무관하게 독립적으로 사용할 수 있다.<br/>
	 * 자동 연결이 되어도 현재 playerid 와 Connect 되는것은 아니다.
	 *
	 *  \~english
	 * You can use achievements and leaderboards within Google Play Games.<br/>
	 * If it is not connected to Google Play Games, it will try to connect automatically when calling API.<br/>
	 * It can be used independently regardless of AuthV4 Connect.<br/>
	 * Even if automatic connection is made, it is not connected with the current playerId.
	 * 
 	 *  \~
 	 *
	 * Created by hife on 2017. 6. 7.
	 * 
	 * @author hife
	 * @since		4.3.0 
	 * @ingroup ProviderGoogle
	 */
    public class ProviderGoogle
    {

        public delegate void onAchievementsResult(ResultAPI resultAPI);
        public delegate void onLeaderboardsResult(ResultAPI resultAPI);
		public delegate void onGooglePlayerIdResult(ResultAPI resultAPI, String googlePlayerId, String authCode);

        /**
		 *  \~korean 
		 * 숨겨진 업적을 공개한다.<br/>
		 * 업적이 0% 로 공개만 될 뿐 달성 되지는 않는다.<br/>
		 *
		 * @param achievementId 공개할 achievementId
		 * 
		 *  \~english
		 * It show hidden achievements.<br/>
		 * Achievements are only revealed at 0%, not achieved.<br/>
		 *
		 * @param achievementId achievementId to reveal
		 *  
		 *  \~
		 * @ingroup ProviderGoogle
		 */
        public static void achievementsReveal(String achievementId)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderGoogle", "achievementsReveal", null);
            jsonParam.AddField("achievementId", achievementId);

            JSONObject resJsonObject = HIVEUnityPlugin.callNative(jsonParam);
        }

        /**
		 *  \~korean 
		 * 숨겨진 업적을 공개한다.<br/>
		 * 업적이 0% 로 공개만 될 뿐 달성 되지는 않는다.<br/>
		 *
		 * @param achievementId 공개할 achievementId
 		 * @param listener GoogleAchievementsListener
		 * 
		 *  \~english
		 * It show hidden achievements.<br/>
		 * Achievements are only revealed at 0%, not achieved.<br/>
		 *
		 * @param achievementId achievementId to reveal
		 * @param listener GoogleAchievementsListener
		 * 
		 *  \~
		 * @ingroup ProviderGoogle
		 */
        public static void achievementsReveal(String achievementId, onAchievementsResult listener)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderGoogle", "achievementsReveal", listener);
            jsonParam.AddField("achievementId", achievementId);

            JSONObject resJsonObject = HIVEUnityPlugin.callNative(jsonParam);
        }

        /**
		 *  \~korean
		 * 업적을 달성한다.<br/>
		 * 숨겨져 있거나 공개된 여부와 상관없이 업적이 100% 로 달성 된다.<br/>
		 *
		 * @param achievementId 공개할 achievementId
		 * 
		 *  \~english
		 * It achieve achievements.<br/>
		 * Whether hidden or open, achievement is achieved at 100%.<br/>
		 *
		 * @param achievementId achievementId to achieve
		 *
		 *  \~
		 * @ingroup ProviderGoogle
		 */
        public static void achievementsUnlock(String achievementId)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderGoogle", "achievementsUnlock", null);
            jsonParam.AddField("achievementId", achievementId);

            JSONObject resJsonObject = HIVEUnityPlugin.callNative(jsonParam);
        }

        /**
		 *  \~korean
		 * 업적을 달성한다.<br/>
		 * 숨겨져 있거나 공개된 여부와 상관없이 업적이 100% 로 달성 된다.<br/>
		 *
		 * @param achievementId 공개할 achievementId
		 * @param listener GoogleAchievementsListener
		 * 
		 *  \~english
		 * It achieve achievements.<br/>
		 * Whether hidden or open, achievement is achieved at 100%.<br/>
		 *
		 * @param achievementId achievementId to achieve
		 * @param listener GoogleAchievementsListener
		 *
		 *  \~
		 * @ingroup ProviderGoogle
		 */
        public static void achievementsUnlock(String achievementId, onAchievementsResult listener)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderGoogle", "achievementsUnlock", listener);
            jsonParam.AddField("achievementId", achievementId);

            JSONObject resJsonObject = HIVEUnityPlugin.callNative(jsonParam);
        }

        /**
		 *  \~korean
		 * 업적 수치를 증가 시킨다.<br/>
		 * value 만큼 설정이 아닌 매 value 만큼 합산 된다.<br/>
		 * 총 합산이 Max 가 될 경우 자동으로 업적이 달성되며, 계속 호출하여도 무방하다.<br/>
		 *
		 *  \~english
		 * It increases achievement figures.<br/>
		 * Achievement figures is added as much as value set by the API call, not by setting.<br/>
		 * If the total sum is Max, the achievement is automatically accomplished.<br/>
		 *
		 *  \~
		 * @param incrementalAchievementId incrementalAchievementId
		 * @param value value
		 * @ingroup ProviderGoogle
		 */
        public static void achievementsIncrement(String incrementalAchievementId, int value)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderGoogle", "achievementsIncrement", null);
            jsonParam.AddField("incrementalAchievementId", incrementalAchievementId);
            jsonParam.AddField("value", value);

            JSONObject resJsonObject = HIVEUnityPlugin.callNative(jsonParam);
        }

        /**
		 *  \~korean
		 * 업적 수치를 증가 시킨다.<br/>
		 * value 만큼 설정이 아닌 매 value 만큼 합산 된다.<br/>
		 * 총 합산이 Max 가 될 경우 자동으로 업적이 달성되며, 계속 호출하여도 무방하다.<br/>
		 *
		 *  \~english
		 * It increases achievement figures.<br/>
		 * Achievement figures is added as much as value set by the API call, not by setting.<br/>
		 * If the total sum is Max, the achievement is automatically accomplished.<br/>
		 *
		 *  \~
		 * @param incrementalAchievementId incrementalAchievementId
		 * @param value value
		 * @param listener GoogleAchievementsListener
		 * @ingroup ProviderGoogle
		 */
        public static void achievementsIncrement(String incrementalAchievementId, int value, onAchievementsResult listener)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderGoogle", "achievementsIncrement", listener);
            jsonParam.AddField("incrementalAchievementId", incrementalAchievementId);
            jsonParam.AddField("value", value);

            JSONObject resJsonObject = HIVEUnityPlugin.callNative(jsonParam);
        }

        /**
		 *  \~korean
		 * Google 업적 UI 를 띄운다.<br/>
		 *
		 *  \~english
		 * It shows the Google achievement UI.<br/> 
		 *
		 *  \~
		 * @param listener GoogleAchievementsListener
		 * @ingroup ProviderGoogle
		 */
        public static void showAchievements(onAchievementsResult listener)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderGoogle", "showAchievements", listener);

            HIVEUnityPlugin.callNative(jsonParam);
        }

        /**
		 *  \~korean
		 * 리더보드 점수를 갱신한다.<br/>
		 * leaderboardId 에 해당하는 기록에 score 수치로 갱신된다.<br/>
		 *
		 *  \~english
		 * It update the leaderboard score.<br/>
		 * The score corresponding to the leaderboardId is updated with the score value.<br/>
		 *
		 *  \~
		 * @param leaderboardId leaderboardId
		 * @param score score
		 * @ingroup ProviderGoogle
		 */
        public static void leaderboardsSubmitScore(String leaderboardId, long score)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderGoogle", "leaderboardsSubmitScore", null);
            jsonParam.AddField("leaderboardId", leaderboardId);
            jsonParam.AddField("score", score);

            JSONObject resJsonObject = HIVEUnityPlugin.callNative(jsonParam);
        }

        /**
		 *  \~korean
		 * 리더보드 점수를 갱신한다.<br/>
		 * leaderboardId 에 해당하는 기록에 score 수치로 갱신된다.<br/>
		 *
		 *  \~english
		 * It update the leaderboard score.<br/>
		 * The score corresponding to the leaderboardId is updated with the score value.<br/>
		 *
		 *  \~
		 * @param leaderboardId leaderboardId
		 * @param score score
		 * @param listener GoogleLeaderboardsListener
		 * @ingroup ProviderGoogle
		 */
        public static void leaderboardsSubmitScore(String leaderboardId, long score, onLeaderboardsResult listener)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderGoogle", "leaderboardsSubmitScore", listener);
            jsonParam.AddField("leaderboardId", leaderboardId);
            jsonParam.AddField("score", score);

            JSONObject resJsonObject = HIVEUnityPlugin.callNative(jsonParam);
        }

        /**
		 *  \~korean
		 * Google 리더보드 UI 를 띄운다.<br/>
		 *
		 *  \~english
		 * It shows Google Leaderboard UI.
		 *
		 *  \~
		 * @param listener GoogleLeaderboardsListener
		 * @ingroup ProviderGoogle
		 */
        public static void showLeaderboards(onLeaderboardsResult listener)
        {
            JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderGoogle", "showLeaderboards", listener);

            HIVEUnityPlugin.callNative(jsonParam);
        }

		/**
		 *  \~korean
		 * Google Games Player ID를 반환한다.
		 *  \~
		 * @ingroup ProviderGoogle
		 */
		public static void getGooglePlayerId(onGooglePlayerIdResult listener) {

#if !UNITY_EDITOR && UNITY_ANDROID
			JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderGoogle", "getGooglePlayerId", listener);

			HIVEUnityPlugin.callNative (jsonParam);
#else
			ResultAPI result = new ResultAPI();
			result.errorCode = ResultAPI.ErrorCode.NOT_SUPPORTED;
			result.code = ResultAPI.Code.AuthV4GoogleNotSupported;

			listener (result, "", "");
#endif
		}

		private static string getBase64decode(string content) {
			
            byte[] arr = System.Convert.FromBase64String(content);
            return System.Text.Encoding.UTF8.GetString(arr);
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
                case "achievementsReveal":
                case "achievementsUnlock":
                case "achievementsIncrement":
                case "showAchievements":
                    onAchievementsResult onAchievementsResult = (onAchievementsResult)handler;
                    onAchievementsResult(new ResultAPI(resJsonObject.GetField("resultAPI")));
                    break;

                case "leaderboardsSubmitScore":
                case "showLeaderboards":
                    onLeaderboardsResult onLeaderboardsResult = (onLeaderboardsResult)handler;
                    onLeaderboardsResult(new ResultAPI(resJsonObject.GetField("resultAPI")));
                    break;

				case "getGooglePlayerId":
					String googlePlayerId = "";
					resJsonObject.GetField (ref googlePlayerId, "googlePlayerId");

					String authCode = "";
					resJsonObject.GetField (ref authCode, "authCode");

					onGooglePlayerIdResult listener = (onGooglePlayerIdResult)handler;
					listener (new ResultAPI (resJsonObject), getBase64decode(googlePlayerId), getBase64decode(authCode));
					break;

                default:
                    break;
            }
        }
    }		// end of public class UserEngagement
}

/** @} */

