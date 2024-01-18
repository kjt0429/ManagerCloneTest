
/**
 * @file    HiveDependencies.cs
 * 
 * @author  nanomech
 * @date    2016-2022
 * @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 * @defgroup HiveDependencies
 * @{
 * @brief HIVE dependency process on loadtime support. <br/><br/>
 */


using System;
using System.Collections.Generic;
using UnityEditor;
// using Google.JarResolver;
using System.IO;

[InitializeOnLoad]
public class HiveDependencies : AssetPostprocessor{
#if UNITY_ANDROID
  /// <summary>Instance of the PlayServicesSupport resolver</summary>
  public static object svcSupport;
#endif  // UNITY_ANDROID

/// Initializes static members of the class.
  static HiveDependencies() {

        //
        //
        // NOTE:
        //
        //       UNCOMMENT THIS CALL TO MAKE THE DEPENDENCIES BE REGISTERED.
        //   THIS FILE IS ONLY A SAMPLE!!
        //
        // RegisterDependencies();
        //

        if (!Directory.Exists("Assets/Plugins/Android"))
        {
            Directory.CreateDirectory("Assets/Plugins/Android");
            AssetDatabase.Refresh();
        }

#if UNITY_ANDROID
        // string sdkPath = "Assets/Hive_SDK_v4/Plugins/Android/libs/";
        // string pluginPath = "Assets/Plugins/Android/";
        // string SDKFile = "HIVE_SDK.aar";
        // string SDKPluginFile = "HIVE_SDK_UnityPlugin.aar";
        // if( File.Exists(sdkPath+SDKFile) || File.Exists(sdkPath+SDKPluginFile) )
        // {
        //   UnityEngine.Debug.Log("Reimport Assets/Hive_SDK_v4/Plugins");

        //   if( File.Exists(sdkPath+SDKFile) )
        //   {
        //       FileUtil.MoveFileOrDirectory(sdkPath+SDKFile,pluginPath+SDKFile);
        //   }
        //   if( File.Exists(sdkPath+SDKPluginFile) )
        //   {
        //       FileUtil.MoveFileOrDirectory(sdkPath+SDKPluginFile,pluginPath+SDKPluginFile);
        //   }
        //   AssetDatabase.Refresh();
        // }

        //   UnityEngine.Debug.Log("Refresh Assets/Plugins");
        // // Hive.Unity.Editor.HiveAARExpolder.ProcessAars("Assets/Plugins/Android"); 
        // AssetDatabase.Refresh();
#endif
        
  }

    /// <summary>
  /// Registers the dependencies needed by this plugin.
  /// </summary>
  public static void RegisterDependencies() {
#if UNITY_ANDROID
    // RegisterAndroidDependencies();
    // 'HIVE SDK 4.13.0' Using HIVESDKDependencies.Xml  
#elif UNITY_IOS
    RegisterIOSDependencies();
#endif
  }

  /// <summary>
  /// Registers the android dependencies.
  /// </summary>
  public static void RegisterAndroidDependencies() {
#if UNITY_ANDROID
    // Using HIVESDKDependencies.Xml
#endif
  }

  /// <summary>
  /// Registers the IOS dependencies.
  /// </summary>
  public static void RegisterIOSDependencies() {

    // Setup the resolver using reflection as the module may not be
    // available at compile time.
    Type iosResolver = Google.VersionHandler.FindClass(
        "Google.IOSResolver", "Google.IOSResolver");
    if (iosResolver == null) {
      return;
    }

    // Dependencies for iOS are added by referring to CocoaPods.  The libraries and frameworkds are
    //  and added to the Unity project, so they will automatically be included.
    //
    // This example add the GooglePlayGames pod, version 5.0 or greater, disabling bitcode generation.

    // Google.VersionHandler.InvokeStaticMethod(
    //   iosResolver, "AddPod",
    //   new object[] { "GooglePlayGames" },
    //   namedArgs: new Dictionary<string, object>() {
    //       { "version", "5.0+" },
    //       { "bitcodeEnabled", false },
    //   });
  }

  // Handle delayed loading of the dependency resolvers.
  private static void OnPostprocessAllAssets(
      string[] importedAssets, string[] deletedAssets,
      string[] movedAssets, string[] movedFromPath) {

        // if **/HIVE*SDK-4.*.aar imported -> move to Asset/Plugin/Android
        // auto explored AAR and rename $(applicationId)

        // foreach (string str in importedAssets)
        // {
        //   if( str.StartsWith("Assets/Plugins/Android" ))
        //   {
        //     if( str.Contains("hive_config.xml"))
        //     {
        //     }
        //   }
        //   else
        //   {
        //     if( str.Contains("HIVE") && str.Contains("SDK") && str.EndsWith(".aar") )
        //     {
        //       if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        //       {
                
        //         string path = System.IO.Path.GetDirectoryName(str);
        //         string filename = System.IO.Path.GetFileName(str);

        //         string from = System.IO.Path.Combine(path,filename);
        //         string to = System.IO.Path.Combine("Assets/Plugins/Android",filename);

        //         if (!Directory.Exists("Assets/Plugins/Android"))
        //         {
        //             Directory.CreateDirectory("Assets/Plugins/Android");
        //         }

        //         FileUtil.MoveFileOrDirectory(from,to);
        //         UnityEngine.Debug.Log("Moved HIVE SDK : " + str + " to Assets/Plugins/Android/");

        //         // Hive.Unity.Editor.HiveAARExpolder.ProcessAars("Assets/Plugins/Android");  
        //       }
        //     }
        //   }
        // }

        

    // foreach (string asset in importedAssets) {
    //   if (asset.Contains("IOSResolver") ||
    //     asset.Contains("JarResolver")) {
    //     RegisterDependencies();
    //     break;
    //   }
    // }

    var hiveBundleFile = "Assets/Hive_SDK_v4/Plugins/iOS/resource/HIVE_SDK_resource.bundle";
		if(Directory.Exists(hiveBundleFile)){
			PluginImporter hiveBundle = (PluginImporter)PluginImporter.GetAtPath(hiveBundleFile);
			hiveBundle.SetCompatibleWithAnyPlatform(false);
			hiveBundle.SetCompatibleWithEditor(false);
			hiveBundle.SetCompatibleWithPlatform(BuildTarget.iOS,true);
		}
  }
}
