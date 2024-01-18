/**
 * @file    HiveUnity.cs
 * @brief   Unity 통신 브릿지
 * 
 * @ingroup hive
 * @author  ryuvsken
 * @date    2016-2022
 * @copyright Copyright © Com2uS Platform Corporation. All Right Reserved.
 */
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading.Tasks;	// for 	custom UnitySendMessage implement
using AOT;

namespace hive
{
	/**
	 * @brief Unity 통신 브릿지
	 * 
	 * @ingroup hive
	 * @author ryuvsken
	 */
	public class HIVEUnityPlugin : MonoBehaviour {
#if UNITY_EDITOR || UNITY_STANDALONE
		void OnApplicationQuit()
		{
			JSONObject jsonParam = HIVEUnityPlugin.createParam("AuthV4", "terminateProcess", null);
			HIVEUnityPlugin.callNative(jsonParam);
		}
#endif
#if !UNITY_EDITOR && UNITY_ANDROID
		private static AndroidJavaClass _androidClass;
#endif


		public static String targetObject;

		public static int handlerId = 0;

		public static Dictionary<int, object> callbackHandler = new Dictionary<int, object>();

		public static Dictionary<int, object> openCallbackHandler = new Dictionary<int, object>();
		public static Dictionary<int, object> closeCallbackHandler = new Dictionary<int, object>();
		public static Dictionary<int, object> startPlaybackCallbackHandler = new Dictionary<int, object>();
		public static Dictionary<int, object> finishPlaybackCallbackHandler = new Dictionary<int, object>();
		public static Dictionary<int, object> exitCallbackHandler = new Dictionary<int, object>();

		public static object userEngagementHandler;
		public static object authV4ProviderChangeHandler;
#if UNITY_EDITOR || UNITY_STANDALONE
		static HIVEUnityPlugin currentPlugin;
#endif
		
		public static void InitPlugin(){
			var hiveObj = new GameObject();
			hiveObj.name = "HIVEUnityPluginObject(DontDestroyOnLoad)";
#if UNITY_EDITOR || UNITY_STANDALONE
			currentPlugin = 
#endif
			hiveObj.AddComponent<HIVEUnityPlugin>();

		}

		public void Awake()
		{
#if !UNITY_EDITOR
			DontDestroyOnLoad(gameObject);
#endif

#if !UNITY_EDITOR && UNITY_ANDROID		
			_androidClass = new AndroidJavaClass("com.hive.plugin.HivePluginUnity");
#endif

			HIVEUnityPlugin.targetObject = this.name;
#if !UNITY_EDITOR && UNITY_STANDALONE
			s_SendMessageDelegate = UnitySendMessageWrapper;
		    SetUnitySendMessageCallback(s_SendMessageDelegate);
#endif
		}

#if UNITY_STANDALONE
		public delegate void UnitySendMessageDelegate([MarshalAs(UnmanagedType.LPStr)]string gameObjectName, [MarshalAs(UnmanagedType.LPStr)]string methodName, [MarshalAs(UnmanagedType.LPStr)]string message);
		
		private static UnitySendMessageDelegate s_SendMessageDelegate;
		
		[DllImport("HIVE_PLUGIN")]
		private static extern void SetUnitySendMessageCallback(UnitySendMessageDelegate sendMessage);
		[MonoPInvokeCallback (typeof(UnitySendMessageDelegate))]
		private static void UnitySendMessageWrapper(string gameObjectName, string methodName, string message)
		{
			if(currentPlugin != null)
				currentPlugin.Enqueue(UnitySendMessageOnTheMainThread(gameObjectName, methodName, message)); 
		}
		public static IEnumerator UnitySendMessageOnTheMainThread(string gameObjectName, string methodName, string message) {
			var gameObject = GameObject.Find(gameObjectName);
			if (gameObject != null)
			{
				gameObject.SendMessage(methodName, message);
			}
			yield return null;
		}
		#endif

