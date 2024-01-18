/**
 * @file    HiveManifestMod.cs
 * 
 * @author  nanomech
 * @date    2016-2022
 * @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 * @defgroup Hive.Unity.Editor
 * @{
 * @brief HIVE SDK AndroidManifest.xml 수정 모드 <br/><br/>
 */

namespace UnityEditor.HiveEditor
{
    using System.Collections.Generic;
    using System.Globalization;
    using System;
    using System.IO;
    using System.Xml;
    using System.Collections;
    using Hive.Unity.Editor;
    using UnityEditor;
    using UnityEngine;

    public class HiveManifestMod
    {
        public const string DefaultUnityMainActivity = "com.unity3d.player.UnityPlayerActivity";
        public const string HiveUnityMainActivity = "com.hive.UnityPlayerActivity";
        public const string ApplicationIdMetaDataName = "com.facebook.sdk.ApplicationId";
        public const string FacebookClientTokenMetaDataName = "com.facebook.sdk.ClientToken";
        public const string FacebookContentProviderName = "com.facebook.FacebookContentProvider";
        public const string FacebookContentProviderAuthFormat = "com.facebook.app.FacebookContentProvider{0}";
        public const string FacebookActivityName = "com.facebook.FacebookActivity";
        public const string AndroidManifestPath = "Plugins/Android/AndroidManifest.xml";
        public const string HiveDefaultAndroidManifestPath = "Hive_SDK_v4/Editor/android/DefaultAndroidManifest.xml";

        public static bool bAllowManifestMerge = false;

        public static void GenerateManifest()
        {
            #if UNITY_5_5_OR_NEWER
            /*
            5.5.3p4
            (898978) - Android: Fixed manifest merging with new Android SDK tools.
            5.6.0p4
            (none) - Android: Fixed manifest merging with new android sdk tools.
             */
            int version = Utility.GetUnityVersion();
            if( EditorUserBuildSettings.androidBuildSystem == AndroidBuildSystem.Gradle ) {
                if( (version >= 5534 && version < 5600) ||
                    (version >= 5604 && version < 5700) || 
                    (version >= 2017100) ) {
                    bAllowManifestMerge = true;
                }
            }
            #endif
            
            var outputFile = Path.Combine(Application.dataPath, HiveManifestMod.AndroidManifestPath);

            // Create containing directory if it does not exist
            Directory.CreateDirectory(Path.GetDirectoryName(outputFile));

            // only copy over a fresh copy of the AndroidManifest if one does not exist
            if (!File.Exists(outputFile))
            {
                HiveManifestMod.CreateDefaultAndroidManifest(outputFile);
            }

            UpdateManifest(outputFile);
        }

