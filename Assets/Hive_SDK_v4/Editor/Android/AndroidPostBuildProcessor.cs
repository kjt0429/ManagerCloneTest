

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Android;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Globalization;


#if UNITY_2018_1_OR_NEWER
public class AndroidPostBuildProcessor : IPostGenerateGradleAndroidProject
{
    private string Android_Huawei_Agconnect_Version = "1.7.3.300";
    private string Android_Huawei_Gradle_Tool_Version = "7.1.3";
    
    public int callbackOrder
    {
        get
        {
            return 999;
        }
    }

    void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path)
    {
        Debug.Log("Build path : " + path);

        #if UNITY_2022_2_OR_NEWER
        // 2022.2.1f1 이상에서 androidX 라이브러리를 못찾는 이슈가 있어 프로젝트 레벨의 gradle.properties 에 android.useAndroidX=true를 쓸수 있도록 경로 수정
        string gradlePropertiesFile = path + "/../gradle.properties";
        #else
        string gradlePropertiesFile = path + "/gradle.properties";
        #endif

        if (File.Exists(gradlePropertiesFile))
        {
            File.Delete(gradlePropertiesFile);
        }
        StreamWriter writer = File.CreateText(gradlePropertiesFile);
        writer.WriteLine("org.gradle.jvmargs=-Xmx4096M");
        writer.WriteLine("android.useAndroidX=true");
        writer.WriteLine("android.enableJetifier=true");
        #if UNITY_2022_2_OR_NEWER        
        writer.WriteLine("unityStreamingAssets=.unity3d, google-services-desktop.json, google-services.json, GoogleService-Info.plist");
        #endif        
        writer.Flush();
        writer.Close();

        #if UNITY_2019_3_OR_NEWER
        resolveBaseBuildGradle(path); // for huawei
        resolveLauncherBuildGradle(path);
        #endif
        #if UNITY_2020_1_OR_NEWER
        resolveMainBuildGradle(path);
        #endif

        // resolve targetSDKversion 31, JDK 11 issue
        // https://developers.google.com/ar/develop/unity-arf/android-12-build
        resolveMainBuildGradleTargetSDK31(path);
        resolveUnityResourcesBuildGradleTargetSDK31(path);
        copyToGoogleServicesJson(path);
    }

    /*
    * Resolve launcher's build.gradle (#GCPSDK4-99)
    * - unity 2019.3에서 launcher, unityLibrary 로 프로젝트가 분리
    * - unityLibrary's build.gradle은 기존 mainTemplate.gradle로 커스텀할 수 있으나, launcher's build.gradle은 커스텀할 수 없는 이슈 존재
    */
    private void resolveLauncherBuildGradle(string path){
        
        string launcherPath = path + "/../launcher/";
        string launcherBuildGradle = launcherPath + "build.gradle";
        string tmpBuildGradle = launcherPath + "tmpBuild.gradle";

        if (!File.Exists(launcherBuildGradle))
        {
            Debug.Log("launcher's build.gradle is not exist");
            return;
        }

        IEnumerable<string> lines = null;
        try
        {
            lines = File.ReadAllLines(launcherBuildGradle);
        }
        catch (Exception ex)
        {
            Debug.Log(String.Format("Unable to read lines {0} ({1})", launcherBuildGradle, ex.ToString()));
        }

        StreamWriter writer = File.CreateText(tmpBuildGradle);
            
        IEnumerator ienum = lines.GetEnumerator();
        while(ienum.MoveNext())
        {
            string line = (string)ienum.Current;

            // resolve targetSDKversion 31, JDK 11 issue
            // https://developers.google.com/ar/develop/unity-arf/android-12-build
            #if UNITY_2022_2_OR_NEWER
            //
            #else
            if(line.Contains("compileSdkVersion 31")) {
                line = "compileSdkVersion 30 // Add Hive for targetSDKversion 31 issue";
            } else if(line.Contains("buildToolsVersion \'31")){
                line = "buildToolsVersion \'30.0.3\' // Add Hive for targetSDKversion 31 issue";
            }
            #endif

            writer.WriteLine(line);
        }

        // google firebase apply plugin
        string firebaseDependenciesXml = Application.dataPath + "/Hive_SDK_v4/Editor/HIVESDK_ProviderFirebaseDependencies.xml";
        if (File.Exists(firebaseDependenciesXml)) {
            if(!checkDuplicateHiveSetting(launcherBuildGradle, "apply plugin: 'com.google.gms.google-services")) {
                writer.WriteLine("apply plugin: 'com.google.gms.google-services'");
            }
        } else {
            Debug.Log("firebase dependecies is not exist");
        }

        // huawei apply plugin
        string huaweiDependenciesXml = Application.dataPath + "/Hive_SDK_v4/Editor/HIVESDK_ProviderHuaweiDependencies.xml";
        if (File.Exists(huaweiDependenciesXml)) {
            if(!checkDuplicateHiveSetting(launcherBuildGradle, "apply plugin: 'com.huawei.agconnect")) {
                writer.WriteLine("apply plugin: 'com.huawei.agconnect'");
            }
            copyToHuaweiServicesJson(path); // huawei

        } else {
            Debug.Log("huawei dependecies is not exist");
        }

        writer.Flush();
        writer.Close();

        File.Delete(launcherBuildGradle);
        File.Move(tmpBuildGradle, launcherBuildGradle);

    }

    /*
    * Resolve launcher's build.gradle (#GCPTAM-391)
    * - unity 2020.1에서 aaptOptions STREAMING_ASSETS 예약어가 사라짐
    */
    private void resolveMainBuildGradle(string path){
        
        string mainPath = path + "/../unityLibrary/";
        string mainBuildGradle = mainPath + "build.gradle";
        string tmpBuildGradle = mainPath + "tmpBuild.gradle";

        if (!File.Exists(mainBuildGradle))
        {
            Debug.Log("main's build.gradle is not exist");
            return;
        }

        IEnumerable<string> lines = null;
        try
        {
            lines = File.ReadAllLines(mainBuildGradle);
        }
        catch (Exception ex)
        {
            Debug.Log(String.Format("Unable to read lines {0} ({1})", mainBuildGradle, ex.ToString()));
            return;
        }

        StreamWriter writer = File.CreateText(tmpBuildGradle);
            
        IEnumerator ienum = lines.GetEnumerator();
        while(ienum.MoveNext())
        {
            string line = (string)ienum.Current;

            // Find and Replace '**STREAMING_ASSETS**'
            if(line.Contains("**STREAMING_ASSETS**")) {
                writer.WriteLine("noCompress = ['.ress', '.resource', '.obb'] + unityStreamingAssets.tokenize(', ')");
            } else {
                writer.WriteLine(line);
            }
        }
        writer.Flush();
        writer.Close();

        File.Delete(mainBuildGradle);
        File.Move(tmpBuildGradle, mainBuildGradle);

    }

    private void resolveMainBuildGradleTargetSDK31(string path){
        
        string mainPath = path + "/../unityLibrary/";
        string mainBuildGradle = mainPath + "build.gradle";
        string tmpBuildGradle = mainPath + "tmpBuild.gradle";

        if (!File.Exists(mainBuildGradle))
        {
            Debug.Log("main's build.gradle is not exist");
            return;
        }

        IEnumerable<string> lines = null;
        try
        {
            lines = File.ReadAllLines(mainBuildGradle);
        }
        catch (Exception ex)
        {
            Debug.Log(String.Format("Unable to read lines {0} ({1})", mainBuildGradle, ex.ToString()));
            return;
        }

        StreamWriter writer = File.CreateText(tmpBuildGradle);
            
        IEnumerator ienum = lines.GetEnumerator();
        while(ienum.MoveNext())
        {
            string line = (string)ienum.Current;

            #if UNITY_2022_2_OR_NEWER
            //
            #else
            if(line.Contains("compileSdkVersion 31")) {
                line = "compileSdkVersion 30 // Add Hive for targetSDKversion 31 issue";
            } else if(line.Contains("buildToolsVersion \'31")){
                line = "buildToolsVersion \'30.0.3\' // Add Hive for targetSDKversion 31 issue";
            }
            #endif

            writer.WriteLine(line);  
        }
        writer.Flush();
        writer.Close();

        File.Delete(mainBuildGradle);
        File.Move(tmpBuildGradle, mainBuildGradle);

    }

    private void resolveUnityResourcesBuildGradleTargetSDK31(string path){
        
        string mainPath = path + "/../unityLibrary/unity-android-resources/";
        string mainBuildGradle = mainPath + "build.gradle";
        string tmpBuildGradle = mainPath + "tmpBuild.gradle";

        if (!File.Exists(mainBuildGradle))
        {
            Debug.Log("main's build.gradle is not exist");
            return;
        }

        IEnumerable<string> lines = null;
        try
        {
            lines = File.ReadAllLines(mainBuildGradle);
        }
        catch (Exception ex)
        {
            Debug.Log(String.Format("Unable to read lines {0} ({1})", mainBuildGradle, ex.ToString()));
            return;
        }

        StreamWriter writer = File.CreateText(tmpBuildGradle);
            
        IEnumerator ienum = lines.GetEnumerator();
        while(ienum.MoveNext())
        {
            string line = (string)ienum.Current;
            
            #if UNITY_2022_2_OR_NEWER
            //
            #else
            if(line.Contains("compileSdkVersion 31")) {
                line = "compileSdkVersion 30 // Add Hive for targetSDKversion 31 issue";
            } else if(line.Contains("buildToolsVersion \'31")){
                line = "buildToolsVersion \'30.0.3\' // Add Hive for targetSDKversion 31 issue";
            }
            #endif

            writer.WriteLine(line);  
        }
        writer.Flush();
        writer.Close();

        File.Delete(mainBuildGradle);
        File.Move(tmpBuildGradle, mainBuildGradle);

    }

    private void resolveBaseBuildGradle(string path){
        
        string basePath = path + "/../";
        string baseBuildGradle = basePath + "build.gradle";
        string tmpBuildGradle = basePath + "tmpBuild.gradle";

        if (!File.Exists(baseBuildGradle))
        {
            Debug.Log("main's build.gradle is not exist");
            return;
        }

        IEnumerable<string> lines = null;
        bool alreadyHiveAdded = false;
        try
        {
            lines = File.ReadAllLines(baseBuildGradle);
            #if UNITY_2022_2_OR_NEWER
            foreach (string line in lines)
            {
                if(line.Contains("Add HIVE")) {
                    Debug.Log("already hive settings added");
                    alreadyHiveAdded = true;
                    break;
                }
            }
            #endif
        }
        catch (Exception ex)
        {
            Debug.Log(String.Format("Unable to read lines {0} ({1})", baseBuildGradle, ex.ToString()));
            return;
        }

        StreamWriter writer = File.CreateText(tmpBuildGradle);
            
        IEnumerator ienum = lines.GetEnumerator();
        while(ienum.MoveNext())
        {
            string line = (string)ienum.Current;

            #if UNITY_2022_2_OR_NEWER
            if(line.Contains("plugins")) {
                writer.WriteLine(line);
                if(!alreadyHiveAdded) {
                    writer.WriteLine("    // Add HIVE");
                    writer.WriteLine("    id 'org.jetbrains.kotlin.android' version '1.5.20' apply false");
                    writer.WriteLine("    id 'com.google.gms.google-services' version '4.3.14' apply false");
                }
            } else {
                writer.WriteLine(line);
            }
            #else
            if(line.Contains("jcenter()")) {
                writer.WriteLine("jcenter()");
                writer.WriteLine("// Add HIVE");
                writer.WriteLine("maven { url 'https://developer.huawei.com/repo/' }");
            } else {
                writer.WriteLine(line);
            }
            #endif
        }
        writer.Flush();
        writer.Close();

        File.Delete(baseBuildGradle);
        File.Move(tmpBuildGradle, baseBuildGradle);

        #if UNITY_2022_2_OR_NEWER
        string huaweiDependenciesXml = Application.dataPath + "/Hive_SDK_v4/Editor/HIVESDK_ProviderHuaweiDependencies.xml";
        if (File.Exists(huaweiDependenciesXml)) {
            includeHuaweiBaseBuildGradle(path);

            // GCPSDK4-1378, HiveSettingsTemplate_2022* 구성하게되어
            // settings.gradle를 후처리하는 부분 제거
            // includeHuaweiSettingsGradle(path);       
        }
        #endif
    }

    private void includeHuaweiBaseBuildGradle(string path) {
        string basePath = path + "/../";
        string baseBuildGradle = basePath + "build.gradle";
        string tmpBuildGradle = basePath + "tmpBuild.gradle";
        bool alreadyHuaweiAdded = false;

        if (!File.Exists(baseBuildGradle))
        {
            Debug.Log("includeHuaweiToBaseBuildGradle, main's build.gradle is not exist");
            return;
        }

        IEnumerable<string> lines = null;
        try
        {
            lines = File.ReadAllLines(baseBuildGradle);
            foreach (string line in lines)
            {
                if(line.Contains("com.huawei.agconnect:agcp")) {
                    Debug.Log("already huawei settings added");
                    alreadyHuaweiAdded = true;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(String.Format("includeHuaweiToBaseBuildGradle, Unable to read lines {0} ({1})", baseBuildGradle, ex.ToString()));
            return;
        }

        // 상위 gradle 파일 읽기에 이상이 없는 경우
        // Huawei 설정 추가 진행
        StreamWriter writer = File.CreateText(tmpBuildGradle);
        IEnumerator ienum = lines.GetEnumerator();
        bool alreadySet = false;

        if(alreadyHuaweiAdded) {

            while(ienum.MoveNext()) {
                string line = (string)ienum.Current;

                // 상위 gradle에 buildscript 내용이 있는 경우
                // Huawei agconnect 설정만 추가
                if(line.Contains("com.huawei.agconnect:agcp")) {
                    string agconnectInfo = "        classpath 'com.huawei.agconnect:agcp:";
                    agconnectInfo += Android_Huawei_Agconnect_Version;
                    agconnectInfo += "'";
                    writer.WriteLine(agconnectInfo);

                    alreadySet = true;
                } else {
                    writer.WriteLine(line);
                }
            }
        } else {

            while(ienum.MoveNext()) {
                string line = (string)ienum.Current;

                // 상위 gradle에 buildscript 내용이 있는 경우
                // Huawei agconnect 설정만 추가
                if(line.Contains("com.android.tools.build:gradle")) {
                    writer.WriteLine(line);
                    writer.WriteLine("        // Add Hive(for Huawei)");
                    string agconnectInfo = "        classpath 'com.huawei.agconnect:agcp:";
                    agconnectInfo += Android_Huawei_Agconnect_Version;
                    agconnectInfo += "'";
                    writer.WriteLine(agconnectInfo);

                    alreadySet = true;
                } else {
                    writer.WriteLine(line);
                }
            }

            // 상위 gradle에 buildscript 내용이 애초 없었던 상태
            if(!alreadySet) {
                writer.Flush();
                writer.Close();     // tmpBuildGradle 삭제를 위해 Close 처리 (OS(ex : Windows)에 따라 Close 처리 안할 경우 파일 접근 불가)
                File.Delete(tmpBuildGradle);

                writer = File.CreateText(tmpBuildGradle);
                ienum = lines.GetEnumerator();

                while(ienum.MoveNext()) {
                    string line = (string)ienum.Current;

                    if(line.Contains("plugins")) {
                        writer.WriteLine("// Add Hive(for Huawei)");
                        writer.WriteLine("buildscript {");
                        writer.WriteLine("    dependencies {");

                        string gradleToolInfo = "        classpath 'com.android.tools.build:gradle:";
                        gradleToolInfo += Android_Huawei_Gradle_Tool_Version;
                        gradleToolInfo += "'";
                        writer.WriteLine(gradleToolInfo);

                        string agconnectInfo = "        classpath 'com.huawei.agconnect:agcp:";
                        agconnectInfo += Android_Huawei_Agconnect_Version;
                        agconnectInfo += "'";
                        writer.WriteLine(agconnectInfo);
                        
                        writer.WriteLine("    }");
                        writer.WriteLine("}");
                        writer.WriteLine("\n");

                        writer.WriteLine(line);

                        alreadySet = true;
                    }
                    else {
                        writer.WriteLine(line);
                    }
                } 
            }
        }

        writer.Flush();
        writer.Close();

        File.Delete(baseBuildGradle);
        File.Move(tmpBuildGradle, baseBuildGradle);
    }

    private void includeHuaweiSettingsGradle(string path) {
        string basePath = path + "/../";
        string baseSettingsGradle = basePath + "settings.gradle";
        string tmpSettingsGradle = basePath + "tmpBuild.gradle";
        int settingsNumHuawei = 0;

        if (!File.Exists(baseSettingsGradle))
        {
            Debug.Log("includeHuaweiSettingsGradle, main's settings.gradle is not exist");
            return;
        }

        IEnumerable<string> lines = null;
        try
        {
            lines = File.ReadAllLines(baseSettingsGradle);
            foreach (string line in lines)
            {
                if(line.Contains("developer.huawei.com/repo")) {
                    Debug.Log("already huawei settings(developer.huawei.com/repo) added : " + ++settingsNumHuawei);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(String.Format("includeHuaweiSettingsGradle, Unable to read lines {0} ({1})", baseSettingsGradle, ex.ToString()));
            return;
        }

        // 상위 settings.gradle 파일 읽기에 이상이 없는 경우
        // Huawei 설정 추가 진행
        StreamWriter writer = File.CreateText(tmpSettingsGradle);
        IEnumerator ienum = lines.GetEnumerator();
        bool isPluginManagement = false;

        if(settingsNumHuawei == 1) {
            while(ienum.MoveNext()) {
                string line = (string)ienum.Current;

                if(line.Contains("pluginManagement")) {
                    isPluginManagement = true;
                }

                if(isPluginManagement && line.Contains("repositories")) {
                    writer.WriteLine(line);
                    writer.WriteLine("        // Add Hive(for Huawei)");
                    writer.WriteLine("        maven { url 'https://developer.huawei.com/repo/' }");

                    isPluginManagement = false;
                } else {
                    writer.WriteLine(line);
                }
            }
        }

        writer.Flush();
        writer.Close();

        File.Delete(baseSettingsGradle);
        File.Move(tmpSettingsGradle, baseSettingsGradle);
    }

    private void copyToGoogleServicesJson(string path) {

        string launcherPath = path + "/../launcher/google-services.json";
        string jsonFile = Application.dataPath + "/google-services.json";

        if (File.Exists(jsonFile)) {
            File.Copy(jsonFile, launcherPath, true);
        } else {
            Debug.Log("google-services.json is not exist");
            return;
        }
    }

    private void copyToHuaweiServicesJson(string path) {

        string launcherPath = path + "/../launcher/agconnect-services.json";
        string jsonFile = Application.dataPath + "/agconnect-services.json";

        if (File.Exists(jsonFile)) {
            File.Copy(jsonFile, launcherPath, true);
        } else {
            Debug.Log("agconnect-services.json is not exist");
            return;
        }
    }

    private bool checkDuplicateHiveSetting(string fileName, string checkStr) {
        IEnumerable<string> lines = null;
        bool isDuplicate = false;
        try
        {
            lines = File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                if(line.Contains(checkStr)) {
                    isDuplicate = true;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(String.Format("Unable to read lines {0} ({1})", fileName, ex.ToString()));
            isDuplicate = false;
        }

        return isDuplicate;
    }
}
#endif