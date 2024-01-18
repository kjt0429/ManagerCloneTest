/**
 * @file    ResultAPI.cs
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
 * @defgroup ResultAPI
 * @{
 * \~korean  API 호출에 대한 결과를 담는 클래스<br/><br/>
 * \~english Class containing results for API calls<br/><br/>
 */

namespace hive
{
	/**
	 * @since		4.0.0
	 * @author ryuvsken, hife
	 * @ingroup ResultAPI
	 *  \~korean 비동기 API 호출에 대한 결과를 담는 클래스<br/><br/>
	 * 
	 *  \~english A class that holds the results of an asynchronous API call<br/><br/>
	 * 
	 */
	public partial class ResultAPI {

		

		public ErrorCode errorCode = ErrorCode.SUCCESS;

		public Code code = Code.Success;

		public String errorMessage = "SUCCESS";

		public String message = "SUCCESS";

		public long latencyMs = 0;


		public ResultAPI() {
		}


		public ResultAPI(int errorCode, String errorMessage) {

			if (Enum.IsDefined(typeof(ErrorCode), errorCode))
				this.errorCode = (ErrorCode)errorCode;
			else
				this.errorCode = ErrorCode.UNKNOWN;

			this.errorMessage = errorMessage;
		}

		public ResultAPI(ErrorCode errorCode, String errorMessage) {

			this.errorCode = errorCode;
			this.errorMessage = errorMessage;
		}


		public ResultAPI(JSONObject resJsonParam) {

			int _errorCode = 0;
			resJsonParam.GetField (ref _errorCode, "errorCode");

			if (Enum.IsDefined(typeof(ErrorCode), _errorCode))
				this.errorCode = (ErrorCode)_errorCode;
			else
				this.errorCode = ErrorCode.UNKNOWN;

			int _code = 0;
			resJsonParam.GetField (ref _code, "code");

			if(Enum.IsDefined(typeof(Code), _code))
				this.code = (Code)_code;
			else
				this.code = Code.CommonUnknown;

			resJsonParam.GetField (ref this.errorMessage, "errorMessage");
			resJsonParam.GetField (ref this.message, "message");
			resJsonParam.GetField (ref this.latencyMs, "latencyMs");
		}


		public Boolean isSuccess() {
			
			return (this.errorCode >= ErrorCode.SUCCESS
				|| this.code >= Code.Success);
		}

		public Boolean needExit() {
			return this.errorCode == ErrorCode.NEED_EXIT;
		}

		public String toString() {

			StringBuilder sb = new StringBuilder ();

			sb.Append ("ResultAPI { errorCode = ");
			sb.Append (this.errorCode);
			sb.Append (", Code = ");
			sb.Append (this.code);
			sb.Append (", msg = ");
			sb.Append (this.errorMessage);
			if (latencyMs != 0) {
				sb.Append (", latencyMs = ");
				sb.Append (this.latencyMs);
			}
			sb.Append (" }\n");

			return sb.ToString ();
		}

	}
}


/** @} */




