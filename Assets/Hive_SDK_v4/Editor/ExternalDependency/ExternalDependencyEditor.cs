/**
 * @file    ExternalDependencyEditor.cs
 * 
 * @author  disker
 * @date    2020-2022
 * @copyright Copyright © Com2uS Platform Corporation. All Right Reserved.
 * @defgroup Hive.Unity.Editor
 * @{
 * @brief HIVE External Dependency EditorWindow <br/><br/>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace Hive.Unity.Editor
{
    using ExternalDependencyDictionary = Dictionary<ExternalDependencyType, bool>;
    using ExternalDependencySerializableDictionary = SerializableDictionary<string, bool>;

    public class ExternalDependencyEditor : EditorWindow
    {
        private ExternalDependencyDictionary externalDependencyDictionary;

        private GUILayoutOption[] layoutOptions = { GUILayout.Width(150), GUILayout.ExpandWidth(false) };
        private GUILayoutOption[] subLayoutOptions = { GUILayout.Width(135), GUILayout.Height(45), GUILayout.ExpandWidth(true) };


        [MenuItem("Hive/ExternalDependency")]
        public static void create()
        {
            var editor = (ExternalDependencyEditor)EditorWindow.GetWindow(typeof(ExternalDependencyEditor));
            editor.Initialize();
            editor.Show();
        }


        public void Initialize()
        {
            minSize = new Vector2(350, 585);
            position = new Rect(UnityEngine.Screen.width / 3, UnityEngine.Screen.height / 3, minSize.x, minSize.y);

            externalDependencyDictionary = ExternalDependencyDictionaryManager.LoadExternalDependencyDictionaryFromJson();
        }


        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("[IDP]", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            // Google
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Google", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.Google)){
                externalDependencyDictionary[ExternalDependencyType.Google] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.Google], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // Facebook
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Facebook", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.Facebook)){
                externalDependencyDictionary[ExternalDependencyType.Facebook] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.Facebook], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // Line
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("LINE", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.Line))
                externalDependencyDictionary[ExternalDependencyType.Line] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.Line], layoutOptions);
            EditorGUILayout.EndHorizontal();

            // VK
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("VK", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.VK))
                externalDependencyDictionary[ExternalDependencyType.VK] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.VK], layoutOptions);
            EditorGUILayout.EndHorizontal();

            // QQ
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("QQ", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.QQ))
                externalDependencyDictionary[ExternalDependencyType.QQ] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.QQ], layoutOptions);
            EditorGUILayout.EndHorizontal();

            // Wechat
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Wechat", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.Wechat))
                externalDependencyDictionary[ExternalDependencyType.Wechat] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.Wechat], layoutOptions);
            EditorGUILayout.EndHorizontal();
            
            // Weverse
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Weverse", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.Weverse))
                externalDependencyDictionary[ExternalDependencyType.Weverse] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.Weverse], layoutOptions);
            EditorGUILayout.EndHorizontal();

            // Huawei
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Huawei", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.Huawei))
                externalDependencyDictionary[ExternalDependencyType.Huawei] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.Huawei], layoutOptions);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("- Add the 'agconnect-services.json' to your app.", subLayoutOptions);
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;

            EditorGUI.indentLevel--;
            GUILayout.Space(20);

            GUILayout.Label("[Analytics]", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            // Adjust
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Adjust", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.Adjust))
                externalDependencyDictionary[ExternalDependencyType.Adjust] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.Adjust], layoutOptions);
            EditorGUILayout.EndHorizontal();

            // Singular
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Singular", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.Singular))
                externalDependencyDictionary[ExternalDependencyType.Singular] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.Singular], layoutOptions);
            EditorGUILayout.EndHorizontal();

            // AppsFlyer
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("AppsFlyer", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.AppsFlyer))
                externalDependencyDictionary[ExternalDependencyType.AppsFlyer] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.AppsFlyer], layoutOptions);
            EditorGUILayout.EndHorizontal();

            // Firebase
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Firebase", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.Firebase))
                externalDependencyDictionary[ExternalDependencyType.Firebase] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.Firebase], layoutOptions);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("- Add the 'google-services.json' to your app.", subLayoutOptions);
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;

            EditorGUI.indentLevel--;
            GUILayout.Space(20);


            GUILayout.Label("[etc]", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            // reCAPTCHA
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("reCAPTCHA", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.Recaptcha))
                externalDependencyDictionary[ExternalDependencyType.Recaptcha] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.Recaptcha], layoutOptions);
            EditorGUILayout.EndHorizontal();

            // Hercules
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Hercules", layoutOptions);
            if(externalDependencyDictionary.ContainsKey(ExternalDependencyType.Hercules))
                externalDependencyDictionary[ExternalDependencyType.Hercules] = EditorGUILayout.Toggle(externalDependencyDictionary[ExternalDependencyType.Hercules], layoutOptions);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
            GUILayout.Space(20);


            GUILayout.Space(50);

            if (GUILayout.Button("Reset to Defaults"))
            {
                ExternalDependencyDictionaryManager.Reset(ref externalDependencyDictionary);
            }

            GUILayout.BeginHorizontal();
            bool closeButton = GUILayout.Button("Cancel");

            bool okButton = GUILayout.Button("OK");
            closeButton |= okButton;
            if (okButton)
            {
                SetupEDM4UExternalDependency();
                SetupIncludedExternalDependency();
                ExternalDependencyDictionaryManager.SaveExternalDependencyDictionaryToJson(ref externalDependencyDictionary);
                AssetDatabase.Refresh();

                ResolveEDM4U();
            }
            if (closeButton) Close();
            GUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }


        private void SetupEDM4UExternalDependency()
        {
            ExternalDependencyType[] edm4uExternalDependencies =
            {
                ExternalDependencyType.Google,
                ExternalDependencyType.Facebook,
                ExternalDependencyType.Line,
                ExternalDependencyType.Wechat,
                ExternalDependencyType.VK,
                ExternalDependencyType.QQ,
                ExternalDependencyType.Weverse,
                ExternalDependencyType.Huawei,
                ExternalDependencyType.Adjust,
                ExternalDependencyType.AppsFlyer,
                ExternalDependencyType.Singular,
                ExternalDependencyType.Firebase,
                ExternalDependencyType.Recaptcha,
                ExternalDependencyType.Hercules
            };

            string editorPath = Path.Combine(Application.dataPath, "Hive_SDK_v4/Editor");
            string dependenciesPath = Path.Combine(Application.dataPath, "Hive_SDK_v4/Dependencies");

            DirectoryCopy(dependenciesPath, editorPath);

            DirectoryInfo editorDirInfo = new DirectoryInfo(editorPath);
            foreach (ExternalDependencyType t in edm4uExternalDependencies)
            {
                if (externalDependencyDictionary[t])
                    continue;

                foreach (FileInfo f in editorDirInfo.GetFiles())
                {
                    if (f.Name.Contains(Enum.GetName(typeof(ExternalDependencyType), t) + "Dependencies.xml"))
                        f.Delete();
                }
            }

        }


        private void SetupIncludedExternalDependency()
        {
            SetupIncludedExternalDependencyForAndroid();
            SetupIncludedExternalDependencyForIOS();
        }


        private void SetupIncludedExternalDependencyForAndroid()
        {
            var libPath = "Assets/Hive_SDK_v4/Plugins/Android/libs";

            if (!Directory.Exists(libPath))
            {
                Directory.CreateDirectory(libPath);
            }

            DirectoryInfo libDirInfo = new DirectoryInfo(libPath);
            FileInfo[] libFileInfoes = libDirInfo.GetFiles();
            if (libFileInfoes == null || libFileInfoes.Length == 0)
            {
                Debug.Log(libPath + "\n" + "FileInfo[] is null or empty");
                return;
            }

            foreach (FileInfo f in libFileInfoes)
            {
                if (f.Extension.ToLower().Equals(".meta"))
                    continue;

                PluginImporter plugin = PluginImporter.GetAtPath(libPath + "/" + f.Name) as PluginImporter;

                if (f.Name.Contains("open_sdk_"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.QQ];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.Android, enable);
                }
                else if (f.Name.Contains("mid-sdk-"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.QQ];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.Android, enable);
                }
                else if (f.Name.Contains("mta-sdk-"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.QQ] || externalDependencyDictionary[ExternalDependencyType.Wechat];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.Android, enable);
                }

                // Google
                else if (f.Name.Contains("hive-service-extension-google-"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.Google];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.Android, enable);
                }

                // Facebook
                else if (f.Name.Contains("hive-service-extension-facebook-"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.Facebook];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.Android, enable);
                }

                try
                {
                    plugin.SaveAndReimport();
                }
                catch (Exception ex)
                {
                    Debug.Log(String.Format("{0} is not allocated as PluginImporter. \n {1}", f.Name, ex.ToString()));
                }
                
            }

        }


        private void SetupIncludedExternalDependencyForIOS()
        {
            var frameworkPath = "Assets/Hive_SDK_v4/Plugins/iOS/framework";

            if (!Directory.Exists(frameworkPath))
            {
                Directory.CreateDirectory(frameworkPath);
            }

            DirectoryInfo frameworkDirInfo = new DirectoryInfo(frameworkPath);
            DirectoryInfo[] frameworkDirInfoes = frameworkDirInfo.GetDirectories();
            if (frameworkDirInfoes == null || frameworkDirInfoes.Length == 0)
            {
                Debug.Log(frameworkPath + "\n" + "DirectoryInfo[] is null or empty");
                return;
            }

            foreach (DirectoryInfo d in frameworkDirInfoes)
            {
                PluginImporter plugin = PluginImporter.GetAtPath(frameworkPath + "/" + d.Name) as PluginImporter;

                if (d.Name.Equals("WXApi.framework"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.Wechat];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);

                }
                else if (d.Name.Equals("TencentOpenAPI.framework"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.QQ];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }
                else if (d.Name.Equals("VKSdk.framework"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.VK];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }
                else if (d.Name.Equals("ProviderAdapter.framework"))
                {
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, true);
                }

                /*
                    HIVE 제공 framework
                */
                else if (d.Name.Contains("ProviderWechat"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.Wechat];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }
                else if (d.Name.Contains("ProviderVK"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.VK];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }
                else if (d.Name.Contains("ProviderSingular"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.Singular];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }
                else if (d.Name.Contains("ProviderQQ"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.QQ];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }
                else if (d.Name.Contains("ProviderLine"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.Line];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }
                else if (d.Name.Contains("ProviderWeverse"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.Weverse];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }
                else if (d.Name.Contains("ProviderAppsFlyer"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.AppsFlyer];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }
                else if (d.Name.Contains("ProviderAdjust"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.Adjust];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }
                else if (d.Name.Contains("ProviderFirebase"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.Firebase];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }
                else if (d.Name.Contains("HiveRecaptcha"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.Recaptcha];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }
                else if (d.Name.Contains("Hercules"))
                {
                    bool enable = externalDependencyDictionary[ExternalDependencyType.Hercules];
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(false);
                    plugin.SetCompatibleWithPlatform(BuildTarget.iOS, enable);
                }

                try
                {
                    plugin.SaveAndReimport();
                }
                catch (Exception ex)
                {
                    Debug.Log(String.Format("{0} is not allocated as PluginImporter. \n {1}", d.Name, ex.ToString()));
                }
            }
        }


        private void ResolveEDM4U()
        {
#if UNITY_ANDROID
            string mainTemplatePath = Path.Combine(Application.dataPath, "Plugins/Android/mainTemplate.gradle");

            // platfrom Android 설정된 상태에서 mainTemplate.gradle이 존재하지 않은채 Resolve 수행시, 프리징
            if (File.Exists(mainTemplatePath))
            {
                GooglePlayServices.PlayServicesResolver.Resolve(null, true, null);
                GooglePlayServices.PlayServicesResolver.ResolveSync(true);
            }
            else
            {
                Debug.Log("Failed EDM4U Resolve. Please check if the file(Plugins/Android/mainTemplate.gradle) exist.");
            }
#endif
        }


        private bool IsFile(string path, string file)
        {
            string[] files = Directory.GetFiles(path, file);
            return files.Length > 0 ? true : false;
        }


        private void DirectoryCopy(string sourcePath, string destPath)
        {
            DirectoryInfo dir = new DirectoryInfo(sourcePath);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + dir
                );
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(".meta"))
                    continue;
                string tempPath = Path.Combine(destPath, file.Name);
                file.CopyTo(tempPath, true);
            }
        }

    }


}