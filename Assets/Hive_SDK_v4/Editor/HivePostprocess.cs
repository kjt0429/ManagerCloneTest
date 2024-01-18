/**
 * @file    HivePostprocess.cs
 * 
 * @author  nanomech
 * @date    2016-2022
 * @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 * @defgroup UnityEditor.HiveEditor
 * @{
 * @brief PostPrcessing on BuildTime <br/><br/>
 */

namespace UnityEditor.HiveEditor
{
    using System.IO;
    using System.Collections.Generic;
    using Hive.Unity;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEngine;

#if UNITY_IOS
    using UnityEditor.iOS;
    using UnityEditor.iOS.Xcode;
#endif
    using System.Diagnostics;
    using Debug = UnityEngine.Debug;
    using System.Linq;

    public static class HivePostProcess
    {
        [PostProcessBuild(100)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            // // Unity renamed build target from iPhone to iOS in Unity 5, this keeps both versions happy
            // if (target.ToString() == "iOS" || target.ToString() == "iPhone")
            // {
            //     UpdatePlist(path);
            //     FixupFiles.FixSimulator(path);
            //     FixupFiles.AddVersionDefine(path);
            //     FixupFiles.FixColdStart(path);
            // }

            Debug.Log("OnPostProcessBuild -\n target: "+target+"\npath: "+path);

            if (target == BuildTarget.Android)
			{
				// If integrating with facebook on any platform, throw a warning if the app id is invalid
				if (!Hive.Unity.Editor.HiveConfigXML.Android.IsValidAppId)
				{
					Debug.LogWarning("You didn't specify a Hive app ID.  Please add one using the Hive menu in the main Unity editor.");
				}
                // The default Bundle Identifier for Unity does magical things that causes bad stuff to happen
                if (PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android) == "com.Company.ProductName")
                {
                    Debug.LogError("The default Unity Bundle Identifier (com.Company.ProductName) will not work correctly.");
                }

                // if (!HiveAndroidUtil.SetupProperly)
                // {
                //     Debug.LogError("Your Android setup is not correct. See Settings in Facebook menu.");
                // }

                if (!HiveManifestMod.CheckManifest())
                {
                    // If something is wrong with the Android Manifest, try to regenerate it to fix it for the next build.
                    HiveManifestMod.GenerateManifest();
                }


            }
			else if(target == BuildTarget.iOS) {				
				// If integrating with facebook on any platform, throw a warning if the app id is invalid
				if (!Hive.Unity.Editor.HiveConfigXML.iOS.IsValidAppId)
				{
					Debug.LogWarning("You didn't specify a Hive app ID.  Please add one using the Hive menu in the main Unity editor.");
				}
                iOSPostBuild(path);    
            }
            else if (target == BuildTarget.StandaloneWindows64) {
                // Do Edit File's Version.
                string editToolPath = "Assets/Hive_SDK_v4/Plugins/Windows/postBuildUtil";
                if (!Directory.Exists(editToolPath)) {
                    Debug.LogWarning(editToolPath + "Not Exist! ");
                } else {
                    // https://github.com/electron/rcedit
                    string toolExecutePath = Path.Combine(editToolPath, "rcedit.exe");
                    if (!File.Exists(toolExecutePath)) {
                        Debug.LogWarning("rcedit.exe Not Exist! : " + toolExecutePath);
                    } else {
                        Debug.Log("File, Product Version Change To :" + Application.version);
                        Process p = new System.Diagnostics.Process();   //  Process is included in System.Diagnostics
                        p.StartInfo.FileName = toolExecutePath;
                        p.StartInfo.Arguments = " \"" + path + "\" --set-product-version " + Application.version + " --set-file-version " + Application.version;
                        p.Start();
                    }
                }
                string resultPath = Path.GetDirectoryName(path);
                string windowsHiveStringPath = Path.GetFullPath("Assets/Hive_SDK_v4/Plugins/desktop/hive_string");
                directoryCopy(windowsHiveStringPath, $"{resultPath}\\resources\\hive_string", true);
                string additionalFilesDir = Path.GetFullPath("Assets/Hive_SDK_v4/Plugins/Windows/additional");
                string hivePluginsDir = Path.GetFullPath("Assets/Hive_SDK_v4/Plugins/Windows/hivePlugins");


                // Plugin DLL COPY!
                string windowsHivePluginDll = Path.GetFullPath($"{resultPath}/{Application.productName}_Data/Plugins/x86_64/HIVE_PLUGIN.dll");                
                if (File.Exists(windowsHivePluginDll)) {
                    File.Copy(windowsHivePluginDll, $"{resultPath}\\HIVE_PLUGIN.dll", true);
                } else {
                    Debug.LogWarning("PostProcessor not found windowsHivePluginDll File : " + windowsHivePluginDll);
                }
                // Copy Additional 
                directoryCopy(additionalFilesDir, resultPath, true);
                // Copy hivePlugins (this is not HIVE_PLUGIN.dll)
                directoryCopyToFlatDirectory(hivePluginsDir, resultPath);

                string windowsHiveConfig = Path.GetFullPath("Assets/Plugins/Windows/res/hive_config.xml");
                if (File.Exists(windowsHiveConfig)) {
                    File.Copy(windowsHiveConfig, $"{resultPath}\\resources\\hive_config.xml", true);
                } else {
                    Debug.LogWarning("PostProcessor not found hive_config File : " + windowsHiveConfig);
                }

                EditorUtility.ClearProgressBar();
            }
            else if (target == BuildTarget.StandaloneOSX) {
                string macOSHiveStringPath = Path.GetFullPath("Assets/Hive_SDK_v4/Plugins/desktop/hive_string");
                directoryCopy(macOSHiveStringPath, $"{path}/Contents/Resources/hive_string", true);
                string macOSHiveConfig = Path.GetFullPath("Assets/Plugins/macOS/res/hive_config.xml");
                if (File.Exists(macOSHiveConfig)) {
                    File.Copy(macOSHiveConfig, $"{path}/Contents/Resources/hive_config.xml", true);
                } else {
                    Debug.LogWarning("PostProcessor not found hive_config File : " + macOSHiveConfig);
                }
                
            }
        }

