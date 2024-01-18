/**
 * @file    SocialGoogle.cs
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
 * @defgroup SocialGoogle
 * @ingroup Social 
 * @addgroup SocialGoogle
 * @{
 */
 
namespace hive
{
	/**
	 * @ingroup SocialGoogle
	 * @author		ryuvsken, nanomech
	 * @since		4.0.0
	 * \~korean HIVE 유저는 자신의 프로필 정보를 조회 하거나 수정할 수 있고 친구를 맺어서 친구와 함께 게임을 즐길 수 있는 소셜 게임 서비스를 제공한다.<br/>
	 * HIVE 는 Facebook, 주소록 등의 외부 유저 정보를 이용하여 HIVE 친구를 맺고 목록의 동기화를 수행할 수 있다.<br/>
	 * SocialGoogle 클래스는 Google Play Service 의 프로필 조회 기능을 제공한다.<br/><br/>
	 *
	 * \~english HIVE users can view and modify their profile information and provide a social game service where they can make friends and play games with their friends together.<br/>
	 * HIVE can make HIVE friends and synchronize the list by using external user information such as Facebook and contacts on your device.<br/>
	 * The SocialGoogle class provides Google Play Games profile.<br/><br/>
	 */
	public class SocialGoogle {

		// \internal
		// \korean Native 영역에서 호출된 요청을 처리하기 위한 플러그인 내부 코드
		// \english Internal code to handle requests invoked from the native code
		public static void executeEngine(JSONObject resJsonObject) {

			String methodName = null;
			resJsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			resJsonObject.GetField (ref handlerId, "handler");
			object handler = (object)HIVEUnityPlugin.popHandler (handlerId);

			if (handler == null) return;
		}
	}




	/**
	 * \~korean GooglePlay Player 프로필 정보
	 *
	 * \~english Google Play Games Profile information
	 * \~
	 * @ingroup SocialGoogle
	 * @author ryuvsken
	 */
	public class ProfileGooglePlay {

		public String playerId;			///< GooglePlay's PlayerId
		public String playerName;		///< GooglePlay's DiaplayPlayerName


		public ProfileGooglePlay() {
		}

		public ProfileGooglePlay(String playerId, String playerName) {

			this.playerId = playerId;
			this.playerName = playerName;
		}


		public ProfileGooglePlay(JSONObject resJsonParam) {

			if (resJsonParam == null || resJsonParam.count <= 0)
				return;

			resJsonParam.GetField (ref this.playerId, "playerId");
			resJsonParam.GetField (ref this.playerName, "playerName");
		}


		public String toString() {

			StringBuilder sb = new StringBuilder();

			sb.Append ("ProfileGoogle {");
			sb.Append ("playerId = ");
			sb.Append (this.playerId);
			sb.Append (", playerName = ");
			sb.Append (this.playerName);
			sb.Append (" }\n");

			return sb.ToString();
		}
	}
}


/** @} */