        public static bool CheckManifest()
        {
            bool result = true;
            var outputFile = Path.Combine(Application.dataPath, HiveManifestMod.AndroidManifestPath);
            if (!File.Exists(outputFile))
            {
                Debug.LogError("An android manifest must be generated for the HIVE SDK to work.  Go to Hive->Edit Config and press \"Regenerate Android Manifest\"");
                return false;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(outputFile);

            if (doc == null)
            {
                Debug.LogError("Couldn't load " + outputFile);
                return false;
            }

            XmlNode manNode = FindChildNode(doc, "manifest");
            XmlNode dict = FindChildNode(manNode, "application");

            if (dict == null)
            {
                Debug.LogError("Error parsing " + outputFile);
                return false;
            }

            // Comment : Merge from Facebook AAR
            // XmlElement loginElement;
            // if (!HiveManifestMod.TryFindElementWithAndroidName(dict, FacebookActivityName, out loginElement))
            // {
            //     Debug.LogError(string.Format("{0} is missing from your android manifest.  Go to Hive->Edit Config and press \"Regenerate Android Manifest\"", FacebookActivityName));
            //     result = false;
            // }

            return result;
        }

        public static void UpdateManifest(string fullPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fullPath);

            if (doc == null)
            {
                Debug.LogError("Couldn't load " + fullPath);
                return;
            }

            XmlNode manifestNode = FindChildNode(doc, "manifest");
            XmlNode applicationNode = FindChildNode(manifestNode, "application");

            if (applicationNode == null)
            {
                Debug.LogError("Error parsing " + fullPath);
                return;
            }

            string ns = applicationNode.GetNamespaceOfPrefix("android");
			string toolns = applicationNode.GetNamespaceOfPrefix ("tools");

			// add xmlns:tools in manifest
			XmlAttribute attr = doc.CreateAttribute("xmlns:tools");
			attr.Value = "http://schemas.android.com/tools";
			manifestNode.Attributes.Append (attr);

            UpdateMainManifest(doc, applicationNode, ns);

			//			<application android:allowBackup="false" android:hardwareAccelerated="true" tools:replace="android:allowBackup,android:hardwareAccelerated">
			//				<!-- 중략 -->
			//				</application>
			// TODO 
			XmlAttribute attr_ab = doc.CreateAttribute("android:allowBackup", ns);
			attr_ab.Value = "false";
			applicationNode.Attributes.Append(attr_ab);

            XmlAttribute attr_fbc = doc.CreateAttribute("android:fullBackupContent", ns);
			attr_fbc.Value = "false";
			applicationNode.Attributes.Append(attr_fbc);

			XmlAttribute attr_ha = doc.CreateAttribute ("android:hardwareAccelerated", ns);
			attr_ha.Value = "true";
			applicationNode.Attributes.Append (attr_ha);

            //android:usesCleartextTraffic="true"
            XmlAttribute attr_ca = doc.CreateAttribute ("android:usesCleartextTraffic", ns);
			attr_ca.Value = "true";
			applicationNode.Attributes.Append (attr_ca);

			XmlAttribute attr_tr = doc.CreateAttribute("tools:replace", toolns);
			attr_tr.Value = "android:allowBackup,android:hardwareAccelerated,android:fullBackupContent";
			applicationNode.Attributes.Append (attr_tr);


			UpdateHiveAuthManifest(doc, applicationNode, ns, toolns);

            UpdateFacebookManifest(doc, applicationNode, ns);

            UpdateGooglePlayManifest(doc, applicationNode, ns);

			UpdateQQManifest (doc, applicationNode, ns, toolns);

            UpdatePermissionManifest(doc, manifestNode, ns, toolns);
            // KS SDK v1.19.4 검증 시 제외? dangerous permission 없이 테스트?

            // Save the document formatted
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(fullPath, settings))
            {
                doc.Save(xmlWriter);
            }
        }

        private static void UpdatePermissionManifest(XmlDocument doc, XmlNode xmlNode, string ns, string toolns)
        {
            // for resolve conflict vun
            XmlElement writeExternalStoragePermission = doc.CreateElement("uses-permission");
            writeExternalStoragePermission.SetAttribute("name", ns, "android.permission.WRITE_EXTERNAL_STORAGE");
            writeExternalStoragePermission.SetAttribute("maxSdkVersion", ns, "22");
            writeExternalStoragePermission.SetAttribute("replace", toolns, "android:maxSdkVersion");
            SetOrReplaceXmlElement(xmlNode, writeExternalStoragePermission);
        }

