/**
 * @file    HiveConfigEditor.cs
 * 
 * @author  nanomech
 * @date    2016-2022
 * @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 * @defgroup Hive.Unity.Editor
 * @{
 * @brief HIVE SDK Config 편집 및 플랫폼 설정 지원 <br/><br/>
 */

namespace Hive.Unity.Editor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.HiveEditor;
    using UnityEngine;
    using Hive.Unity;
    using System.IO;

    [CustomEditor(typeof(HiveConfig))]
    public class HiveConfigEditor : UnityEditor.Editor
    {
        // private static int UnityVersion = Unity.Editor.Utility.GetUnityVersion();

        private static bool autoValidation = false;
        private bool showHiveInitSettings = true;
        private bool showHiveAdvanceSettings = false;
        private bool showMobileAppTrackerSettings = true;
        private bool showAdjust = true;
        private bool showSingular = true;
        private bool showAppsFlyer = true;
        private bool showFirebase = true;
        private bool showFacebook = true;//HiveConfig.UseFacebook;
        private bool showGooglePlay = true;//HiveConfig.UseGooglePlay;
		private bool showQQAppId = true; // HiveConfig.QQAppID
        private bool showVKAppId = true; // HiveConfig.VKAppID
        private bool showWeChatAppId = true; // HiveConfig.WeChatAppID
        private bool showLineChannelId = true; // HiveConfig.LineChannelId
        private bool showWeverseClientId = true; // HiveConfig.WeverseClientId
        private bool showSignInWithAppleServiceId = true; // HiveConfig.SignInWithAppleServiceId
        private bool showIAPGUI = true;
        // private bool showAndroidUtils = EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
        // private bool showIOSSettings = EditorUserBuildSettings.activeBuildTarget g == BuildTarget.iOS;
        //private bool showAppLinksSettings = false;
        //private bool showAboutSection = false;
        private enum ConfigPlatform : int{
            Unkown = -1,
            Android = 0,
            iOS = 1,
            macOS = 2,
            Windows = 3,
        }

        private ConfigPlatform platformShown = ConfigPlatform.Android;

        private static GUIContent appNameLabel = new GUIContent("Hive App Id [?]:", "Hive App Id can be fount at https://developers.withhive.com/");
        private static GUIContent facebookAppIdLabel = new GUIContent("Facebook App Id [?]:", "Facebook App Ids can be found at https://developers.facebook.com/apps");
        private static GUIContent facebookClientTokenLabel = new GUIContent("Facebook Client Token [?]:", "Facebook Client Tokens can be found at https://developers.facebook.com/apps");
        private static GUIContent facebookPermission = new GUIContent("Value: ");
        private static GUIContent googleAppIdLabel = new GUIContent("Google App Id [?]:", "Google App Ids can be found at https://play.google.com/apps/publish/#GameListPlace");
		private static GUIContent googleServerClientIdLabel = new GUIContent("Google Server Client Id [?]:","Google Server Client ID can be found at https://play.google.com/apps/publish/#GameListPlace");
        private static GUIContent googlePlayGamesAppIdLabel = new GUIContent("GooglePlayGames App Id [?]:", "Google App Ids can be found at https://play.google.com/apps/publish/#GameListPlace");
        private static GUIContent googlePlayGamesServerClientIdLabel = new GUIContent("GooglePlayGames Server Client Id [?]:", "Google Server Client ID can be found at https://play.google.com/apps/publish/#GameListPlace");
        private static GUIContent googleClientIdLabel = new GUIContent("Google Client Id [?]:","Google Client ID can be found at https://play.google.com/apps/publish/#GameListPlace");
		private static GUIContent googleReversedClientIdLabel = new GUIContent("Google Reversed Client Id [?]","Google Reversed Client ID can be found at https://play.google.com/apps/publish/#GameListPlace");
		private static GUIContent qqAppIdLabel = new GUIContent("QQ App Id [?]:", "Fill the form when you support QQ Login");
        private static GUIContent vkAppIdLabel = new GUIContent("VK App Id [?]:", "Fill the form when you support VK Login");
        private static GUIContent weChatAppIdLabel = new GUIContent("WeChat App Id [?]:", "Fill the form when you support WeChat Login");        
        private static GUIContent weChatAppSecretLabel = new GUIContent("WeChat App Secret [?]:", "Fill the form when you support WeChat Login");
        private static GUIContent universalLinkLabel = new GUIContent("Universal Link [?]:", "Fill the form when you need universal link");
        private static GUIContent weChatPaymentKeyLabel = new GUIContent("WeChat Payment Key [?]:", "Fill the from when you support WeChat Payment");
        private static GUIContent lineChannelIdLabel = new GUIContent("Line Channel Id [?]:","Fill the form when you support Line Login");
        private static GUIContent weverseClientIdLabel = new GUIContent("Weverse Client Id [?]:","Fill the form when you support Weverse Login");
        private static GUIContent signInWithAppleServiceIdLabel = new GUIContent("SignInWithApple Service Id [?]:","Fill the form when you support Apple Login");
		//        private static GUIContent HIVEAndroidAppIdLabel = new GUIContent("HIVE App Id [Android]:");//, "HIVE App Ids can be found at https://play.google.com/apps/publish/#GameListPlace");
//        private static GUIContent HIVEiOSAppIdLabel = new GUIContent("HIVE App Id [iOS]:");

        //iOS
        private static GUIContent urlFacebookSuffixLabel = new GUIContent("URL Scheme Suffix [?]", "Use this to share Facebook APP ID's across multiple iOS apps.  https://developers.facebook.com/docs/ios/share-appid-across-multiple-apps-ios-sdk/");
        private static GUIContent urlHiveLabel = new GUIContent("HIVE URL Scheme [?]", "Use this for HIVE Deeplink across multiple iOS apps.");

        private static GUIContent zoneLabel = new GUIContent("Zone [?]", "Hive 플랫폼의 서버 선택. sandbox 는 개발용, real 은 실계 (sandbox, real)");
        private static GUIContent hivePermissionViewOnLabel = new GUIContent("hivePermissionViewOn [?]", "Hive SDK 권한고지 팝업 노출 여부 설정");
        private static GUIContent loggingLabel = new GUIContent("Logging [?]", "SDK 내부 동작 로그 사용 유무 설정 (true, false)");
        private static GUIContent ageGateU13Label = new GUIContent("AgeGateU13 [?]", "ageGateU13 체크 설정 (true, false)");
        private static GUIContent marketLabel = new GUIContent("Market [?]","결제 마켓을 설정 (GO : Google Play, LE : Com2us Lebi, AP : Apple Appstore)");
        private static GUIContent hiveOrientationLabel = new GUIContent("HiveOrientation [?]","Hive UI 회전방향 설정 (undefined, all, portrait, landscape)");

        //Advance
        private static GUIContent httpConnectTimeoutLabel = new GUIContent("httpConnectTimeout [?]","Hive SDK 내부에서 사용되는 HTTP Connect Timeout 시간을 초단위로 설정 (특별한 경우가 아니면 변경 금지)");
        private static GUIContent httpReadTimeoutLabel = new GUIContent("httpReadTimeout [?]","Hive SDK 내부에서 사용되는 HTTP Read Timeout 시간을 초단위로 설정 (특별한 경우가 아니면 변경 금지)");
        private static GUIContent maxGameLogSizeLabel = new GUIContent("maxGameLogSize [?]","게임 로그 최대 저장 갯수 설정 (특별한 경우가 아니면 변경 금지)");
        //Advance/Analytics
        private static GUIContent analyticsSendLimitLabel = new GUIContent("analyticsSendLimit [?]","한 사이클에 최대로 전송가능한 Analytics 로그의 양입니다.");
        private static GUIContent analyticsQueueLimitLabel = new GUIContent("analyticsQueueLimit [?]","최대로 Queue에 쌓을수 있는 Analytics 로그의 양입니다.");
        private static GUIContent analyticsSendCycleLabel = new GUIContent("analyticsSendCycle [?]","Analytics 전송 사이클 입니다.");
        //Advance/CrashReport
        private static GUIContent useCrashReportLabel = new GUIContent("useCrashReport [?]","하이브 내부의 크래시 리포트핸들러 활성화 여부. 3rd-party 크래시 리포트 솔루션 사용시 false로 설정.");

        // Android
        private static GUIContent packageNameLabel = new GUIContent("Package Name [?]", "aka: the bundle identifier");
        private static GUIContent classNameLabel = new GUIContent("Class Name [?]", "aka: the activity class name for Launch & Engagements as Deeplink");
        private static GUIContent debugAndroidKeyLabel = new GUIContent("Debug Android Key Hash [?]", "Copy this key to the Facebook Settings in order to test a Facebook Android app");
        private static GUIContent writeExternalStoragePermissionLabel = new GUIContent("WriteExternalStoragePermission [?]","Use WriteExternalStoragePermission");

        // MobileAppTracking
        // private static GUIContent isDebugMode = new GUIContent("isDeubgMode [?]:", "TrackerModule use stagin mode and show log");
		private static GUIContent appToken = new GUIContent("appToken [?]:", "Adjust appToken");
		// private static GUIContent addCustomEvent = new GUIContent("Add Custom Event [?]", "Add KeyMatching table item for Custom EventKey.");
        private static GUIContent facebookEventLogging = new GUIContent("FacebookEventLogging Ad [?]:", "facebookEventLogging");
        //Singular
        private static GUIContent appKeySingular = new GUIContent("appID [?]:","Singular appId");
        private static GUIContent secretKeySingular = new GUIContent("secret [?]", "Singular secret value");
        //AppsFlyer
        private static GUIContent secretKeyAppsFlyer = new GUIContent("key [?]", "AppsFlyer key value");
        private static GUIContent secretiTunesConnectIDAppsFlyer = new GUIContent("Itunes Connect App Id [?]", "ItunesConnect AppID key value");

        // Start About App Secret for Adjust
        private static GUIContent appSecretTitle = new GUIContent("App Secret Value [?]","App Secret values in Adjust (secretId, info1, info2, info3, info4) ");
        private static GUIContent appSecretID = new GUIContent("secretId [?]","secretId value in Adjust");
        private static GUIContent appInfo1Values = new GUIContent("info1 [?]","info1 value in Adjust");
        private static GUIContent appInfo2Values = new GUIContent("info2 [?]","info2 value in Adjust");
        private static GUIContent appInfo3Values = new GUIContent("info3 [?]","info3 value in Adjust");
        private static GUIContent appInfo4Values = new GUIContent("info4 [?]","info4 value in Adjust");        

        // Finish About App Secret for Adjust


        private static GUIContent eventNameLabel = new GUIContent("Name : "); 
        private static GUIContent eventValueLabel = new GUIContent("Value : ");

		private static GUIContent chooseAuthTypeLabel = new GUIContent("Authentication (Auth) Version : ");
		private static GUIContent authV1Label = new GUIContent("Auth v1 [?]", "Check when you use Auth v1 in HIVE SDK.");
		private static GUIContent authV4Label = new GUIContent("Auth v4 [?]", "Check when you use Auth v4 in HIVE SDK.");
        private static GUIContent autoValidationLabel = new GUIContent("Auto Validation : ", "Check to validate hive_config.xml with XML schema(XSD)");

        private HiveConfigXML SelectedPlatform = null;
        private string[] platform =  {"Android","iOS", "macOS", "Windows"};
        private int SelectedIndex = 0;
        private string SelectedPlatformPostfix = "";
        // private bool bAndroid = false;
        // private bool biOS = false;

        public class FacebookPermissions
        {
            public List<string> permissions = new List<string>();

            public virtual void OnGUI(HiveConfigEditor editor)
            {                
                if(permissions != null && permissions.Count > 0)
                {
                    string[] permissionsArr = permissions.ToArray();

                    EditorGUI.indentLevel++;
                    for(int i = 0; i < permissionsArr.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        permissionsArr[i] = editor.EditableTextField(facebookPermission, permissionsArr[i]);
                    
                        if (GUILayout.Button("Remove", GUILayout.Width(70)))
                        {
                           permissionsArr[i] = null;                           
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUI.indentLevel--;

                    for (int i = 0; i < permissionsArr.Length; i++)
                    {
                        permissions[i] = permissionsArr[i];
                    }

                    for (int i = 0; i < permissionsArr.Length; i++)
                    {
                        if (permissionsArr[i] == null)
                        {
                            permissions.RemoveAt(i);
                        }
                    }
                }
                
                if (GUILayout.Button("Add Permission"))
                {
                    permissions.Add("");
                }
            }
        }

        public class Tracker
        {
            public string name = "";
            //public Dictionary<string, string> events;
            public List<string> eventName = new List<string>();
            public List<string> eventValue = new List<string>();

            public void AddEvent(string name, string value)
            {
                eventName.Add(name);
                eventValue.Add(value);
            }
            public void RemoveEventByName(string name)
            {
                int index = eventName.BinarySearch(name);
                eventName.RemoveAt(index);
                eventValue.RemoveAt(index);
            }
            public virtual void OnGUI(HiveConfigEditor editor)
            {
                if( eventName != null && eventName.Count > 0 )
                {
                    EditorGUILayout.LabelField("Events");
                    EditorGUI.indentLevel++;
                    string[] names = eventName.ToArray();
                    string[] values = eventValue.ToArray();
                    for( int i = 0 ; i < names.Length ; i++ )
                    {
                        EditorGUILayout.BeginHorizontal();
                        names[i] = editor.EditableTextField(eventNameLabel,names[i]);
                        values[i] = editor.EditableTextField(eventValueLabel,values[i]);
                        if( GUILayout.Button("Remove",GUILayout.Width(70)) )
                        {
                            names[i] = null;
                            values[i] = null;
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    for( int i = 0 ; i < names.Length ; i++ )
                    {
                        eventName[i] = names[i];
                        eventValue[i] = values[i];
                    }

                    for( int i = 0 ; i < names.Length ; i++ )
                    {
                        if( names[i] == null && values[i] == null )
                        {
                            eventName.RemoveAt(i);
                            eventValue.RemoveAt(i);
                        }
                    }

                    if( GUILayout.Button("Add Event") )
                    {
                        eventName.Add("New Event");
                        eventValue.Add("No Value");
                    }

                    EditorGUI.indentLevel--;
                }     
            }
        }

        public class AdjustTracker : Tracker {
            public string id = "";
            public string key = "";
            public string secretId = "";
            public string secretInfo1 = "";
            public string secretInfo2 = "";
            public string secretInfo3 = "";
            public string secretInfo4 = "";

            public override void OnGUI(HiveConfigEditor editor) {
                EditorGUILayout.BeginHorizontal();
                key = editor.EditableTextField( appToken, key);
                EditorGUILayout.EndHorizontal();
                
                // Start to Show App Secret Key Edit Field
                EditorGUILayout.LabelField(appSecretTitle, GUILayout.Height(16));
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                secretId = editor.VerticalEditableTextField(appSecretID, secretId);
                secretInfo1 = editor.VerticalEditableTextField(appInfo1Values, secretInfo1);
                secretInfo2 = editor.VerticalEditableTextField(appInfo2Values, secretInfo2);
                secretInfo3 = editor.VerticalEditableTextField(appInfo3Values, secretInfo3);
                secretInfo4 = editor.VerticalEditableTextField(appInfo4Values, secretInfo4);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;

                base.OnGUI(editor);
            }
        
        }

        public class SingularTracker : Tracker{
            public string id = "";
            public string key = "";

            public override void OnGUI(HiveConfigEditor editor) {

                EditorGUILayout.BeginHorizontal();
                id = editor.EditableTextField( appKeySingular, id);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                key = editor.EditableTextField ( secretKeySingular, key);
                EditorGUILayout.EndHorizontal();

                base.OnGUI(editor);
            }
        }

        public class AppsFlyerTracker : Tracker{
            public string id = "";
            public string key = "";
            public string itunesConnectAppId = "";

            public override void OnGUI(HiveConfigEditor editor) {
                EditorGUILayout.BeginHorizontal();
                key = editor.EditableTextField ( secretKeyAppsFlyer, key);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                itunesConnectAppId = editor.EditableTextField ( secretiTunesConnectIDAppsFlyer, itunesConnectAppId);
                EditorGUILayout.EndHorizontal();
                base.OnGUI(editor);
            }
        }

        public class FirebaseTracker : Tracker{
            public override void OnGUI(HiveConfigEditor editor) {
                base.OnGUI(editor);
            }
        }



		private GUILayoutOption[] fieldName = {GUILayout.Width(200),GUILayout.ExpandWidth(false)};
		private GUILayoutOption[] both = {GUILayout.Width(250),GUILayout.ExpandWidth(false)};
        public override void OnInspectorGUI()
        {
            // #if !(UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE)
            EditorGUILayout.LabelField("HiveConfig Settings is Enable to Android & iOS only.");
            // if( GUILayout.Button("Switch to Target Android") ){
            //     EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
            //     SetDirty();
            // }
            // if( GUILayout.Button("Switch to Target iOS") ){
            //     EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
            //     SetDirty();
            // }
            // #else

            //platformShown = System.Enum.Parse(ConfigPlatform, platformShown);


            SelectedIndex = GUILayout.SelectionGrid(SelectedIndex,platform,platform.Length);
            if( SelectedIndex == 0 )
            {
                platformShown = ConfigPlatform.Android;
                SelectedPlatformPostfix = " - [Android]";
            }
            else if( SelectedIndex == 1 )
            {
                platformShown = ConfigPlatform.iOS;
                SelectedPlatformPostfix = " - [iOS]";
            }
            else if (SelectedIndex == 2)
            {
                platformShown = ConfigPlatform.macOS;
                SelectedPlatformPostfix = " - [macOS]";
            }
            else if (SelectedIndex == 3)
            {
                platformShown = ConfigPlatform.Windows;
                SelectedPlatformPostfix = " - [Windows]";
            }
            else
            {
                platformShown = ConfigPlatform.Unkown;
                SelectedPlatformPostfix = " - [Unknown]";
            }

            if( platformShown == ConfigPlatform.Android ){
                if( SelectedPlatform != HiveConfigXML.Android )
                {
                    //EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
                    EditorUtility.SetDirty(this);
                } 
                SelectedPlatform = HiveConfigXML.Android;
            }
            else if(platformShown == ConfigPlatform.iOS){
                if( SelectedPlatform != HiveConfigXML.iOS )
                {
                    //EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
                    EditorUtility.SetDirty(this);
                } 
                SelectedPlatform = HiveConfigXML.iOS;
            }
            else if (platformShown == ConfigPlatform.macOS)
            {
                if (SelectedPlatform != HiveConfigXML.macOS)
                {
                    //EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
                    EditorUtility.SetDirty(this);
                }
                SelectedPlatform = HiveConfigXML.macOS;
            }
            else if (platformShown == ConfigPlatform.Windows)
            {
                if (SelectedPlatform != HiveConfigXML.Windows)
                {
                    //EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
                    EditorUtility.SetDirty(this);
                }
                SelectedPlatform = HiveConfigXML.Windows;
            }


            EditorGUILayout.Separator();
            this.AutoValidationGUI();
            this.AppIdGUI();

            if (SelectedPlatform == HiveConfigXML.Android) {
                EditorGUILayout.Separator();
                this.HiveIAPGUI();
            }

            if( SelectedPlatform != null )
            {
                EditorGUILayout.Separator();
                this.HiveConfigGUI();
            }

            EditorGUILayout.Separator();
            this.AndroidUtilGUI();

            EditorGUILayout.Separator();
            this.IOSUtilGUI();

            EditorGUILayout.Separator();
            this.macOSUtilGUI();

            EditorGUILayout.Separator();
            this.WindowsUtilGUI();


            // TODO :Tracking

            
            //EditorGUILayout.Separator();
            //this.AppLinksUtilGUI();

            //EditorGUILayout.Separator();
            //this.AboutGUI();
            
            // #endif 
        }

        private void AutoValidationGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(autoValidationLabel, fieldName);
            autoValidation = EditorGUILayout.Toggle(autoValidation, both);
            EditorGUILayout.EndHorizontal();
        }
        private void AppIdGUI()
        {
            if (SelectedPlatform == HiveConfigXML.Android || SelectedPlatform == HiveConfigXML.iOS)
            {

                EditorGUILayout.LabelField("Check which Authentication you use in HIVE SDK");

                // Choose  HIVE SDK Authentication version
                GUILayoutOption[] ToogleWidth = { GUILayout.Width(15), GUILayout.ExpandWidth(false) };
                GUILayoutOption[] ToogleTextWidth = { GUILayout.Width(85), GUILayout.ExpandWidth(false) };

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(chooseAuthTypeLabel, fieldName);

                SelectedPlatform.useAuthv1 = EditorGUILayout.Toggle(SelectedPlatform.useAuthv1, ToogleWidth);
                EditorGUILayout.LabelField(authV1Label, ToogleTextWidth);
                SelectedPlatform.useAuthv4 = EditorGUILayout.Toggle(SelectedPlatform.useAuthv4, ToogleWidth);
                EditorGUILayout.LabelField(authV4Label, ToogleTextWidth);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
            } else if (SelectedPlatform == HiveConfigXML.macOS || SelectedPlatform == HiveConfigXML.Windows)
            {
                SelectedPlatform.useAuthv1 = false;
                SelectedPlatform.useAuthv4 = true;
            }
                // Setup HIVE App ID
                EditorGUILayout.LabelField("Setup the HIVE App Id associated with this game");
			if (!SelectedPlatform.IsValidAppId)
            {
                EditorGUILayout.HelpBox("Invalid Hive App Id", MessageType.Error);
            }
            EditorGUILayout.Space();
			// Added in HIVE SDK 4.5.0
			// appID
			EditorGUILayout.BeginHorizontal();
			SelectedPlatform.HIVEAppID = EditableTextField(appNameLabel, SelectedPlatform.HIVEAppID); 
			EditorGUILayout.EndHorizontal ();


            // Added in HIVE SDK 4.14.0
            EditorGUILayout.BeginHorizontal();
            SelectedPlatform.universalLink = EditableTextField (universalLinkLabel, SelectedPlatform.universalLink);
            EditorGUILayout.EndHorizontal ();

            this.showFacebook = EditorGUILayout.Foldout(this.showFacebook, "Facebook Settings" + SelectedPlatformPostfix);
            if (this.showFacebook)
            {
                // Added in HIVE SDK 4.5.0
                // Facebook ID
                EditorGUILayout.BeginHorizontal();
                SelectedPlatform.facebookAppID = EditableTextField(facebookAppIdLabel, SelectedPlatform.facebookAppID);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                SelectedPlatform.facebookClientToken = EditableTextField(facebookClientTokenLabel, SelectedPlatform.facebookClientToken);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Facebook Permissions");
                EditorGUILayout.EndHorizontal();

                FacebookPermissions permissions = SelectedPlatform.facebookPermissions;
                if (permissions != null)
                {
                    permissions.OnGUI(this);
                }
                SelectedPlatform.facebookPermissions = permissions;
            }

            if (SelectedPlatform == HiveConfigXML.Android)
            {
                // Google Play in Android

                this.showGooglePlay = EditorGUILayout.Foldout(this.showGooglePlay, "Google Settings" + SelectedPlatformPostfix);

                if (this.showGooglePlay)
                {
                    // Google Play App ID
                    EditorGUILayout.BeginHorizontal();
                    SelectedPlatform.googlePlayAppID = EditableTextField(googleAppIdLabel, SelectedPlatform.googlePlayAppID);
                    EditorGUILayout.EndHorizontal();


                    if (SelectedPlatform.useAuthv4)
                    {
                        // if use Auth v4
                        // Added in HIVE SDK 4.5.0
                        // Google Server Client ID
                        EditorGUILayout.BeginHorizontal();
                        SelectedPlatform.googleServerClientID = EditableTextField(googleServerClientIdLabel, SelectedPlatform.googleServerClientID);
                        EditorGUILayout.EndHorizontal();

                    }

                    EditorGUILayout.BeginHorizontal();
                    SelectedPlatform.googlePlayGamesAppID = EditableTextField(googlePlayGamesAppIdLabel, SelectedPlatform.googlePlayGamesAppID);
                    EditorGUILayout.EndHorizontal();


                    if (SelectedPlatform.useAuthv4)
                    {
                        EditorGUILayout.BeginHorizontal();
                        SelectedPlatform.googlePlayGamesServerClientID = EditableTextField(googlePlayGamesServerClientIdLabel, SelectedPlatform.googlePlayGamesServerClientID);
                        EditorGUILayout.EndHorizontal();

                    }
                }


            }
            else if (SelectedPlatform == HiveConfigXML.iOS)
            {
                // Google Play in iOS
                this.showGooglePlay = EditorGUILayout.Foldout(this.showGooglePlay, "Google Play Settings" + SelectedPlatformPostfix);

                if (this.showGooglePlay)
                {
                    // Added in HIVE SDK 4.5.0 in Auth V1
                    // Added in HIVE SDK 4.11.0 in Auth V4
                    // Google Service Client ID
                    EditorGUILayout.BeginHorizontal();
                    SelectedPlatform.googleClientID = EditableTextField(googleClientIdLabel, SelectedPlatform.googleClientID);
                    EditorGUILayout.EndHorizontal();

                    // Added in HIVE SDK 4.5.0 in Auth V1
                    // Added in HIVE SDK 4.11.0 in Auth V4
                    // Google Reversed Client ID
                    EditorGUILayout.BeginHorizontal();
                    SelectedPlatform.googleReversedClientID = EditableTextField(googleReversedClientIdLabel, SelectedPlatform.googleReversedClientID);
                    EditorGUILayout.EndHorizontal();

                    if (SelectedPlatform.useAuthv4)
                    {
                        EditorGUILayout.BeginHorizontal();
                        SelectedPlatform.googleServerClientID = EditableTextField(googleServerClientIdLabel, SelectedPlatform.googleServerClientID);
                        EditorGUILayout.EndHorizontal();
                    }
                }

            } else if (SelectedPlatform == HiveConfigXML.macOS || SelectedPlatform == HiveConfigXML.Windows)
            {
                EditorGUILayout.BeginHorizontal();
                SelectedPlatform.googleClientID = EditableTextField(googleClientIdLabel, SelectedPlatform.googleClientID);
                EditorGUILayout.EndHorizontal();
            }

            if (SelectedPlatform == HiveConfigXML.Android || SelectedPlatform == HiveConfigXML.iOS)
            {
                // QQ AppID
                if (SelectedPlatform.useAuthv4)
                {
                    this.showQQAppId = EditorGUILayout.Foldout(this.showQQAppId, "QQ App ID Settings" + SelectedPlatformPostfix);
                    if (this.showQQAppId)
                    {
                        // Added in HIVE SDK 4.5.0
                        // QQ AppID
                        EditorGUILayout.BeginHorizontal();
                        SelectedPlatform.qqAppId = EditableTextField(qqAppIdLabel, SelectedPlatform.qqAppId);
                        EditorGUILayout.EndHorizontal();
                    }
                }

                // weChat AppID
                if (SelectedPlatform.useAuthv4)
                {
                    this.showWeChatAppId = EditorGUILayout.Foldout(this.showWeChatAppId, "WeChat App ID Settings" + SelectedPlatformPostfix);
                    if (this.showWeChatAppId)
                    {
                        // Added in HIVE SDK 4.6.0
                        EditorGUILayout.BeginHorizontal();
                        SelectedPlatform.weChatAppId = EditableTextField(weChatAppIdLabel, SelectedPlatform.weChatAppId);
                        EditorGUILayout.EndHorizontal();

                        // Added in HIVE SDK 4.6.0
                        EditorGUILayout.BeginHorizontal();
                        SelectedPlatform.weChatAppSecret = EditableTextField(weChatAppSecretLabel, SelectedPlatform.weChatAppSecret);
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }

            if(SelectedPlatform == HiveConfigXML.iOS)
            {
                // VK AppID
                if (SelectedPlatform.useAuthv4)
                {
                    this.showVKAppId = EditorGUILayout.Foldout(this.showVKAppId, "VK App ID Settings" + SelectedPlatformPostfix);
                    if (this.showVKAppId)
                    {
                        // Added in HIVE SDK 4.6.0
                        EditorGUILayout.BeginHorizontal();
                        SelectedPlatform.vkAppId = EditableTextField(vkAppIdLabel, SelectedPlatform.vkAppId);
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }

            if (SelectedPlatform == HiveConfigXML.Android || SelectedPlatform == HiveConfigXML.iOS)
            {
                // Line ChannelId
                if (SelectedPlatform.useAuthv4) {
                    this.showLineChannelId = EditorGUILayout.Foldout (this.showLineChannelId, "Line ChannelId Settings" + SelectedPlatformPostfix);
                    if (this.showLineChannelId) {
                        // Added in HIVE SDK 4.15.2
                        EditorGUILayout.BeginHorizontal();
                        SelectedPlatform.lineChannelId = EditableTextField (lineChannelIdLabel, SelectedPlatform.lineChannelId);
                        EditorGUILayout.EndHorizontal ();
                    }
                }
            }

            // Weverse ClientId
            if (SelectedPlatform.useAuthv4) {
                this.showWeverseClientId = EditorGUILayout.Foldout (this.showWeverseClientId, "Weverse ClientId Settings" + SelectedPlatformPostfix);
                if (this.showWeverseClientId) {
                    // Added in HIVE SDK 4.15.2
                    EditorGUILayout.BeginHorizontal();
                    SelectedPlatform.weverseClientId = EditableTextField (weverseClientIdLabel, SelectedPlatform.weverseClientId);
                    EditorGUILayout.EndHorizontal ();
                }
            }

            // SignInWithApple ServiceId
            if (SelectedPlatform.useAuthv4) {
                this.showSignInWithAppleServiceId = EditorGUILayout.Foldout (this.showSignInWithAppleServiceId, "Apple ServiceId Settings" + SelectedPlatformPostfix);
                if (this.showSignInWithAppleServiceId) {
                    // Added in HIVE SDK 4.15.6
                    EditorGUILayout.BeginHorizontal();
                    SelectedPlatform.signInWithAppleServiceId = EditableTextField (signInWithAppleServiceIdLabel, SelectedPlatform.signInWithAppleServiceId);
                    EditorGUILayout.EndHorizontal ();
                }
            }

			EditorGUILayout.Space();
        }

        private void HiveIAPGUI() {
            this.showIAPGUI = EditorGUILayout.Foldout(this.showIAPGUI, "InApp Settings " + SelectedPlatformPostfix);
            if (this.showIAPGUI) {
                EditorGUILayout.BeginHorizontal();
                SelectedPlatform.weChatPaymentKey = EditableTextField (weChatPaymentKeyLabel, SelectedPlatform.weChatPaymentKey);
                EditorGUILayout.EndHorizontal();
            }

        }
        private void HiveConfigGUI()
        {
            //HiveConfigXML.Android.load();
            
            // GUILayoutOption[] platformField = {GUILayout.Width(80),GUILayout.ExpandWidth(false)};
            // GUILayoutOption[] field = {GUILayout.Width(170),GUILayout.ExpandWidth(false)};
            
            // System.Action localSeperator = () => {
            //     EditorGUILayout.BeginVertical();
            //     EditorGUILayout.LabelField("Android", platformField);
            //     EditorGUILayout.LabelField("iOS", platformField);
            //     EditorGUILayout.EndVertical();
            // };


            
            this.showHiveInitSettings = EditorGUILayout.Foldout(this.showHiveInitSettings, "HiveConfig Settings"+ SelectedPlatformPostfix);
            if (this.showHiveInitSettings)
            {
                //zone
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(zoneLabel, fieldName);
                SelectedPlatform.zone = (HiveConfigXML.ZoneType)EditorGUILayout.EnumPopup( SelectedPlatform.zone, both);
                // if( HiveConfigXML.Android.zone == HiveConfigXML.iOS.zone )
                // {
                //     HiveConfigXML.Android.zone = HiveConfigXML.iOS.zone = (HiveConfigXML.ZoneType)EditorGUILayout.EnumPopup( HiveConfigXML.Android.zone, both);
                // }
                // else
                // {
                //     EditorGUILayout.BeginHorizontal(both);
                //     localSeperator();
                //     EditorGUILayout.BeginVertical();
                //     HiveConfigXML.Android.zone = (HiveConfigXML.ZoneType)EditorGUILayout.EnumPopup( HiveConfigXML.Android.zone, field);
                //     HiveConfigXML.iOS.zone = (HiveConfigXML.ZoneType)EditorGUILayout.EnumPopup( HiveConfigXML.iOS.zone, field);
                //     EditorGUILayout.EndVertical();
                //     EditorGUILayout.EndHorizontal();
                // }
                EditorGUILayout.EndHorizontal();

                //hivePermissionViewOn
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(hivePermissionViewOnLabel, fieldName);
                SelectedPlatform.hivePermissionViewOn = EditorGUILayout.Toggle( SelectedPlatform.hivePermissionViewOn, both);
                EditorGUILayout.EndHorizontal();

                //useLog
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(loggingLabel, fieldName);
                SelectedPlatform.useLog = EditorGUILayout.Toggle( SelectedPlatform.useLog, both);
                // if( HiveConfigXML.Android.useLog == HiveConfigXML.iOS.useLog )
                // {
                //     HiveConfigXML.Android.useLog = HiveConfigXML.iOS.useLog = EditorGUILayout.Toggle( HiveConfigXML.Android.useLog, both);
                // }
                // else
                // {  
                //     EditorGUILayout.BeginHorizontal(both);
                //     localSeperator();
                //     EditorGUILayout.BeginVertical();
                //     HiveConfigXML.Android.useLog = EditorGUILayout.Toggle( HiveConfigXML.Android.useLog, field);
                //     HiveConfigXML.iOS.useLog = EditorGUILayout.Toggle( HiveConfigXML.iOS.useLog, field);
                //     EditorGUILayout.EndVertical();
                //     EditorGUILayout.EndHorizontal();
                // }
                EditorGUILayout.EndHorizontal();

                // ageGateU13
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(ageGateU13Label, fieldName);
                SelectedPlatform.ageGateU13 = EditorGUILayout.Toggle( SelectedPlatform.ageGateU13, both);
                EditorGUILayout.EndHorizontal();
                // HiveConfigXML.Android.ageGateU13(false);

                //market
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(marketLabel, fieldName);
                SelectedPlatform.market = (HiveConfigXML.MarketType)EditorGUILayout.EnumPopup( SelectedPlatform.market, both);
                // if( HiveConfigXML.Android.market == HiveConfigXML.iOS.market )
                // {
                //     HiveConfigXML.Android.market = HiveConfigXML.iOS.market = (HiveConfigXML.MarketType)EditorGUILayout.EnumPopup( HiveConfigXML.Android.market, both);
                // }
                // else
                // {
                //     EditorGUILayout.BeginHorizontal(both);
                //     localSeperator();
                //     EditorGUILayout.BeginVertical();
                //     HiveConfigXML.Android.market = (HiveConfigXML.MarketType)EditorGUILayout.EnumPopup( HiveConfigXML.Android.market, field);
                //     HiveConfigXML.iOS.market = (HiveConfigXML.MarketType)EditorGUILayout.EnumPopup( HiveConfigXML.iOS.market, field);   
                //     EditorGUILayout.EndVertical();
                //     EditorGUILayout.EndHorizontal();
                // }
                EditorGUILayout.EndHorizontal();

                //HiveOrientation
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(hiveOrientationLabel, fieldName);
                SelectedPlatform.hiveOrientation = (HiveConfigXML.HiveOrientationType)EditorGUILayout.EnumPopup( SelectedPlatform.hiveOrientation, both);
                EditorGUILayout.EndHorizontal();

            }

            this.showHiveAdvanceSettings = EditorGUILayout.Foldout(this.showHiveAdvanceSettings, "HiveConfig Advance Settings"+ SelectedPlatformPostfix);
            if (this.showHiveAdvanceSettings)
            {
                //httpConnectTimeout
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(httpConnectTimeoutLabel, fieldName);
                SelectedPlatform.httpConnectTimeout = EditorGUILayout.IntSlider( SelectedPlatform.httpConnectTimeout, 1, 60, both);
                // if( HiveConfigXML.Android.httpConnectTimeout == HiveConfigXML.iOS.httpConnectTimeout )
                // {
                //     HiveConfigXML.Android.httpConnectTimeout = HiveConfigXML.iOS.httpConnectTimeout = EditorGUILayout.IntSlider( HiveConfigXML.Android.httpConnectTimeout, 1, 60, both);
                // }
                // else
                // {
                //     EditorGUILayout.BeginHorizontal(both);
                //     localSeperator();
                //     EditorGUILayout.BeginVertical();
                //     HiveConfigXML.Android.httpConnectTimeout = EditorGUILayout.IntSlider( HiveConfigXML.Android.httpConnectTimeout, 1, 60, field);
                //     HiveConfigXML.iOS.httpConnectTimeout = EditorGUILayout.IntSlider( HiveConfigXML.iOS.httpConnectTimeout, 1, 60, field);
                //     EditorGUILayout.EndVertical();
                //     EditorGUILayout.EndHorizontal();
                // }
                EditorGUILayout.EndHorizontal();

                //httpReadTimeout
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(httpReadTimeoutLabel, fieldName);
                SelectedPlatform.httpReadTimeout = EditorGUILayout.IntSlider( SelectedPlatform.httpReadTimeout, 1, 60, both);
                // if( HiveConfigXML.Android.httpReadTimeout == HiveConfigXML.iOS.httpReadTimeout )
                // {
                //     HiveConfigXML.Android.httpReadTimeout = HiveConfigXML.iOS.httpReadTimeout = EditorGUILayout.IntSlider( HiveConfigXML.Android.httpReadTimeout, 1, 60, both);
                // }
                // else
                // {
                //     EditorGUILayout.BeginHorizontal(both);
                //     localSeperator();
                //     EditorGUILayout.BeginVertical();
                //     HiveConfigXML.Android.httpReadTimeout = EditorGUILayout.IntSlider( HiveConfigXML.Android.httpReadTimeout, 1, 60, field);
                //     HiveConfigXML.iOS.httpReadTimeout = EditorGUILayout.IntSlider( HiveConfigXML.iOS.httpReadTimeout, 1, 60, field);
                //     EditorGUILayout.EndVertical();
                //     EditorGUILayout.EndHorizontal();
                // }
                EditorGUILayout.EndHorizontal();

                //maxGameLogSize
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(maxGameLogSizeLabel, fieldName);
                SelectedPlatform.maxGameLogSize = EditorGUILayout.IntSlider( SelectedPlatform.maxGameLogSize, 0, 1000, both);
                // if( HiveConfigXML.Android.maxGameLogSize == HiveConfigXML.iOS.maxGameLogSize )
                // {
                //     HiveConfigXML.Android.maxGameLogSize = HiveConfigXML.iOS.maxGameLogSize = EditorGUILayout.IntSlider( HiveConfigXML.Android.maxGameLogSize, 0, 1000, both);
                // }
                // else
                // {
                //     EditorGUILayout.BeginHorizontal(both);
                //     localSeperator();
                //     EditorGUILayout.BeginVertical();
                //     HiveConfigXML.Android.maxGameLogSize = EditorGUILayout.IntSlider( HiveConfigXML.Android.maxGameLogSize, 0, 1000, field);
                //     HiveConfigXML.iOS.maxGameLogSize = EditorGUILayout.IntSlider( HiveConfigXML.iOS.maxGameLogSize, 0, 1000, field);
                //     EditorGUILayout.EndVertical();
                //     EditorGUILayout.EndHorizontal();
                // }
                EditorGUILayout.EndHorizontal();

                //analyticsSendLimit
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(analyticsSendLimitLabel, fieldName)   ;
                long tempSendLimit = EditorGUILayout.LongField(SelectedPlatform.analyticsSendLimit);
                if(tempSendLimit > 0) {
                    SelectedPlatform.analyticsSendLimit = (uint)tempSendLimit;
                }
                EditorGUILayout.EndHorizontal();

                //analyticsQueueLimit
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(analyticsQueueLimitLabel, fieldName)   ;
                long tempQueueLimit = EditorGUILayout.LongField(SelectedPlatform.analyticsQueueLimit);
                if(tempSendLimit > 0) {
                    SelectedPlatform.analyticsQueueLimit = (uint)tempQueueLimit;
                }
                EditorGUILayout.EndHorizontal();

                //analyticsSendCycle
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(analyticsSendCycleLabel, fieldName)   ;
                float tempCycle = EditorGUILayout.FloatField(SelectedPlatform.analyticsSendCycle);
                if(tempSendLimit > 0) {
                    SelectedPlatform.analyticsSendCycle = tempCycle;
                }
                EditorGUILayout.EndHorizontal();

                //useCrashReport
                if (SelectedPlatform == HiveConfigXML.iOS) {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(useCrashReportLabel, fieldName);
                    SelectedPlatform.useCrashReport = EditorGUILayout.Toggle( SelectedPlatform.useCrashReport, both);
                    EditorGUILayout.EndHorizontal();
                }
            }


            if (SelectedPlatform == HiveConfigXML.Android || SelectedPlatform == HiveConfigXML.iOS)
            {
                this.showMobileAppTrackerSettings = EditorGUILayout.Foldout(this.showMobileAppTrackerSettings, "HiveConfig Mobile App Tracking Settings" + SelectedPlatformPostfix);
                if (this.showMobileAppTrackerSettings)
                {
                    //Adjust
                    EditorGUI.indentLevel++;
                    this.showAdjust = EditorGUILayout.Foldout(this.showAdjust, "Adjust" + SelectedPlatformPostfix);
                    if (this.showAdjust)
                    {
                        Tracker tracker = SelectedPlatform.Adjust;
                        if (tracker != null)
                        {
                            tracker.OnGUI(this);
                        }
                        SelectedPlatform.Adjust = tracker;
                    }
                    EditorGUI.indentLevel--;

                    // Singular
                    EditorGUI.indentLevel++;
                    this.showSingular = EditorGUILayout.Foldout(this.showSingular, "Singular" + SelectedPlatformPostfix);
                    if (this.showSingular)
                    {
                        Tracker tracker = SelectedPlatform.Singular;
                        if (tracker != null)
                        {
                            tracker.OnGUI(this);
                        }
                        SelectedPlatform.Singular = tracker;
                    }
                    EditorGUI.indentLevel--;

                    // Adjust
                    EditorGUI.indentLevel++;
                    this.showAppsFlyer = EditorGUILayout.Foldout(this.showAppsFlyer, "AppsFlyer" + SelectedPlatformPostfix);
                    if (this.showAppsFlyer)
                    {
                        Tracker tracker = SelectedPlatform.AppsFlyer;
                        if (tracker != null)
                        {
                            tracker.OnGUI(this);
                        }
                        SelectedPlatform.AppsFlyer = tracker;
                    }
                    EditorGUI.indentLevel--;

                    // Firebase
                    EditorGUI.indentLevel++;
                    this.showAppsFlyer = EditorGUILayout.Foldout(this.showFirebase, "Firebase" + SelectedPlatformPostfix);
                    if (this.showFirebase)
                    {
                        Tracker tracker = SelectedPlatform.Firebase;
                        if (tracker != null)
                        {
                            tracker.OnGUI(this);
                        }
                        SelectedPlatform.Firebase = tracker;
                    }
                    EditorGUI.indentLevel--;

                }
            }
            EditorGUILayout.Space();
        }

        private void IOSUtilGUI()
        {
            // this.showIOSSettings = EditorGUILayout.Foldout(this.showIOSSettings, "iOS Build Settings");
            // if (this.showIOSSettings)
            {
                //EditorGUILayout.BeginHorizontal();

				if( HiveConfigXML.iOS.IsValidFacebookAppId )
                {
					this.ReadOnlyTextField(urlFacebookSuffixLabel, "fb"+HiveConfigXML.iOS.facebookAppID);
                }
				this.ReadOnlyTextField(urlHiveLabel, HiveConfigXML.iOS.HIVEAppID);

                //EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Regenerate iOS Plist & Hive Config"))
                {
                    //HiveManifestMod.GenerateManifest();
                    // HiveConfigXML.Instance.load();
                    // HiveConfigXML.Instance.commit();
                    HiveConfigXML.iOS.commit();
                }
            }

            EditorGUILayout.Space();
        }

        static public void PrepareAndroidSetting(){
            
            /*
            * TODO
            * CreateAttribute 를 통해 속성 추가시 namespace prefix 'tools:' 가 정상적으로 붙지 않는 이슈 존재
            */
            HiveManifestMod.GenerateManifest();
            HiveManifestMod.GenerateManifest();
            // HiveConfigXML.Instance.load();
            // HiveConfigXML.Instance.commit();
            HiveMultidexResolver.resolveMultidex();
            HiveConfigXML.Android.commit();

            /*
            if android build system was Gradle(New!) and UnityVersion after than 5.6.0p1 or 5.5.2p4, skip below copy process. except 2017.1 and after version.
            5.6.0p1
            (888274) - Android: Fixed an issue where ApplicationId was missing from AndroidManifest.xml in Gradle builds
            https://issuetracker.unity3d.com/issues/android-applicationid-is-not-being-provided-in-the-build-dot-gradle
            5.5.2p4
            (888274) - Android: Fixed an issue where ApplicationId was missing from AndroidManifest.xml in Gradle builds.
             */
            bool bGradleApplicationIdBugFixed = false;
            #if UNITY_5_5_OR_NEWER
            int version = Utility.GetUnityVersion();
            if( EditorUserBuildSettings.androidBuildSystem == AndroidBuildSystem.Gradle ) {
                if( (version >= 5524 && version < 5600) ||
                    (version >= 5601 && version < 5700) ||
                    (version >= 2017100) ) {
                    bGradleApplicationIdBugFixed = true;
                }
            } else {
                if( !EditorPrefs.GetBool("noShowGradle",false) ) {
                    int result = EditorUtility.DisplayDialogComplex(
                    "Select Android Build System.",
                    "We recommand Gradle Build for Android Build System. Do you want change to Gradle Build?",
                    "Yes, I will change Now.",
                    "No",
                    "No, I Don't want see any more.");

                    if ( result == 0 ) {
                        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
                        bGradleApplicationIdBugFixed = true;
                    } else if ( result == 2 ) {
                        EditorPrefs.SetBool("noShowGradle",true);
                    }
                }
            }
            #endif

            string sdkPath = "Assets/Hive_SDK_v4/Plugins/Android/libs/";
            string pluginPath = "Assets/Plugins/Android/";
            string SDKFile = "HIVE_SDK.aar";
            string SDKPluginFile = "HIVE_SDK_UnityPlugin.aar";
            string SDKDirectory = "HIVE_SDK";
            string SDKPluginDirectory = "HIVE_SDK_UnityPlugin";
            if( File.Exists(sdkPath+SDKFile) || File.Exists(sdkPath+SDKPluginFile) )
            {
                UnityEngine.Debug.Log("Reimport Assets/Hive_SDK_v4/Plugins");

                if( File.Exists(sdkPath+SDKFile) )
                {
                    PluginImporter hiveBundle = (PluginImporter)PluginImporter.GetAtPath(sdkPath+SDKFile);
                    hiveBundle.SetCompatibleWithAnyPlatform(false);
                    hiveBundle.SetCompatibleWithEditor(false);
                    hiveBundle.SetCompatibleWithPlatform(BuildTarget.Android, bGradleApplicationIdBugFixed);
                    hiveBundle.SaveAndReimport();
                    if( bGradleApplicationIdBugFixed ) {
                        FileUtil.DeleteFileOrDirectory(pluginPath+SDKDirectory);
                    } else if( !File.Exists(pluginPath+SDKFile) )
                        FileUtil.CopyFileOrDirectory(sdkPath+SDKFile,pluginPath+SDKFile);
                }
                if( File.Exists(sdkPath+SDKPluginFile) )
                {
                    PluginImporter hiveBundle = (PluginImporter)PluginImporter.GetAtPath(sdkPath+SDKPluginFile);
                    hiveBundle.SetCompatibleWithAnyPlatform(false);
                    hiveBundle.SetCompatibleWithEditor(false);
                    hiveBundle.SetCompatibleWithPlatform(BuildTarget.Android, bGradleApplicationIdBugFixed);
                    hiveBundle.SaveAndReimport();
                    if( bGradleApplicationIdBugFixed ) {
                        FileUtil.DeleteFileOrDirectory(pluginPath+SDKPluginDirectory);
                    } else if( !File.Exists(pluginPath+SDKPluginFile) )
                        FileUtil.CopyFileOrDirectory(sdkPath+SDKPluginFile,pluginPath+SDKPluginFile);
                }
            }

            //Just once fire!
            HiveDependencies.RegisterAndroidDependencies();
            Google.VersionHandler.UpdateNow();
            GooglePlayServices.PlayServicesResolver.MenuForceResolve();

            if(Directory.Exists(pluginPath+SDKDirectory))
            {
                PluginImporter hiveBundle = (PluginImporter)PluginImporter.GetAtPath(pluginPath+SDKDirectory);
                hiveBundle.SetCompatibleWithAnyPlatform(false);
                hiveBundle.SetCompatibleWithEditor(false);
                hiveBundle.SetCompatibleWithPlatform(BuildTarget.Android,true);
                hiveBundle.SaveAndReimport();
            }

            if(Directory.Exists(pluginPath+SDKPluginDirectory))
            {
                PluginImporter hiveBundle = (PluginImporter)PluginImporter.GetAtPath(pluginPath+SDKPluginDirectory);
                hiveBundle.SetCompatibleWithAnyPlatform(false);
                hiveBundle.SetCompatibleWithEditor(false);
                hiveBundle.SetCompatibleWithPlatform(BuildTarget.Android,true);
                hiveBundle.SaveAndReimport();
            }
        }

        private void AndroidUtilGUI()
        {
            // this.showAndroidUtils = EditorGUILayout.Foldout(this.showAndroidUtils, "Android Build Settings");
            // if (this.showAndroidUtils)
            {

				this.ReadOnlyTextField(packageNameLabel, HiveConfigXML.Android.HIVEAppID);
                this.ReadOnlyTextField(classNameLabel, HiveManifestMod.HiveUnityMainActivity);

                HiveConfigXML.Android.sdwritePermission = EditorGUILayout.Toggle(writeExternalStoragePermissionLabel, HiveConfigXML.Android.sdwritePermission);

                if (GUILayout.Button("Regenerate Android Manifest & Hive Config & Import HIVE SDK, Google Play Services (Game, etc)"))
                {
                    PrepareAndroidSetting();
                }
                // if( EditorUserBuildSettings.exportAsGoogleAndroidProject )
                // {
                //     if (GUILayout.Button("Export Android Studio Project"))
                //     {
                //         //BuildPipeline
                //     }
                // }
            }

            EditorGUILayout.Space();
        }
        private void macOSUtilGUI()
        {
            {
                if (GUILayout.Button("macOS Button"))
                {
                    HiveConfigXML.macOS.commit();
                }
            }
            EditorGUILayout.Space();
        }

        private void WindowsUtilGUI()
        {
            {
                if (GUILayout.Button("Windows Button"))
                {
                    HiveConfigXML.Windows.commit();
                }
            }
            EditorGUILayout.Space();
        }

        private void ReadOnlyTextField(GUIContent label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(200), GUILayout.Height(16));
            EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
            EditorGUILayout.EndHorizontal();
        }

        private string EditableTextField(GUIContent label, string value)
        {
            string returnValue;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.MaxWidth(150), GUILayout.MinWidth(30), GUILayout.Height(16));
            returnValue = EditorGUILayout.DelayedTextField(value, GUILayout.Height(16));
            EditorGUILayout.EndHorizontal();
            return returnValue;
        }
        private string VerticalEditableTextField(GUIContent label, string value) 
        {
            string returnValue;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(label, GUILayout.MaxWidth(150), GUILayout.MinWidth(30), GUILayout.Height(16));
            returnValue = EditorGUILayout.DelayedTextField(value, GUILayout.Height(16));
            EditorGUILayout.EndVertical();
            return returnValue;

        }

        public static bool isAutoValidation(){
            return autoValidation;
        }
    }
}