		public static JSONObject createParam(string className, string methodName, object handler) {

			JSONObject jsonParam = new JSONObject ();
			jsonParam.AddField ("targetObject", HIVEUnityPlugin.targetObject);
			jsonParam.AddField ("class", className);
			jsonParam.AddField ("method", methodName);
			jsonParam.AddField ("platform", "unity");

			if ("Promotion".Equals(className)) {
				if( handler != null &&
				methodName != null ) {
					jsonParam.AddField ("handler", pushPromotionHandler(methodName, handler));
				}
			}
			else if ("AuthV4".Equals(className) && "setProviderChangedListener".Equals(methodName)) {
				if (handler != null && methodName != null) {
					jsonParam.AddField("handler", pushAuthV4Handler(methodName, handler));
				}
			}
			// else if("UserEngagement".Equals(className)) {
			// 	if( handler != null && methodName != null ) {
			// 		setEngagementHandler(methodName, handler);
			// 	}
			// }
			else if (handler != null) {
				jsonParam.AddField ("handler", HIVEUnityPlugin.pushHandler (handler));
			}
			return jsonParam;
		}


		public static int pushHandler(object handler) {

			int newHandlerId = HIVEUnityPlugin.handlerId++;
			callbackHandler [newHandlerId] = handler;
			return newHandlerId;
		}


		public static object popHandler(int handlerIdParam) {

			if (callbackHandler.ContainsKey(handlerIdParam) == false) return null;

			object handler = callbackHandler [handlerIdParam];
			if (handler != null) {
				callbackHandler.Remove (handlerIdParam);
			}
			return handler;
		}

		// public static void setEngagementHandler(string methodName, object handler) {

		// 	if ("setEngagementHandler".Equals(methodName)) {
		// 		userEngagementHandler = handler;
		// 	}
		// }

		// public static object getEngagementHandler() {

		// 	return userEngagementHandler;
		// }
		public static int pushAuthV4Handler(string methodName, object handler) {
			int newHandlerId = HIVEUnityPlugin.handlerId++;
			if("setProviderChangedListener".Equals(methodName)) {
				authV4ProviderChangeHandler = handler;
			} else {
				pushHandler(handler);
			}
			return newHandlerId;
		}

		public static object popAuthV4Handler(JSONObject jsonObject) {
			String methodName = null;
			jsonObject.GetField(ref methodName, "method");

			int handlerId = -1;
			jsonObject.GetField (ref handlerId, "handler");

			object handler = null;
			if("setProviderChangedListener".Equals(methodName)) {
				handler = authV4ProviderChangeHandler;
			} else {
				handler = (object)HIVEUnityPlugin.popHandler(handlerId);
			}
			return handler;
		}
		public static int pushPromotionHandler(string methodName, object handler) {

			int newHandlerId = HIVEUnityPlugin.handlerId++;

			if ("showPromotion".Equals (methodName) ||
				"showCustomContents".Equals (methodName) ||
				"showCustomContentsOnGameWindow".Equals (methodName) ||
				"showOfferwall".Equals(methodName) ||
				"showNews".Equals(methodName) ||
				"showReview".Equals (methodName)) {

				openCallbackHandler [newHandlerId] = handler;
				closeCallbackHandler [newHandlerId] = handler;
				startPlaybackCallbackHandler [newHandlerId] = handler;
				finishPlaybackCallbackHandler [newHandlerId] = handler;
			}
			else if ("showExit".Equals(methodName)) {

				openCallbackHandler [newHandlerId] = handler;
				closeCallbackHandler [newHandlerId] = handler;
				exitCallbackHandler [newHandlerId] = handler;
			}
			else if ("setEngagementHandler".Equals(methodName)) {
				userEngagementHandler = handler;
			}
			else {
				callbackHandler [newHandlerId] = handler;
			}
			
			return newHandlerId;
		}

		public static object popPromotionHandler(JSONObject jsonObject) {

			String methodName = null;
			jsonObject.GetField (ref methodName, "method");

			int handlerId = -1;
			jsonObject.GetField (ref handlerId, "handler");

			if (handlerId == -1) return null;

			String eventValue = "";
			jsonObject.GetField (ref eventValue, "promotionEventType");

			object handler = null;