        private static string getFacebookAppID() {
            //TODO: 페이스북 앱아이디 얻는 작업필요함.
			#if UNITY_IOS
			return Hive.Unity.Editor.HiveConfigXML.iOS.facebookAppID;
			#elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.facebookAppID;
			#else
			return null;
			#endif
        }

        private static bool hasFacebookAppId() {
			#if UNITY_IOS
			return Hive.Unity.Editor.HiveConfigXML.iOS.IsValidFacebookAppId;
			#elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.IsValidFacebookAppId;
			#else
			return false;
			#endif
        }

        private static string getFacebookClientToken()
        {
            //TODO: 페이스북 앱아이디 얻는 작업필요함.
            #if UNITY_IOS
            return Hive.Unity.Editor.HiveConfigXML.iOS.facebookClientToken;
            #elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.facebookClientToken;
            #else
			return null;
            #endif
        }

        private static bool hasFacebookClientToken()
        {
            #if UNITY_IOS
            return Hive.Unity.Editor.HiveConfigXML.iOS.IsValidFacebookClientToken;
            #elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.IsValidFacebookClientToken;
            #else
			return false;
            #endif
        }

        private static string getBundleIdentifier(){
			#if UNITY_IOS
			return Hive.Unity.Editor.HiveConfigXML.iOS.HIVEAppID;
			#elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.HIVEAppID;
            #else
            return null;
            #endif
            
        }

		private static string getGooglePlayAppId() {
			#if UNITY_IOS
			return Hive.Unity.Editor.HiveConfigXML.iOS.googlePlayAppID;
			#elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.googlePlayAppID;
			#else
			return null;
			#endif
		}

