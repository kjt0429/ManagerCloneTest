using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

namespace Hive.Unity.Editor {

	[InitializeOnLoad]
	public class LoginDataEntryPoint {
		static LoginDataEntryPoint() {
			EditorApplication.playModeStateChanged += OnPlayModeChange;//2017.2+
		}

		private static void OnPlayModeChange(PlayModeStateChange state){
			if(EditorApplication.isPlayingOrWillChangePlaymode) {
				//v1 data setting
				hive.HIVEUnityPlugin.AuthV1LoginData.did = AuthV1LoginData.did;
				hive.HIVEUnityPlugin.AuthV1LoginData.vid = AuthV1LoginData.vid;
				hive.HIVEUnityPlugin.AuthV1LoginData.accessToken = AuthV1LoginData.accessToken;

				//v4 data setting
				hive.HIVEUnityPlugin.AuthV4LoginData.playerId = AuthV4LoginData.playerId;
				hive.HIVEUnityPlugin.AuthV4LoginData.playerName = AuthV4LoginData.playerName;
				hive.HIVEUnityPlugin.AuthV4LoginData.playerImageUrl = AuthV4LoginData.playerImageUrl;
				hive.HIVEUnityPlugin.AuthV4LoginData.playerToken = AuthV4LoginData.playerToken;
				hive.HIVEUnityPlugin.AuthV4LoginData.did = AuthV4LoginData.did;
			}
		}
	}

	public class AuthV1LoginData {
		static public string did {
			get {
				return FromBase64(EditorPrefs.GetString("authv1_did",""));
			}
			internal set {
				EditorPrefs.SetString("authv1_did",ToBase64(value));
			}
		}
		static public string vid {
			get {
				return FromBase64(EditorPrefs.GetString("authv1_vid",""));
			}
			internal set {
				EditorPrefs.SetString("authv1_vid",ToBase64(value));
			}
		}
		static public string accessToken {
			get {
				return FromBase64(EditorPrefs.GetString("authv1_accessToken",""));
			}
			internal set {
				EditorPrefs.SetString("authv1_accessToken",ToBase64(value));
			}
		}

		static string ToBase64 (string s){
        	return System.Convert.ToBase64String (UTF8Encoding.UTF8.GetBytes (s));
		}

		static string FromBase64 (string s){
			return UTF8Encoding.UTF8.GetString (System.Convert.FromBase64String (s));
		}
		
	}

	public class AuthV4LoginData {
		static public long playerId {
			get {
				return long.Parse(EditorPrefs.GetString("authv4_playerId","0"));
			}
			internal set {
				EditorPrefs.SetString("authv4_playerId",value.ToString());
			}
		}
		static public string playerName {
			get {
				return FromBase64(EditorPrefs.GetString("authv4_playerName",""));
			}
			internal set {
				EditorPrefs.SetString("authv4_playerName",ToBase64(value));
			}
		}
		static public string playerImageUrl {
			get {
				return FromBase64(EditorPrefs.GetString("authv4_playerImageUrl",""));
			}
			internal set {
				EditorPrefs.SetString("authv4_playerImageUrl",ToBase64(value));
			}
		}
		static public string playerToken {
			get {
				return FromBase64(EditorPrefs.GetString("authv4_playerToken",""));
			}
			internal set {
				EditorPrefs.SetString("authv4_playerToken",ToBase64(value));
			}
		}
		static public string did {
			get {
				return FromBase64(EditorPrefs.GetString("authv4_did",""));
			}
			internal set {
				EditorPrefs.SetString("authv4_did",ToBase64(value));
			}
		}
		static string ToBase64 (string s){
        	return System.Convert.ToBase64String (UTF8Encoding.UTF8.GetBytes (s));
		}

		static string FromBase64 (string s){
			return UTF8Encoding.UTF8.GetString (System.Convert.FromBase64String (s));
		}
	}

	public abstract class LoginDataEditor : EditorWindow {
		enum LoginSimulatorVersion {
			AUTHV1,
			AUTHV4
		}