			if ("showPromotion".Equals (methodName) ||
			    "showCustomContents".Equals (methodName) ||
				"showCustomContentsOnGameWindow".Equals (methodName) ||
			    "showOfferwall".Equals (methodName) ||
				"showNews".Equals(methodName) ||
				"showReview".Equals (methodName) ||
			    "showExit".Equals (methodName)) {

				if ("OPEN".Equals (eventValue.ToUpper ())) {
					openCallbackHandler.TryGetValue(handlerId, out handler);
					if (handler != null) openCallbackHandler.Remove (handlerId);
				}
				else if ("CLOSE".Equals (eventValue.ToUpper ())) {
					closeCallbackHandler.TryGetValue(handlerId, out handler);
					if (handler != null) closeCallbackHandler.Remove (handlerId);
				}	
				else if ("START_PLAYBACK".Equals (eventValue.ToUpper ())) {
					startPlaybackCallbackHandler.TryGetValue(handlerId, out handler);
					if (handler != null) startPlaybackCallbackHandler.Remove (handlerId);
				}	
				else if ("FINISH_PLAYBACK".Equals (eventValue.ToUpper ())) {
					finishPlaybackCallbackHandler.TryGetValue(handlerId, out handler);
					if (handler != null) finishPlaybackCallbackHandler.Remove (handlerId);
				}	
				else if ("EXIT".Equals (eventValue.ToUpper ())) {
					exitCallbackHandler.TryGetValue(handlerId, out handler);
					if (handler != null) exitCallbackHandler.Remove (handlerId);
				}
			}
			else if ("setEngagementHandler".Equals (methodName)) {
				handler = userEngagementHandler;
			}
			else {
				callbackHandler.TryGetValue(handlerId, out handler);
				if (handler != null) callbackHandler.Remove (handlerId);
			}

			return handler;
		}


#if !UNITY_EDITOR && UNITY_IPHONE

		[DllImport("__Internal")]
		public static extern IntPtr HivePlugin_callNative_u(String jsonParam);
#elif !UNITY_EDITOR && UNITY_STANDALONE
		[DllImport("HIVE_PLUGIN")]
		public static extern IntPtr HivePlugin_callNative_u(String jsonParam);
		[DllImport("HIVE_PLUGIN")]
		public static extern void HivePlugin_callNative_after(IntPtr ptr1);
#endif

		/**
		 * Unity Engine 영역에서 Native (Java / Objective-C) 영역으로 호출
		 */
		public static JSONObject callNative(JSONObject jsonParam) {

#if !UNITY_EDITOR && UNITY_ANDROID

			String jsonParamString = jsonParam.ToString();
			String resJsonString = _androidClass.CallStatic<String>("callNative", jsonParamString);
			return new JSONObject(resJsonString);

#elif !UNITY_EDITOR && (UNITY_IPHONE || UNITY_STANDALONE)
			String jsonParamString = jsonParam.ToString();
#if UNITY_STANDALONE
			IntPtr resultString = HivePlugin_callNative_u(jsonParamString);
#if UNITY_STANDALONE_OSX
			String resJsonString = Marshal.PtrToStringAuto(resultString);
#elif UNITY_STANDALONE_WIN
			String resJsonString = Marshal.PtrToStringAnsi(resultString);
#endif
			if(!String.IsNullOrEmpty(resJsonString))
				HivePlugin_callNative_after(resultString);
#else
			String resJsonString = Marshal.PtrToStringAuto(HivePlugin_callNative_u(jsonParamString));
#endif
			return new JSONObject(resJsonString);

#elif UNITY_EDITOR

			String jsonParamString = jsonParam.ToString();
			String resJsonString = callNativeUnity(jsonParamString);
			//에디터용 resJsonString 추가.
			return new JSONObject(resJsonString);

#else

			return new JSONObject();

#endif
		}

#if UNITY_EDITOR
		public static String callNativeUnity(String jsonParamString) {
			JSONObject jsonParam = new JSONObject(jsonParamString);

			JSONObject classNameParam = jsonParam["class"];
			JSONObject methodParam = jsonParam["method"];
			
			if(classNameParam.isNull || methodParam.isNull) {	
				Debug.LogError("param not found(className or method)");
				return "";
			}

			String className = classNameParam.stringValue;
			String method = methodParam.stringValue;
			String resString = "";

			if(!isSupportedFeature(className,method)) {
				//지원 불가능은 핸들러 확인해서 not supported 돌려줌
				if(jsonParam.HasField("handler")){
					callEngineNotSupported(jsonParam);
				}
			} else {
				//지원 가능한 피쳐만 구현
				if(className == "Auth") {
					resString = authExecuteNative(jsonParam);
				} else if(className == "AuthV4") {
					resString = authV4ExecuteNative(jsonParam);
				}
			}

			return resString;
		}