		private static bool hasGooglePlayAppId() {
			#if UNITY_IOS
			return Hive.Unity.Editor.HiveConfigXML.iOS.IsValidGooglePlayAppId;
			#elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.IsValidGooglePlayAppId;
			#else
			return false;
			#endif
		}

		private static string getGoogleServerClientId(){
			#if UNITY_IOS
			return Hive.Unity.Editor.HiveConfigXML.iOS.googleServerClientID;
			#elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.googleServerClientID;
			#else
			return null;
			#endif
		}
			
		private static bool hasGoogleServerClientId() {
			#if UNITY_IOS
			return Hive.Unity.Editor.HiveConfigXML.iOS.IsValidGoogleClientId;
			#elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.IsValidGoogleClientId;
			#else
			return false;
			#endif
		}

		private static string getGoogleReversedClientId() {
			#if UNITY_IOS
			return Hive.Unity.Editor.HiveConfigXML.iOS.googleReversedClientID;
			#elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.googleReversedClientID;
			#else
			return null;
			#endif
		}

		private static bool hasGoogleReversedClientId() {
			#if UNITY_IOS
			return Hive.Unity.Editor.HiveConfigXML.iOS.IsValidGoogleReverseClientId;
			#elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.IsValidGoogleReverseClientId;
			#else
			return false;
			#endif
		}

		private static string getQQAppId(){
			#if UNITY_IOS
			return Hive.Unity.Editor.HiveConfigXML.iOS.qqAppId;
			#elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.qqAppId;
			#else
			return null;
			#endif
		}

		private static bool hasQQAppId() {
			#if UNITY_IOS
			return Hive.Unity.Editor.HiveConfigXML.iOS.IsValidQQAppId;
			#elif UNITY_ANDROID
			return Hive.Unity.Editor.HiveConfigXML.Android.IsValidQQAppId;
			#else
			return false;
			#endif
		}

        private static string getVKAppId() {
            #if UNITY_IOS
            return Hive.Unity.Editor.HiveConfigXML.iOS.vkAppId;
            #elif UNITY_ANDROID
            return Hive.Unity.Editor.HiveConfigXML.Android.vkAppId;
            #else
            return null;
            #endif
        }

        private static bool hasVKAppId() {
            #if UNITY_IOS
            return Hive.Unity.Editor.HiveConfigXML.iOS.IsValidVKAppId;
            #elif UNITY_ANDROID
            return Hive.Unity.Editor.HiveConfigXML.Android.IsValidVKAppId;
            #else
            return false;
            #endif
        }

        private static string getWeChatAppId() {
            #if UNITY_IOS
            return Hive.Unity.Editor.HiveConfigXML.iOS.weChatAppId;
            #elif UNITY_ANDROID
            return Hive.Unity.Editor.HiveConfigXML.Android.weChatAppId;
            #else
            return null;
            #endif
        }

        private static bool hasWeChatAppId() {
            #if UNITY_IOS
            return Hive.Unity.Editor.HiveConfigXML.iOS.IsValidWeChatAppId;
            #elif UNITY_ANDROID
            return Hive.Unity.Editor.HiveConfigXML.Android.IsValidWeChatAppId;
            #else
            return false;
            #endif
        }

        private static bool hasLineChannelId() {
            #if UNITY_IOS
            return Hive.Unity.Editor.HiveConfigXML.iOS.IsValidLineChannelId;
            #elif UNITY_ANDROID
            return Hive.Unity.Editor.HiveConfigXML.Android.IsValidLineChannelId;
            #else
            return false;
            #endif
        }

        //xcode project 후반작업
        private static void iOSPostBuild(string buildPath){
            #if UNITY_IOS
            
            iOSSettingProject(buildPath);
            iOSSettingInfoPlist(buildPath);
            iOSSettingPodFile(buildPath);
            iOSSettingCapabilites(buildPath);

            #endif
        }