        private static void UpdateMainManifest(XmlDocument doc, XmlNode xmlNode, string ns)
        {
            //<activity android:name="com.hive.UnityPlayerActivity" android:label="@string/app_name">
            //   <intent-filter>
            //     <action android:name="android.intent.action.MAIN" />
            //     <category android:name="android.intent.category.LAUNCHER" />
            //   </intent-filter>
            //   <meta-data android:name="unityplayer.UnityActivity" android:value="true" />
            // </activity>
            
            //ApplicationUnityMainActivity;//
            XmlElement mainActivity;
            if( TryFindElementWithAndroidName(xmlNode, DefaultUnityMainActivity, out mainActivity ) )
            {
                mainActivity.GetAttributeNode("name", ns).Value = HiveUnityMainActivity;
                // mainActivity.RemoveAttribute("name", ns);
                // mainActivity.SetAttribute("name", ns, HiveUnityMainActivity);
                
            }

            if( TryFindElementWithAndroidName(xmlNode, HiveUnityMainActivity, out mainActivity ) )
            {
				AddAppLinkingActivity(doc, mainActivity, ns, new List<string>(){HiveConfigXML.Android.HIVEAppID});
            }
                
            
            mainActivity.SetAttribute("configChanges", ns, "fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen|layoutDirection");
            mainActivity.SetAttribute("exported", ns, "true"); // for targetSDKversion 31
            

            // <intent-filter>
            //     <action android:name="android.intent.action.VIEW" />
            //     <category android:name="android.intent.category.DEFAULT" />
            //     <category android:name="android.intent.category.BROWSABLE" />
            //     <data android:scheme="com.com2us.misample.normal.freefull.google.global.android.common" />
            // </intent-filter>

            // Issue : 권한요청 팝업스킵. http://developers.withhive.com/guide/hiveset/setunityad   1.1.7  Unity에서 제공하는 권한 팝업 Disable
            // https://forum.unity3d.com/threads/unity-bug-fix-problem-android-6-urgent.367681/
            // <meta-data 
            // android:name="unityplayer.SkipPermissionsDialog" 
            // android:value="true"/>
            // int UnityVersion = Utility.GetUnityVersion();
            // Always
            // if( UnityVersion >= 533 || (UnityVersion < 500 && UnityVersion >= 470))
            // {
                XmlElement skipPermissionElement = doc.CreateElement("meta-data");
                skipPermissionElement.SetAttribute("name", ns, "unityplayer.SkipPermissionsDialog" );
                skipPermissionElement.SetAttribute("value", ns, "true");
                SetOrReplaceXmlElement(xmlNode, skipPermissionElement);
            // }

        }

		private static void UpdateHiveAuthManifest(XmlDocument doc, XmlNode xmlNode, string ns, string toolns)
        {
//			<activity
//			android:name="com.hive.auth.UserAgreeDialog"
//				android:screenOrientation="sensorLandscape"
//				tools:replace="android:screenOrientation"/>

			if( bAllowManifestMerge ) {
                string orient = SupportedOrientation ();
                XmlElement authAcitivityElement = doc.CreateElement("activity");
                authAcitivityElement.SetAttribute("name", ns, "com.hive.auth.UserAgreeDialog");
                authAcitivityElement.SetAttribute("screenOrientation", ns, orient);
                authAcitivityElement.SetAttribute("replace", toolns, "android:screenOrientation");

                HiveManifestMod.SetOrReplaceXmlElement(xmlNode, authAcitivityElement);
            }
        }

        private static void UpdateFacebookManifest(XmlDocument doc, XmlNode xmlNode, string ns)
        {
			if (!HiveConfigXML.Android.IsValidFacebookAppId)
            {
                Debug.Log("You didn't specify a Facebook app ID.  Please setup one using the Hive menu in the main Unity editor.");
                return;
            }
			string appId = HiveConfigXML.Android.facebookAppID;
            string clientToken = HiveConfigXML.Android.facebookClientToken;

            // add the app id
            // <meta-data android:name="com.facebook.sdk.ApplicationId" android:value="\ fb<APPID>" />
            XmlElement appIdElement = doc.CreateElement("meta-data");
            appIdElement.SetAttribute("name", ns, ApplicationIdMetaDataName);
            appIdElement.SetAttribute("value", ns, "fb" + appId);
            HiveManifestMod.SetOrReplaceXmlElement(xmlNode, appIdElement);

            XmlElement clientTokenElement = doc.CreateElement("meta-data");
            clientTokenElement.SetAttribute("name", ns, FacebookClientTokenMetaDataName);
            clientTokenElement.SetAttribute("value", ns, clientToken);
            HiveManifestMod.SetOrReplaceXmlElement(xmlNode, clientTokenElement);

            // Add the facebook content provider
            // <provider
            //   android:name="com.facebook.FacebookContentProvider"
            //   android:authorities="com.facebook.app.FacebookContentProvider<APPID>"
            //   android:exported="true" />
            XmlElement contentProviderElement = CreateContentProviderElement(doc, ns, appId);
            HiveManifestMod.SetOrReplaceXmlElement(xmlNode, contentProviderElement);

            // Remove the FacebookActivity since we can rely on it in the androidsdk aar as of v4.12
            // (otherwise unity manifest merge likes fail if there's any difference at all)
            XmlElement facebookElement;
            if (TryFindElementWithAndroidName(xmlNode, FacebookActivityName, out facebookElement)) {
                xmlNode.RemoveChild(facebookElement);
            }
        }