		public static void callEngineNotSupported(JSONObject jsonParam){
			ResultAPI resultAPI = new ResultAPI(ResultAPI.ErrorCode.NOT_SUPPORTED, "Not supported function in editor");
			JSONObject resJsonParam = createResponse(resultAPI, jsonParam);
			//call engine
			currentPlugin.callEngine(resJsonParam.ToString());
		}

		public static bool isSupportedFeature(string className,string method) {
			return (className == "Auth" && (
				method == "initialize" ||
				method == "getLoginType" ||
				method == "login" ||
				method == "getAccount"
			)) || (className == "AuthV4" && (
				method == "setup" ||
				method == "signIn" ||
				method == "isAutoSignIn" ||
				method == "getPlayerInfo"
			));
		}

		public static JSONObject createResponse(ResultAPI resultAPI, JSONObject jsonParam) {
			JSONObject resJsonParam = new JSONObject();
			resJsonParam["class"] = jsonParam.GetField("class");
			resJsonParam["method"] = jsonParam.GetField("method");
			resJsonParam["handler"] = jsonParam.GetField("handler");
			resJsonParam["platform"] = jsonParam.GetField("platform");

			if(resultAPI != null) {
				JSONObject resultAPIJson = new JSONObject(resultAPI.toString());
				resJsonParam.AddField("resultAPI", resultAPIJson);
			}

			return resJsonParam;
		}

		public struct AuthV1LoginData {
			public static String did;
			public static String vid;
			public static String accessToken;
		}

		public static String authExecuteNative(JSONObject jsonParam) {
			String targetObject = jsonParam.GetField("targetObject").stringValue;
    		String methodName = jsonParam.GetField("method").stringValue;
			
			String resString = "";

			ResultAPI resultAPI = new ResultAPI(ResultAPI.ErrorCode.SUCCESS, "");

			if(methodName == "initialize") {
				JSONObject resJsonParam = createResponse(resultAPI, jsonParam);
				resJsonParam.AddField("authInitResult",new JSONObject());
				resJsonParam["authInitResult"].AddField("isAuthorized", "false");
				resJsonParam["authInitResult"].AddField("loginType", "GUEST");
				resJsonParam["authInitResult"].AddField("did", AuthV1LoginData.did);
				//call engine
				currentPlugin.callEngineInEditor(resJsonParam);		// #GCPSDK4-1020
			} else if(methodName == "getLoginType") {
				JSONObject resJsonParam = createResponse(null, jsonParam);
				resJsonParam.AddField("getLoginType","GUEST"); //GUEST 고정으로 리턴해준다.
				resString = resJsonParam.ToString();
			} else if(methodName == "login") {
				JSONObject resJsonParam = createResponse(resultAPI, jsonParam);
				resJsonParam.AddField("loginType", "GUEST");

    			JSONObject currentAccount = new JSONObject();
				currentAccount.AddField("vid", AuthV1LoginData.vid);
				currentAccount.AddField("uid", "");//GUEST는 빈값
				currentAccount.AddField("did", AuthV1LoginData.did);
				currentAccount.AddField("accessToken", AuthV1LoginData.accessToken);

				resJsonParam.AddField("currentAccount", currentAccount);
				resJsonParam.AddField("usedAccount", new JSONObject()); //충돌상황이 존재하지 않음.

				//call engine
				currentPlugin.callEngineInEditor(resJsonParam);		// #GCPSDK4-1020
			} else if(methodName == "getAccount") {
				JSONObject resJsonParam = createResponse(null, jsonParam);

				JSONObject currentAccount = new JSONObject();
				currentAccount.AddField("vid", AuthV1LoginData.vid);
				currentAccount.AddField("uid", "");//GUEST는 빈값
				currentAccount.AddField("did", AuthV1LoginData.did);
				currentAccount.AddField("accessToken", AuthV1LoginData.accessToken);

				resJsonParam.AddField("getAccount", currentAccount);

				resString = resJsonParam.ToString();
			}

			return resString;
		}