        private static void iOSSettingProject(string buildPath) {
            #if UNITY_IOS

            string framework_path = "$(SRCROOT)/Frameworks/Hive_SDK_v4/Plugins/iOS/framework";
            //copy resource
            string[] hive_res_path = {
                "Plugins/iOS/hive_config.xml"
            };        
            
            //Default Setting System Framework
            string[] system_frameworks = {
                "libz.tbd",
                "libsqlite3.tbd",
                "AdSupport.framework",
                "CFNetwork.framework",
                "CoreData.framework",
                "CoreTelephony.framework",
                "Security.framework",
                "StoreKit.framework",
                "SystemConfiguration.framework",
                "UIKit.framework",
                "iAd.framework",
                "MobileCoreServices.framework",
				"WebKit.framework",
                "MapKit.framework",
                "JavaScriptCore.framework", // Goolge recaptcha Enterprise를 위해 사용.
                "Accelerate.framework"  // facebook framework 6.0 버전 이상부터 필요. 이미지 및 영상 처리에 대한 프레임워크
            };

            string[] optional_frameworks = {
                "SafariServices.framework",
                "CoreSpotlight.framework"
            };

            var path = Path.Combine(buildPath,"Unity-iPhone.xcodeproj/project.pbxproj");
            var project = new PBXProject();
            project.ReadFromFile(path);

            // 프로젝트의 타겟들 
            string mainTarget = "main";
            string unityFrameworkTarget = "unityframework";
                        
#if UNITY_2019_3_OR_NEWER
            var targets = new Dictionary<string, string>(){
                                                    {mainTarget, project.GetUnityMainTargetGuid()}, // 메인 타겟
                                                    {unityFrameworkTarget, project.GetUnityFrameworkTargetGuid()} // 유니티 프레임워크 타겟
                                                    }; 
#else
            var targetName = PBXProject.GetUnityTargetName();
            var targets = new Dictionary<string, string>(){
                                                    {mainTarget, project.TargetGuidByName(targetName)} // 메인 타겟
                                                     };  
#endif

            //add system framework
            for(int i=0;i<system_frameworks.Length;++i){
#if UNITY_2019_3_OR_NEWER
                project.AddFrameworkToProject(targets[unityFrameworkTarget],system_frameworks[i],false);
#else 
                project.AddFrameworkToProject(targets[mainTarget],system_frameworks[i],false);
#endif
            }

            for(int i=0;i<optional_frameworks.Length;++i){
#if UNITY_2019_3_OR_NEWER
                project.AddFrameworkToProject(targets[unityFrameworkTarget],optional_frameworks[i],true);
#else
                project.AddFrameworkToProject(targets[mainTarget],optional_frameworks[i],true);
#endif
            }

            //make resource directory
            string project_res_directory = "Hive_SDK_v4/";
            project_res_directory = Path.Combine(buildPath,project_res_directory);
            if(!Directory.Exists(project_res_directory)){
                Directory.CreateDirectory(project_res_directory);
            }
            
            //add resource
            for(int i=0;i<hive_res_path.Length;++i) {
                string res = hive_res_path[i];
                string project_res_path = Path.Combine("Hive_SDK_v4/",Path.GetFileName(res)); 
                
                string assetsPath = "Assets/"+res;
                string buildPathCombine = Path.Combine(buildPath,project_res_path);

                if(!Directory.Exists(Path.GetDirectoryName(buildPathCombine))){
                    Directory.CreateDirectory(Path.GetDirectoryName(buildPathCombine));
                }

                var attr = File.GetAttributes(assetsPath);
                if((attr & FileAttributes.Directory) == FileAttributes.Directory){
                    directoryCopy(assetsPath,buildPathCombine,true);
                }else {
                    //파일은 무조건 복사해서 덮어쓴다.
                    if( !assetsPath.EndsWith(".meta") )
                        File.Copy(assetsPath,buildPathCombine, true);
                }

                //프로젝트 추가여부는 프로젝트에 추가되어있는지 확인후 결정
                if(!project.ContainsFileByProjectPath(project_res_path)){
                    project.AddFileToBuild(
                        targets[mainTarget],
                        project.AddFile(project_res_path, project_res_path, PBXSourceTree.Source));
                }
            }

            //linker flag setting
            foreach (string target in targets.Values) {
                project.AddBuildProperty(target, "OTHER_CFLAGS", "-Wextern-initializer -Wunguarded-availability-new -Wmissing-declarations");
                project.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC -lz -fobjc-arc");
                //framework search path
                project.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", framework_path);

                project.SetBuildProperty(target, "ENABLE_BITCODE","NO");

                project.SetBuildProperty(target, "SWIFT_VERSION", "5");

                project.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
                project.SetBuildProperty(target, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");
            }

            // main target에만 해당 옵션 추가
            project.SetBuildProperty(targets[mainTarget], "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

            // Firebase GoogleService-Info.plist
            iOSAddGoogleServiceInfoPlist(buildPath, project, targets[mainTarget]);

            //SAVE PROJECT
            project.WriteToFile(path);

            #endif
        }

        private static void iOSSettingInfoPlist(string buildPath) {
            #if UNITY_IOS

            var PlistPath = buildPath + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(PlistPath);

            var rootDict = plist.root;
            rootDict.SetBoolean("UIViewControllerBasedStatusBarAppearance",false);
            if( hasFacebookAppId() )
                rootDict.SetString("FacebookAppID",getFacebookAppID());
            if (hasFacebookClientToken())
                rootDict.SetString("FacebookClientToken", getFacebookClientToken());

            rootDict.SetString("CFBundleIdentifier",getBundleIdentifier());



            //facebook white list
            var LSApplicationQueriesSchemes = rootDict.CreateArray("LSApplicationQueriesSchemes");
            LSApplicationQueriesSchemes.AddString("fbapi");
            LSApplicationQueriesSchemes.AddString("fb-messenger-share-api");

            // for vk
            LSApplicationQueriesSchemes.AddString("vk");
            LSApplicationQueriesSchemes.AddString("vkauthorize");
            LSApplicationQueriesSchemes.AddString("vk-share");

            // for WeChat
            LSApplicationQueriesSchemes.AddString("weixin");
            LSApplicationQueriesSchemes.AddString("weixinULAPI");
            

            // for QQ
            LSApplicationQueriesSchemes.AddString("mqqOpensdkSSoLogin");
            LSApplicationQueriesSchemes.AddString("mqqopensdkapiV2");
            LSApplicationQueriesSchemes.AddString("mqqopensdkapiV3");
            LSApplicationQueriesSchemes.AddString("wtloginmqq2");
            LSApplicationQueriesSchemes.AddString("mqq");
            LSApplicationQueriesSchemes.AddString("mqqapi");
            LSApplicationQueriesSchemes.AddString("mqqopensdknopasteboard");
            LSApplicationQueriesSchemes.AddString("mqqopensdknopasteboardios16");


            LSApplicationQueriesSchemes.AddString("lineauth2");

            //URL Types settings
            var CFBundleURLTypes = rootDict.CreateArray("CFBundleURLTypes");

            var facebookURLType = CFBundleURLTypes.AddDict();
            facebookURLType.SetString("CFBundleTypeRole","Editor");
            if( hasFacebookAppId() )
                facebookURLType.CreateArray("CFBundleURLSchemes").AddString("fb"+getFacebookAppID());

			// add Google reversed client id
			if (hasGoogleReversedClientId()){
				var googleReversedClientId = CFBundleURLTypes.AddDict();
				googleReversedClientId.SetString("CFBundleTypeRole","Editor");
				googleReversedClientId.CreateArray("CFBundleURLSchemes").AddString(getGoogleReversedClientId());
			}
			// add tencent appid
			if (hasQQAppId()){
				var qqAppId = CFBundleURLTypes.AddDict();
				qqAppId.SetString("CFBundleTypeRole","Editor");
				qqAppId.CreateArray("CFBundleURLSchemes").AddString("tencent"+getQQAppId());
			}


            // add VK appid
            if (hasVKAppId()) {
                var vkAppId = CFBundleURLTypes.AddDict();
                vkAppId.SetString("CFBundleTypeRole","Editor");
                vkAppId.CreateArray("CFBundleURLSchemes").AddString("vk"+getVKAppId());
            }

            // add wechat appid
            if (hasWeChatAppId()) {
                var weChatAppId = CFBundleURLTypes.AddDict();
                weChatAppId.SetString("CFBundleTypeRole","Editor");
                weChatAppId.CreateArray("CFBundleURLSchemes").AddString(getWeChatAppId());
            }

            // add line channel id
            if (hasLineChannelId()) {
                var lineChannelId = CFBundleURLTypes.AddDict();
                lineChannelId.SetString("CFBundleTypeRole","Editor");
                lineChannelId.CreateArray("CFBundleURLSchemes").AddString("line3rdp."+getBundleIdentifier());
            }

            var urlSchemes = CFBundleURLTypes.AddDict();
            urlSchemes.SetString("CFBundleTypeRole","Editor");
            urlSchemes.SetString("CFBundleIdentifier",getBundleIdentifier());
            urlSchemes.CreateArray("CFBundleURLSchemes").AddString(getBundleIdentifier());

            // remove exit on suspend if it exists.
            string exitsOnSuspendKey = "UIApplicationExitsOnSuspend";
            if(rootDict.values.ContainsKey(exitsOnSuspendKey))
            {
                rootDict.values.Remove(exitsOnSuspendKey);
            }

            // Set encryption usage boolean
            string encryptKey = "ITSAppUsesNonExemptEncryption";
            rootDict.SetBoolean(encryptKey, false);

            // ATS
            var ATSDict = rootDict.CreateDict("NSAppTransportSecurity");
            ATSDict.SetBoolean("NSAllowsArbitraryLoads",true);

            plist.WriteToFile(PlistPath);

            #endif
        }

        private static void iOSSettingPodFile(string buildPath) {
            #if UNITY_IOS
            using (StreamWriter sw = File.AppendText(buildPath + "/Podfile"))
            {
                //LineSDK 미인식 이슈 해결
                sw.WriteLine("\npost_install do |installer|");
                sw.WriteLine("  installer.pods_project.targets.each do |target|");
                sw.WriteLine("    target.build_configurations.each do |config|");
                sw.WriteLine("      if ['LineSDKSwift'].include? target.name");
                sw.WriteLine("        config.build_settings['BUILD_LIBRARY_FOR_DISTRIBUTION'] = 'YES'");
                sw.WriteLine("      elsif config.build_settings['WRAPPER_EXTENSION'] == 'bundle'");
                sw.WriteLine("        config.build_settings['CODE_SIGNING_ALLOWED'] = 'NO'"); // #GCPSDK4-882 Xcode 14.x 이슈 대응
                sw.WriteLine("      end");
                sw.WriteLine("      if config.build_settings['IPHONEOS_DEPLOYMENT_TARGET'].to_f < 12.0");
                sw.WriteLine("        config.build_settings['IPHONEOS_DEPLOYMENT_TARGET'] = '12.0'");
                sw.WriteLine("      end");
                sw.WriteLine("    end");
                sw.WriteLine("  end");
                sw.WriteLine("end");
            }
            #endif
        }

        #if UNITY_IOS
        public static void iOSAddGoogleServiceInfoPlist(string buildPath, PBXProject project, string target) {
            

            string[] filesToCopy = new string[]
            {
                "GoogleService-Info.plist",
            };

            foreach (string file in filesToCopy) {
                var srcPath = Path.Combine("Assets/ExternalResources/iOS", file);
                var relativePath = Path.Combine("", file);
                var physicalPath = Path.Combine(buildPath, relativePath);

                UnityEngine.Debug.Log("srcPath = " + srcPath);
                UnityEngine.Debug.Log("relativePath = " + relativePath);
                UnityEngine.Debug.Log("physicalPath = " + physicalPath);

                File.Copy(srcPath, physicalPath, true);
                var fileGuid = project.AddFile(physicalPath, relativePath);
                Debug.Log("fileGuid = " + fileGuid);

                project.AddFileToBuild(target, fileGuid);
            }
        }  
        #endif

        /// <summary>
        /// 하위 폴더의 파일을 모두 특정 디렉토리로 복사합니다.
        /// 폴더 구조는 복사되지 않으며 모든 하위 파일이 수평으로 배치됩니다.
        /// </summary>
        /// <param name="sourceDirName">원본 폴더</param>
        /// <param name="destDirName">복사할 폴더</param>
        private static void directoryCopyToFlatDirectory(string sourceDirName, string destDirName) {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists) {
                Debug.Log("Source directory does not exist or could not be found: "
                    + sourceDirName);
                return;
            }

            if (!Directory.Exists(destDirName)) {
                Directory.CreateDirectory(destDirName);
            }
            FileInfo[] files = dir.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            copyFiles(files, destDirName);

            DirectoryInfo[] dirs = dir.GetDirectories("*", SearchOption.TopDirectoryOnly);
            foreach(DirectoryInfo subdir in dirs) {
                directoryCopyToFlatDirectory(subdir.FullName, destDirName);
            }
        }

        private static void directoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        { 
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

#if UNITY_STANDALONE
            DirectoryInfo[] dirs = dir.GetDirectories("*", SearchOption.TopDirectoryOnly);    //  for recursively
#else
            DirectoryInfo[] dirs = dir.GetDirectories();
#endif
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
#if UNITY_STANDALONE
            FileInfo[] files = dir.GetFiles("*.*",SearchOption.TopDirectoryOnly);
#else
            FileInfo[] files = dir.GetFiles();
#endif
            copyFiles(files, destDirName);

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    directoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private static void copyFiles(FileInfo[] files, string destDirName) {
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
#if UNITY_STANDALONE
                if (!temppath.EndsWith(".meta"))
                    file.CopyTo(temppath, true);    //  copy Overwrite option.
#else
                    file.CopyTo(temppath, false);
#endif
            }
        }

        private static void iOSSettingCapabilites(string path) {
            #if UNITY_IOS
            Debug.Log("iOSSettingCapabilites : OnPostProcessBuild -\n path: "+path);
            
            string projPath = PBXProject.GetPBXProjectPath(path);
            string mainTargetName = "Unity-iPhone";

            ProjectCapabilityManager capabilityManager = new ProjectCapabilityManager(projPath, "Entitlements.entitlements", mainTargetName);

            capabilityManager.AddInAppPurchase();
            capabilityManager.AddPushNotifications(true);//development
            capabilityManager.AddBackgroundModes(iOS.Xcode.BackgroundModesOptions.RemoteNotifications);

            /*
            // 필요시 SignIn with Apple 추가
            capabilityManager.AddSignInWithApple();
            // 필요시 게임센터 로그인 추가
            capabilityManager.AddGameCenter();
            // 필요시 유니버셜링크 도메인 추가
            capabilityManager.AddAssociatedDomains(new string[]{"applinks:hiveota.withhive.com", "applinks:promotion.qpyou.cn", "applinks:test-promotion.qpyou.cn", "applinks:sandbox-promotion.qpyou.cn"});
            */

            capabilityManager.WriteToFile();
            #endif
        }
    }
}