        private static void UpdateGooglePlayManifest(XmlDocument doc, XmlNode xmlNode, string ns)
        {

			if(!HiveConfigXML.Android.IsValidGooglePlayAppId)
            {
				if (HiveConfigXML.Android.useAuthv1) {
					Debug.Log("You didn't specify a GooglePlay app ID.  Please setup one using the Hive menu in the main Unity editor.");
				}
            }
            else{
				string appId = HiveConfigXML.Android.googlePlayAppID;

                // <meta-data 
                // android:name="com.google.android.gms.games.APP_ID" 
                // android:value="@string/app_id"/>
                XmlElement appIdElement = doc.CreateElement("meta-data");
                appIdElement.SetAttribute("name", ns, "com.google.android.gms.games.APP_ID" );
                appIdElement.SetAttribute("value", ns, appId + "\\");
                HiveManifestMod.SetOrReplaceXmlElement(xmlNode, appIdElement);   
            }
            // Remove meta-data .. android-sdk aar in GooglePlayService aready included
            // <meta-data 
            // android:name="com.google.android.gms.version" 
            // android:value="@integer/google_play_services_version"/>
            XmlElement googleplayElement;
            if (TryFindElementWithAndroidName(xmlNode, "com.google.android.gms.version", out googleplayElement, "meta-data")) {
                xmlNode.RemoveChild(googleplayElement);
            }
        }

		
		private static void UpdateQQManifest (XmlDocument doc, XmlNode xmlNode, string ns, string toolns)
		{
			if (!HiveConfigXML.Android.IsValidQQAppId) {
				Debug.Log ("You didn't specify a QQ AppID. Please setup one using the Hive menu in the main Unity editor.");
			} else {
				string qqAppId = HiveConfigXML.Android.qqAppId;
				
				//			<activity android:name="com.tencent.tauth.AuthActivity" android:launchMode="singleTask" android:noHistory="true">
				//				<intent-filter>
				//					<action android:name="android.intent.action.VIEW" />
				//					<category android:name="android.intent.category.DEFAULT" />
				//					<category android:name="android.intent.category.BROWSABLE" />
				//					<data android:scheme="tencent1106227203" />
				//					</intent-filter>
				//					</activity>
				//					<activity android:name="com.tencent.connect.common.AssistActivity" android:configChanges="orientation|keyboardHidden" android:screenOrientation="behind" android:theme="@android:style/Theme.Translucent.NoTitleBar" />
				XmlElement qqAuthAcitivityElement = doc.CreateElement("activity");
				qqAuthAcitivityElement.SetAttribute("name", ns, "com.tencent.tauth.AuthActivity");
				qqAuthAcitivityElement.SetAttribute("launchMode", ns, "singleTask");
				qqAuthAcitivityElement.SetAttribute("noHistory", ns, "true");
                qqAuthAcitivityElement.SetAttribute("exported", ns, "true"); // for targetSDKversion 31

				// Add intent filter
				AddAppLinkingActivity(doc, qqAuthAcitivityElement, ns, new List<string>() {"tencent"+HiveConfigXML.Android.qqAppId});
				HiveManifestMod.SetOrReplaceXmlElement(xmlNode, qqAuthAcitivityElement);

                if( bAllowManifestMerge ) {
                    string orient = SupportedOrientation ();
                    XmlElement qqAssistAcitivityElement = doc.CreateElement("activity");
                    qqAssistAcitivityElement.SetAttribute("name", ns, "com.tencent.connect.common.AssistActivity");
                    qqAssistAcitivityElement.SetAttribute("configChanges", ns, "orientation|keyboardHidden");
                    qqAssistAcitivityElement.SetAttribute("screenOrientation", ns, orient);	
                    qqAssistAcitivityElement.SetAttribute("theme", ns, "@android:style/Theme.Translucent.NoTitleBar");
                    qqAssistAcitivityElement.SetAttribute("replace", toolns, "android:screenOrientation,android:configChanges,android:theme");
                    HiveManifestMod.SetOrReplaceXmlElement(xmlNode, qqAssistAcitivityElement);
                }
			}

		
		}