		public struct AuthV4LoginData {
			public static Int64 playerId;
			public static String playerName;
			public static String playerImageUrl;
			public static String playerToken;
			public static String did;
		}

		public static JSONObject GetFakePlayerInfo() {

			JSONObject ret = new JSONObject();
			ret.AddField("playerId",AuthV4LoginData.playerId);
			ret.AddField("playerName",AuthV4LoginData.playerName);
			ret.AddField("playerImageUrl",AuthV4LoginData.playerImageUrl);
			ret.AddField("playerToken",AuthV4LoginData.playerToken);
			ret.AddField("did",AuthV4LoginData.did);
			ret.AddField("providerInfoData",new JSONObject(JSONObject.Type.Array));
			ret.AddField("customProviderInfoData",new JSONObject(JSONObject.Type.Array));
			return ret;
		}

		public static String authV4ExecuteNative(JSONObject jsonParam) {
			String targetObject = jsonParam.GetField("targetObject").stringValue;
    		String methodName = jsonParam.GetField("method").stringValue;
			String resString = "";

			ResultAPI resultAPI = new ResultAPI(ResultAPI.ErrorCode.SUCCESS, "");

			if(methodName == "setup") {
				JSONObject resJsonParam = createResponse(resultAPI, jsonParam);
				resJsonParam.AddField("isAutoSignIn",false);
				resJsonParam.AddField("did", AuthV4LoginData.did);
				JSONObject[] providers = {
					new JSONObject("GUEST")
				};
				resJsonParam.AddField("providerTypeList",new JSONObject(providers));
				currentPlugin.callEngineInEditor(resJsonParam);		// #GCPSDK4-1020
			} else if(methodName == "signIn") {
				JSONObject resJsonParam = createResponse(resultAPI, jsonParam);
				resJsonParam.AddField("playerInfo",GetFakePlayerInfo());
				currentPlugin.callEngineInEditor(resJsonParam);		// #GCPSDK4-1020
			} else if(methodName == "isAutoSignIn") {
				JSONObject resJsonParam = createResponse(null, jsonParam);
				resJsonParam.AddField("isAutoSignIn", true);
				//자동로그인 무조건 true 입니다.
				resString = resJsonParam.ToString();
			} else if(methodName == "getPlayerInfo") {
				//TODO GETPlayerInfo
				JSONObject resJsonParam = createResponse(null, jsonParam);
				resJsonParam.AddField("getPlayerInfo", GetFakePlayerInfo());
				resString = resJsonParam.ToString();
			}

			return resString;
		}

		/**
		 * Native (Java / Objective-C) 영역에서 Unity Engine 영역으로 호출 (for Unity Editor)
		 */
		public void callEngineInEditor(JSONObject jsonParam) {
			// #GCPSDK4-1020, jsonParam 하위 데이터로 JSONObject[] 혹은 JSONObject() 가 포함될 경우
			// jsonParam을 다시 toJSONObject 시 FormatException 발생,
			// ToString하지 않고 json 데이터를 바로 처리하도록 Editor용 callEngine 메서드를 개별 추가

			String className = null;
			jsonParam.GetField(ref className, "class");

			if("Auth".Equals(className)){
				Auth.executeEngine (jsonParam);
			}
			else if("AuthV4".Equals(className)){
				AuthV4.executeEngine(jsonParam);
			}
		}

#endif