		[MenuItem("Hive/LoginSimulator/Open AuthV1 Setting")]
		static public void OpenAuthV1LoginSimulatorSetting() {
			OpenLoginSimulatorSetting(LoginSimulatorVersion.AUTHV1);
		}

		[MenuItem("Hive/LoginSimulator/Open AuthV4 Setting")]
		static public void OpenAuthV4LoginSimulatorSetting() {
			OpenLoginSimulatorSetting(LoginSimulatorVersion.AUTHV4);
		}

		// [MenuItem("Hive/LoginSimulator/Documents")]
		// static public void OpenLoginSimulatorDocument() {
		// 	const string url = "";//TODO: document url
		// 	Application.OpenURL(url);
		// }

		static void OpenLoginSimulatorSetting(LoginSimulatorVersion version) {
			EditorWindow w;
			switch(version) {
				case LoginSimulatorVersion.AUTHV1:
					w = EditorWindow.GetWindow(typeof(AuthV1LoginDataEditor),true,"Login Simulator Setting (AuthV1)");
					break;
				case LoginSimulatorVersion.AUTHV4:
					w = EditorWindow.GetWindow(typeof(AuthV4LoginDataEditor),true,"Login Simulator Setting (AuthV4)");
					break;
				default:
					w = null;
					break;
			}
			if(w != null)
				w.Show();		
		}

		protected void ApplyButton() {
			if(GUILayout.Button("Apply")){
				SaveLoginData();
			}
		}
		
		protected void UseOnlyTestServerWarring() {
			EditorGUILayout.HelpBox("This feature use only test server",MessageType.Warning);
		}

		protected abstract void SaveLoginData();
	}

	public class AuthV1LoginDataEditor : LoginDataEditor {
		string did;
		string vid;
		string accessToken;

		void Awake() {
			did = AuthV1LoginData.did;
			vid = AuthV1LoginData.vid;
			accessToken = AuthV1LoginData.accessToken;
		}

		void OnGUI() {
			EditorGUILayout.BeginVertical();
			UseOnlyTestServerWarring();
			did = EditorGUILayout.TextField("did",did);
			vid = EditorGUILayout.TextField("vid",vid);
			accessToken = EditorGUILayout.TextField("accessToken",accessToken);
			ApplyButton();
			EditorGUILayout.EndVertical();
		}

		protected override void SaveLoginData() {
			AuthV1LoginData.did = did;
			AuthV1LoginData.vid = vid;
			AuthV1LoginData.accessToken	= accessToken;
		}
	}

	public class AuthV4LoginDataEditor : LoginDataEditor {
		long playerId = 0; //Int64
		string playerName = "";
		string playerImageUrl = "";
		string playerToken = "";
		string did = "";

		void Awake() {
			playerId = AuthV4LoginData.playerId;
			playerName = AuthV4LoginData.playerName;
			playerImageUrl = AuthV4LoginData.playerImageUrl;
			playerToken = AuthV4LoginData.playerToken;
			did = AuthV4LoginData.did;
		}

		void OnGUI() {
			EditorGUILayout.BeginVertical();
			UseOnlyTestServerWarring();
			playerId = EditorGUILayout.LongField("playerId",playerId);
			playerName = EditorGUILayout.TextField("playerName",playerName);
			playerImageUrl = EditorGUILayout.TextField("playerImageUrl",playerImageUrl);
			playerToken = EditorGUILayout.TextField("playerToken",playerToken);
			did = EditorGUILayout.TextField("did",did);
			ApplyButton();
			EditorGUILayout.EndVertical();
		}

		protected override void SaveLoginData() {
			AuthV4LoginData.playerId = playerId;
			AuthV4LoginData.playerName = playerName;
			AuthV4LoginData.playerImageUrl = playerImageUrl;
			AuthV4LoginData.playerToken = playerToken;
			AuthV4LoginData.did = did;
		}
	}
}