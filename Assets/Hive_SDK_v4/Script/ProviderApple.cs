/**
 * @file    ProviderApple.cs
 * 
 *  @date		2016-2022
 *  @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 *  @author		Arkeo Lucid
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
 *  @defgroup	ProviderApple
 *  @ingroup 	Provider
 *  @addtogroup ProviderApple
 *  @{
	 * \~korean Apple GameCenter의 업적 및 리더보드 기능을 제공한다.<br />
	 * \~english It provides Apple GameCenter achievements and leaderboard <br />
 */

namespace hive
{
	/*
	 * \~korean Apple GameCenter의 업적 및 리더보드 기능을 제공한다.<br />
	 * \~english It provides Apple GameCenter achievements and leaderboard <br />
	 * @author Arkeo Lucid
	 * @since		4.3.0 
	 * @ingroup ProviderApple
	 */
	public class ProviderApple {
		/**
		* \~korean Apple GameCenter 업적 정보
		* \~english Apple GameCenter achievements
		* @ingroup ProviderApple
		*/
		public class Achievement {

			public String identifier;	///< \~korean 업적 identifier.  \~english  Achievement identifier.
			public String percent;		///< \~korean 업적 진행 percent value.  \~english  Achievement progress percent value.
			public Boolean completed;	///< \~korean 업적 완료 여부.   \~english  Whether the achievement is complete.

			public Achievement() {}

			public Achievement(String identifier, String percent, Boolean completed) {

				this.identifier = identifier;
				this.percent = percent;
				this.completed = completed;
			}

			public Achievement(JSONObject jsonParam) {

				if (jsonParam == null || jsonParam.count <= 0) return;

				jsonParam.GetField (ref this.identifier, "identifier");
				jsonParam.GetField (ref this.percent, "percent");
				jsonParam.GetField (ref this.completed, "completed");	
			}

			public String toString() {

				StringBuilder sb = new StringBuilder();

				sb.Append("Achievement { identifier = ");
				sb.Append(this.identifier);
				sb.Append(", percent = ");
				sb.Append(this.percent);
				sb.Append(", completed = ");
				sb.Append(this.completed);
				sb.Append(" }\n");

				return sb.ToString();
			}
		}
		
		/**
		*  \~korean 
		* Apple GameCenter 리더보드 전송 result handler.
		* 
		* @param result API 호출 결과.
		* 
		*  \~english
		* Apple GameCenter Leaderboard result handler.
		* 
		* @param result Result of API call.
		* 
		*  \~
		* @see ResultAPI
		*
		* @ingroup ProviderApple
		*/
		public delegate void onReportLeaderboard(ResultAPI resultAPI);
		
		/**
		*  \~korean
		* Apple GameCenter 리더보드 UI 노출 result handler.
		*
		* @param result API 호출 결과.
		*
		*  \~english
		* Apple GameCenter ShowLeaderboard result handler.
		*
		* @param result API 호출 결과.
		*
		*  \~
		* @see ResultAPI
		* 
		* @ingroup ProviderApple
		*/
		public delegate void onShowLeaderboard(ResultAPI resultAPI);
		
		/**
		*  \~korean
		* Apple GameCenter 업적 로드 result handler.
		* 
		* @param result API 호출 결과.
		* @param achievements 업적 목록.
		* 
		*  \~english
		* Apple GameCenter LoadAchievements result handler.
		* 
		* @param result Result of API call.
		* @param achievements List of Achievements.
		*
		*  \~
		* @see ResultAPI, Achievement
		* 
		* @ingroup ProviderApple
		*/
		public delegate void onLoadAchievements(ResultAPI resultAPI, List<Achievement> AchievementList);
		
		/**
		*  \~korean
		* Apple GameCenter 업적을 전송 result handler.
		* 
		* @param result API 호출 결과.
		* 
		*  \~english
		* Apple GameCenter ReportAchievement result handler.
		* 
		* @param result Result of API call.
		*  
		*  \~
		* @see ResultAPI
		* 
		* @ingroup ProviderApple
		*/
		public delegate void onReportAchievement(ResultAPI resultAPI);
		
		/**
		*  \~korean
		* Apple GameCenter 업적 UI 노출 result handler.
		* 
		* @param result API 호출 결과.
		* 
		*  \~english
		* Apple GameCenter ShowAchievement result handler.
		* 
		* @param result Result of API call.
		*
		*  \~
		* @see ResultAPI
		* 
		* @ingroup ProviderApple
		*/
		public delegate void onShowAchievement(ResultAPI resultAPI);
		
		/**
		*  \~korean
		* Apple GameCenter 업적 초기화 result handler.
		* 
		* @param result API 호출 결과.
		* 
		*  \~english
		* Apple GameCenter ResetAchievements result handler.
		* 
		* @param result Result of API call.
		*
		*  \~
		* @see ResultAPI
		* 
		* @ingroup ProviderApple
		*/
		public delegate void onResetAchievements(ResultAPI resultAPI);
		