		/**
		 * Native (Java / Objective-C) 영역에서 Unity Engine 영역으로 호출
		 */
		public void callEngine(String jsonParam) {

			JSONObject resJsonObject = new JSONObject (jsonParam);

			String className = null;
			resJsonObject.GetField (ref className, "class");

			if ("Analytics".Equals (className)) {
				Analytics.executeEngine (resJsonObject);
			}
			else if("Configuration".Equals(className)){
				Configuration.executeEngine (resJsonObject);
			}
			else if("Auth".Equals(className)){
				Auth.executeEngine (resJsonObject);
			}
			else if("AuthV4".Equals(className)){
				AuthV4.executeEngine(resJsonObject);
			}
			else if("SocialHive".Equals(className)){
				SocialHive.executeEngine (resJsonObject);
			}
			else if("SocialFacebook".Equals(className)){
				SocialFacebook.executeEngine (resJsonObject);
			}
			else if("SocialGoogle".Equals(className)){
				SocialGoogle.executeEngine (resJsonObject);
			}
			else if("Promotion".Equals(className)){
				Promotion.executeEngine (resJsonObject);
			}
			else if("Push".Equals(className)){
				Push.executeEngine (resJsonObject);
			}
			else if("IAP".Equals(className)){
				IAP.executeEngine (resJsonObject);
			}
			else if("IAPV4".Equals(className)){
				IAPV4.executeEngine (resJsonObject);
			}
			else if("PlatformHelper".Equals(className)){
				PlatformHelper.executeEngine(resJsonObject);
			}
			// else if("UserEngagement".Equals(className)){
			// 	UserEngagement.executeEngine(resJsonObject);
			// }
#if !UNITY_EDITOR && UNITY_ANDROID
			else if("ProviderGoogle".Equals(className)){
				ProviderGoogle.executeEngine(resJsonObject);
			}
#elif !UNITY_EDITOR && UNITY_IPHONE
			else if("ProviderApple".Equals(className)){
				ProviderApple.executeEngine(resJsonObject);
			}
#endif // !UNITY_EDITOR && UNITY_ANDROID or !UNITY_EDITOR && UNITY_IPHONE
			else if("AuthV4Helper".Equals(className)){
				AuthV4.Helper.executeEngine(resJsonObject);
			}
			else if("SocialV4".Equals(className)) {
				SocialV4.executeEngine(resJsonObject);
			}
			else if("DataStore".Equals(className)) {
				DataStore.executeEngine(resJsonObject);
			}
		}

		// Code for MainThread Queue
		private static readonly Queue<Action> _executionQueue = new Queue<Action>();

		public void Update() {
			lock(_executionQueue) {
				while (_executionQueue.Count > 0) {
					_executionQueue.Dequeue().Invoke();
				}
			}
		}
		/// <summary>
		/// Locks the queue and adds the IEnumerator to the queue
		/// </summary>
		/// <param name="action">IEnumerator function that will be executed from the main thread.</param>
		public void Enqueue(IEnumerator action) {
			lock (_executionQueue) {
				_executionQueue.Enqueue (() => {
					StartCoroutine (action);
				});
			}
		}

		/// <summary>
		/// Locks the queue and adds the Action to the queue
		/// </summary>
		/// <param name="action">function that will be executed from the main thread.</param>
		public void Enqueue(Action action)
		{
			Enqueue(ActionWrapper(action));
		}
		
		/// <summary>
		/// Locks the queue and adds the Action to the queue, returning a Task which is completed when the action completes
		/// </summary>
		/// <param name="action">function that will be executed from the main thread.</param>
		/// <returns>A Task that can be awaited until the action completes</returns>
		public Task EnqueueAsync(Action action)
		{
			var tcs = new TaskCompletionSource<bool>();

			void WrappedAction() {
				try 
				{
					action();
					tcs.TrySetResult(true);
				} catch (Exception ex) 
				{
					tcs.TrySetException(ex);
				}
			}

			Enqueue(ActionWrapper(WrappedAction));
			return tcs.Task;
		}

		
		IEnumerator ActionWrapper(Action a)
		{
			a();
			yield return null;
		}
#if UNITY_STANDALONE_WIN
		public static IMECompositionMode savedInputMode;
#endif
		public static void IMECompositionModeOn()
        {
#if UNITY_STANDALONE_WIN
			savedInputMode = Input.imeCompositionMode;
			Input.imeCompositionMode = IMECompositionMode.On;
#endif
		}
		public static void IMECompositionModeRestore()
        {
#if UNITY_STANDALONE_WIN
			Input.imeCompositionMode = savedInputMode;
#endif
		}
	}

}