        private static XmlNode FindChildNode(XmlNode parent, string name)
        {
            XmlNode curr = parent.FirstChild;
            while (curr != null)
            {
                if (curr.Name.Equals(name))
                {
                    return curr;
                }

                curr = curr.NextSibling;
            }

            return null;
        }

        private static XmlNode FindChildNode(XmlNode parent, string name, string attrName){
            XmlNode curr = parent.FirstChild;
            while (curr != null)
            {
                if (curr.Name.Equals(name))
                {
                    XmlAttributeCollection attributes = curr.Attributes;
                    IEnumerator ienum = attributes.GetEnumerator();
                    while(ienum.MoveNext())
                    {
                        XmlAttribute attr = (XmlAttribute)ienum.Current;
                        if(attr.LocalName.Equals("name") && attr.Value.Equals(attrName)){
                            return curr;
                        }
                    }

                }

                curr = curr.NextSibling;
            }

            return null;
        }

        private static void SetOrReplaceXmlElement(
            XmlNode parent,
            XmlElement newElement)
        {
            string attrNameValue = newElement.GetAttribute("name");
            string elementType = newElement.Name;

            XmlElement existingElment;
            if (TryFindElementWithAndroidName(parent, attrNameValue, out existingElment, elementType))
            {
                parent.ReplaceChild(newElement, existingElment);
            }
            else
            {
                parent.AppendChild(newElement);
            }
        }

        private static bool TryFindElementWithAndroidName(
            XmlNode parent,
            string attrNameValue,
            out XmlElement element,
            string elementType = "activity")
        {
            string ns = parent.GetNamespaceOfPrefix("android");
            var curr = parent.FirstChild;
            while (curr != null)
            {
                var currXmlElement = curr as XmlElement;
                if (currXmlElement != null &&
                    currXmlElement.Name == elementType &&
                    currXmlElement.GetAttribute("name", ns) == attrNameValue)
                {
                    element = currXmlElement;
                    return true;
                }

                curr = curr.NextSibling;
            }

            element = null;
            return false;
        }

        // private static void AddSimpleActivity(XmlDocument doc, XmlNode xmlNode, string ns, string className, bool export = false)
        // {
        //     XmlElement element = CreateActivityElement(doc, ns, className, export);
        //     HiveManifestMod.SetOrReplaceXmlElement(xmlNode, element);
        // }

        private static XmlElement CreateUnityOverlayElement(XmlDocument doc, string ns, string activityName)
        {
            // <activity android:name="activityName" android:configChanges="all|of|them" android:theme="@android:style/Theme.Translucent.NoTitleBar.Fullscreen">
            // </activity>
            XmlElement activityElement = HiveManifestMod.CreateActivityElement(doc, ns, activityName);
            activityElement.SetAttribute("configChanges", ns, "fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen|layoutDirection");
            activityElement.SetAttribute("theme", ns, "@style/AppTheme");
            return activityElement;
        }

        private static XmlElement CreateContentProviderElement(XmlDocument doc, string ns, string appId)
        {
            XmlElement provierElement = doc.CreateElement("provider");
            provierElement.SetAttribute("name", ns, FacebookContentProviderName);
            string authorities = string.Format(CultureInfo.InvariantCulture, FacebookContentProviderAuthFormat, appId);
            provierElement.SetAttribute("authorities", ns, authorities);
            provierElement.SetAttribute("exported", ns, "true");
            return provierElement;
        }

        private static XmlElement CreateActivityElement(XmlDocument doc, string ns, string activityName, bool exported = false)
        {
            // <activity android:name="activityName" android:exported="true">
            // </activity>
            XmlElement activityElement = doc.CreateElement("activity");
            activityElement.SetAttribute("name", ns, activityName);
            if (exported)
            {
                activityElement.SetAttribute("exported", ns, "true");
            }

            return activityElement;
        }

