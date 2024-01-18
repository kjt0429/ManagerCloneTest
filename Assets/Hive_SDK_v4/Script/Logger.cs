/**
 * @file    Logger.cs
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
 * @defgroup Logger
 * @{
 * \~korean  로그 정보를 기록하는 클래스<br>
 * (네이티브 영역을 호출하기 때문에 부하를 고려해서 로그를 기록해야 한다.)
 * \~english Class that records log information<br/>
 * (Logs should be recorded taking into account the load, because you are calling the native code. )<br/>
 */

 namespace hive
{
 	/**
 	 * \~korean 로그 정보를 기록하는 클래스<br>
 	 * (네이티브 영역을 호출하기 때문에 부하를 고려해서 로그를 기록해야 한다.)
 	 * \~english Class that records log information<br>
 	 * (Logs should be recorded taking into account the load, because you are calling the native code.)
	 * \~
 	 * @ingroup Logger
 	 * @author ryuvsken
 	 */
	public class Logger {

		/**
		* log 함수를 사용했을때 로그를 원격지 혹은 로컬에 출력하는지의 여부를 반환한다.
		*
		* @return 로그 활성화 여부
		*/
		public static bool isActivateLogging() {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Logger", "isActivateLogging", null);
			
			JSONObject resJsonObject = HIVEUnityPlugin.callNative(jsonParam);

			bool ret = resJsonObject.GetField("isActivateLogging");

			return ret;
		}

		/**
		* \~korean 로그 정보 기록<br/>
		* 로그 출력에 대한 Base API
		* HIVE에서 리모트 로깅 활성화 시 원격지로 로그를 보내는 것이 가능하다.
		* 단, 네트워크 통신이 들어가는 만큼 과도하게 많은양의 로그나 반복적인 로그는 찍지 않도록 주의할것.
		* \~english Record log information<br/>
     	* (Logs should be recorded taking into account the load, because you are calling the native code.)
		* \~
 		* @ingroup Logger
		*/
		public static void log(String msg) {

			if (Configuration.getUseLog ()) {
				Debug.Log (msg);
			}

			JSONObject jsonParam = HIVEUnityPlugin.createParam("Logger", "log", null);

			jsonParam.AddField ("tag", "");
			jsonParam.AddField ("level", "L");
			jsonParam.AddField ("msg", jsonEncoding(msg));
			HIVEUnityPlugin.callNative (jsonParam);
		}

		/**
		* \~korean 로그 필터 설정
		* \~english Set log filter
		* \~
		* @ingroup Logger
		*/
		public static void setLogFilter(LogFilter logFilter) {
			JSONObject jsonParam = HIVEUnityPlugin.createParam("Logger", "setLogFilter", null);

			JSONObject logFilterJson = new JSONObject();
			logFilterJson.AddField("coreLog", Enum.GetName(typeof(LogType), logFilter.coreLog));
			logFilterJson.AddField("serviceLog", Enum.GetName(typeof(LogType), logFilter.serviceLog));

			jsonParam.AddField("logFilter", logFilterJson);
			HIVEUnityPlugin.callNative (jsonParam);
		}


		private static String jsonEncoding(String text) {
			StringBuilder builder = new StringBuilder();

			char[] charArray = text.ToCharArray();
			foreach (char c in charArray) {
			    switch (c) {
			    case '"':
			        builder.Append("\\\"");
			        break;
			    case '\\':
			        builder.Append("\\\\");
			        break;
			    case '\b':
			        builder.Append("\\b");
			        break;
			    case '\f':
			        builder.Append("\\f");
			        break;
			    case '\n':
			        builder.Append("\\n");
			        break;
			    case '\r':
			        builder.Append("\\r");
			        break;
			    case '\t':
			        builder.Append("\\t");
			        break;
			    default:
			        int codepoint = Convert.ToInt32(c);
			        if ((codepoint >= 32) && (codepoint <= 126)) {
			            builder.Append(c);
			        } else {
			            builder.Append("\\u");
			            builder.Append(codepoint.ToString("x4"));
			        }
			        break;
			    }
			}

			return builder.ToString();
		}
	}

	public enum LogType {
		Verbose,
		Debug,
		Info,
		Warning,
		Error,
		None
	}

	public class LogFilter {
		public LogType coreLog = LogType.Verbose;
		public LogType serviceLog = LogType.Verbose;
	}
}


/** @} */