		/**
		*  \~korean
		* Apple GameCenter Leaderboard에 기록을 전송한다.
		* 
		* @warning score format과 leaderboard identifier는 iTunes Connect에서 설정한다.
		* 
		* @param score Player가 얻은 score.
		* @param leaderboardIdentifier Leaderboard Identifier.
		* @param handler API 호출 result handler.
		* 
		*  \~english
		* It send record to Apple GameCenter Leaderboard.
		* 
		* @warning The score format and leaderboard identifier are set in iTunes Connect.
		* 
		* @param score Player's score.
		* @param leaderboardIdentifier Leaderboard Identifier.
		* @param handler  Result handler of API call.
		*
		*  \~
		* @ingroup ProviderApple
		*/
		public static void reportScore(String score, String leaderboardIdentifier, onReportLeaderboard listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderApple", "reportScore", listener);
			jsonParam.AddField ("score", score);
			jsonParam.AddField ("leaderboardIdentifier", leaderboardIdentifier);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);
		}
		
		/**
		*  \~korean
		* Apple GameCenter Leaderboard UI를 노출한다.
		* 
		* @param handler API 호출 result handler.
		* 
		*  \~english
		* It show Apple GameCenter Leaderboard UI.
		* 
		* @param handler Result handler of API call.
		*
		*  \~
		* @ingroup ProviderApple
		*/
		public static void showLeaderboard(onShowLeaderboard listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderApple", "showLeaderboard", listener);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);
		}
		
		/**
		*  \~korean
		* Apple GameCenter 업적을 로드한다.
		* 
		* @param handler API 호출 result handler.
		* 
		*  \~english
		* It load Apple GameCenter achievement.
		* 
		* @param handler Result handler of API call.
		*
		*  \~
		* @ingroup ProviderApple
		*/
		public static void loadAchievements(onLoadAchievements listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderApple", "loadAchievements", listener);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);
		}
		
		/**
		*  \~korean
		* Apple GameCenter 업적을 전송한다.
		* 
		* @warning achievement identifier는 iTunes Connect에서 설정한다.
		* 
		* @param percent 업적 성취 percent value(ex. 75.0)
		* @param showsCompletionBanner 업적 성공시 상단 배너 노출 여부. default is NO.
		* @param achievementIdentifier Achievement identifier
		* @param handler API 호출 result handler.
		* 
		*  \~english
		* It report Apple GameCenter achievement.
		* 
		* @warning achievement identifier is set in iTunes Connect.
		* 
		* @param percent Achievement progress percent value(ex. 75.0)
		* @param showsCompletionBanner Whether the top banner is exposed when the achievement is successful. default is NO.
		* @param achievementIdentifier Achievement identifier
		* @param handler Result handler of API call.
		* 
		* \~
		* @ingroup ProviderApple
		*/
		public static void reportAchievement(String percent, Boolean showsCompletionBanner, String achievementIdentifier, onReportAchievement listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderApple", "reportAchievement", listener);
			jsonParam.AddField ("percent", percent);
			jsonParam.AddField ("showsCompletionBanner", showsCompletionBanner);
			jsonParam.AddField ("achievementIdentifier", achievementIdentifier);

			HIVEUnityPlugin.callNative (jsonParam);
		}
		
		/**
		*  \~korean
		* Apple GameCenter 업적 UI를 노출한다.
		* 
		* @param handler API 호출 result handler.
		* 
		*  \~english
		* It shows Apple GameCenter Achievement UI.
		* 
		* @param handler Result handler of API call.
		* 
		*  \~
		* @ingroup ProviderApple
		*/
		public static void showAchievements(onShowAchievement listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderApple", "showAchievements", listener);

			JSONObject resJsonObject = HIVEUnityPlugin.callNative (jsonParam);
		}
		
		/**
		*  \~korean
		* Apple GameCenter 업적 정보를 초기화한다.
		* 
		* @param handler API 호출 result handler.
		* 
		*  \~english
		* It resets Apple GameCenter Achievement information.
		* 
		* @param handler Result handler of API call.
		*
		*  \~
		* @ingroup ProviderApple
		*/
		public static void resetAchievements(onResetAchievements listener) {

			JSONObject jsonParam = HIVEUnityPlugin.createParam("ProviderApple", "resetAchievements", listener);

			HIVEUnityPlugin.callNative (jsonParam);
		}

		// Native 영역에서 호출된 요청을 처리하기 위한 플러그인 내부 코드
		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			resJsonObject.GetField (ref handlerId, "handler");

			object handler = (object)HIVEUnityPlugin.popHandler (handlerId);

			if (handler == null) return;

			if ("reportScore".Equals (methodName)) {

				onReportLeaderboard listener = (onReportLeaderboard)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("showLeaderboard".Equals (methodName)) {

				onShowLeaderboard listener = (onShowLeaderboard)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("loadAchievements".Equals (methodName)) {

				List<Achievement> achievementList = new List<Achievement> ();

				JSONObject jsonArray = resJsonObject.GetField ("achievementList");
				if (jsonArray != null && jsonArray.count > 0) {
					List<JSONObject> jsonList = jsonArray.list;
					foreach (JSONObject jsonItem in jsonList) {
						Achievement achievement = new Achievement(jsonItem);
						achievementList.Add(achievement);
					}
				}

				onLoadAchievements listener = (onLoadAchievements)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")), achievementList);
			}
			else if ("reportAchievement".Equals (methodName)) {

				onReportAchievement listener = (onReportAchievement)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("showAchievements".Equals (methodName)) {

				onShowAchievement listener = (onShowAchievement)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
			else if ("resetAchievements".Equals (methodName)) {

				onResetAchievements listener = (onResetAchievements)handler;
				listener (new ResultAPI (resJsonObject.GetField ("resultAPI")));
			}
		}
	}		// end of public class UserEngagement
}


/** @} */