        private static void AddAppLinkingActivity(XmlDocument doc, XmlNode xmlNodeActivity, string ns, List<string> schemes)
        {
            // XmlElement element = HiveManifestMod.CreateActivityElement(doc, ns, AppLinkActivityName, true);
            // remove All action=VIEW

            XmlNode intent = xmlNodeActivity.FirstChild;
            XmlNode actionintent = null;
            while (intent != null)
            {
                if (intent != null && intent.Name == "intent-filter" )
                {
                    foreach( XmlNode action in intent.ChildNodes )
                    {
                        if( action.Name.Equals("action") && action.Attributes.GetNamedItem("name",ns).Value.Equals("android.intent.action.VIEW"))
                        {
                            actionintent = intent;
                        }
                    }
                    
                }
                intent = intent.NextSibling;
                if( actionintent != null )
                {
                    xmlNodeActivity.RemoveChild(actionintent);
                    actionintent = null;
                }
            }


            foreach (var scheme in schemes)
            {
                // We have to create an intent filter for each scheme since an intent filter
                // can have only one data element.
                XmlElement intentFilter = doc.CreateElement("intent-filter");

                var action = doc.CreateElement("action");
                action.SetAttribute("name", ns, "android.intent.action.VIEW");
                intentFilter.AppendChild(action);

                var category = doc.CreateElement("category");
                category.SetAttribute("name", ns, "android.intent.category.DEFAULT");
                intentFilter.AppendChild(category);

                var category2 = doc.CreateElement("category");
                category2.SetAttribute("name", ns, "android.intent.category.BROWSABLE");
                intentFilter.AppendChild(category2);

                XmlElement dataElement = doc.CreateElement("data");
                dataElement.SetAttribute("scheme", ns, scheme);
                intentFilter.AppendChild(dataElement);

                

                xmlNodeActivity.AppendChild(intentFilter);
            }

            // HiveManifestMod.SetOrReplaceXmlElement(xmlNode, element);
        }

        private static void CreateDefaultAndroidManifest(string outputFile)
        {
            // TODO : BuildPipeline.GetPlaybackEngineDirectory()/Apk/UnityManifest.xml
            var inputFile = Path.Combine(
                EditorApplication.applicationContentsPath,
                "PlaybackEngines/androidplayer/AndroidManifest.xml");
            if (!File.Exists(inputFile))
            {
                // Unity moved this file. Try to get it at its new location
                inputFile = Path.Combine(
                    EditorApplication.applicationContentsPath,
                    "PlaybackEngines/AndroidPlayer/Apk/AndroidManifest.xml");

                if (!File.Exists(inputFile))
                {
                    // On Unity 5.3+ we don't have default manifest so use our own
                    // manifest and warn the user that they may need to modify it manually
                    inputFile = Path.Combine(Application.dataPath, HiveDefaultAndroidManifestPath);
                    Debug.LogWarning(
                        string.Format(
                            "No existing android manifest found at '{0}'. Creating a default manifest file",
                            outputFile));
                }
            }

            File.Copy(inputFile, outputFile);
        }

		private static string SupportedOrientation(){

			int orient = 0;  // portrait, right, upside-down, left

			if (PlayerSettings.allowedAutorotateToPortrait) {
				orient = orient + 1000;
			}
			if (PlayerSettings.allowedAutorotateToLandscapeRight) {
				orient = orient + 100;
			}
			if (PlayerSettings.allowedAutorotateToPortraitUpsideDown) {
				orient = orient + 10;
			}
			if (PlayerSettings.allowedAutorotateToLandscapeLeft) {
				orient = orient + 1;
			}

			if (orient == 0) {
				return "sensor";
			} else if (orient == 1000) {
				return "portrait";
			} else if (orient == 100) {
				return "landscape";
			} else if (orient == 10) {
				return "reversePortrait";
			} else if (orient == 1) {
				return "reverseLandscape";
			} else if (orient == 1010) {
				return "sensorPortrait";
			} else if (orient == 101) {
				return "sensorLandscape";
			} else {
				return "sensor";
			}

		}
    }
}
