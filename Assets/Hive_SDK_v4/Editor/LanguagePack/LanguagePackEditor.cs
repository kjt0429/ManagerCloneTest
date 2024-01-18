/**
 * @file    LanguagePackEditor.cs
 * 
 * @author  pnpsinki
 * @date    2020-2023
 * @copyright Copyright © Com2uS Platform Corporation. All Right Reserved.
 * @defgroup Hive.Unity.Editor
 * @{
 * @brief HIVE LanguagePack Editor Window <br/><br/>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace Hive.Unity.Editor
{
    using LanguagePackDictionary = Dictionary<LanguagePackCode, bool>;
    using LanguagePackSerializableDictionary = SerializableDictionary<string, bool>;

    public class LanguagePackEditor : EditorWindow
    {
        private LanguagePackDictionary LanguagePackDictionary;

        private GUILayoutOption[] layoutOptions = { GUILayout.Width(150), GUILayout.ExpandWidth(false) };

        [MenuItem("Hive/Resource Settings/UI Language (Android Only)")]
        public static void create()
        {
            var editor = (LanguagePackEditor)EditorWindow.GetWindow(typeof(LanguagePackEditor));
            editor.Initialize();
            editor.Show();
        }


        public void Initialize()
        {
            minSize = new Vector2(350, 460);
            position = new Rect(UnityEngine.Screen.width / 3, UnityEngine.Screen.height / 3, minSize.x, minSize.y);

            LanguagePackDictionary = LanguagePackDictionaryManager.LoadLanguagePackDictionaryFromJson();
        }


        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("[UI Language (Android Only)]", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            // en
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("English (en)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.en)){
                LanguagePackDictionary[LanguagePackCode.en] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.en], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // ko
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("한국어 (ko)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.ko)){
                LanguagePackDictionary[LanguagePackCode.ko] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.ko], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // zh-hans
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("简体中文 (zh-hans)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.zh_hans)){
                LanguagePackDictionary[LanguagePackCode.zh_hans] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.zh_hans], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // zh-hant
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("繁體中文 (zh-hant)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.zh_hant)){
                LanguagePackDictionary[LanguagePackCode.zh_hant] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.zh_hant], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // ja
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("日本語 (ja)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.ja)){
                LanguagePackDictionary[LanguagePackCode.ja] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.ja], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // ru
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Pусский (ru)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.ru)){
                LanguagePackDictionary[LanguagePackCode.ru] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.ru], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // fr
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Français (fr)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.fr)){
                LanguagePackDictionary[LanguagePackCode.fr] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.fr], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // de
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Deutsch (de)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.de)){
                LanguagePackDictionary[LanguagePackCode.de] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.de], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // es
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Español (es)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.es)){
                LanguagePackDictionary[LanguagePackCode.es] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.es], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // pt
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("português (pt)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.pt)){
                LanguagePackDictionary[LanguagePackCode.pt] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.pt], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // id
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bahasa Indonesia (id)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.id)){
                LanguagePackDictionary[LanguagePackCode.id] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.id], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // th
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ไทย (th)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.th)){
                LanguagePackDictionary[LanguagePackCode.th] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.th], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // vi
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Tiếng Việt (vi)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.vi)){
                LanguagePackDictionary[LanguagePackCode.vi] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.vi], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // it
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Italiano (it)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.it)){
                LanguagePackDictionary[LanguagePackCode.it] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.it], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // tr
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Türkçe (tr)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.tr)){
                LanguagePackDictionary[LanguagePackCode.tr] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.tr], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();

            // ar
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("الْعَرَبِيَّةُ (ar)", layoutOptions);
            if(LanguagePackDictionary.ContainsKey(LanguagePackCode.ar)){
                LanguagePackDictionary[LanguagePackCode.ar] = EditorGUILayout.Toggle(LanguagePackDictionary[LanguagePackCode.ar], layoutOptions);
            }
            EditorGUILayout.EndHorizontal();


            GUILayout.Space(50);

            if (GUILayout.Button("Reset to Defaults"))
            {
                LanguagePackDictionaryManager.Reset(ref LanguagePackDictionary);
            }

            GUILayout.BeginHorizontal();
            bool closeButton = GUILayout.Button("Cancel");

            bool okButton = GUILayout.Button("OK");
            closeButton |= okButton;
            if (okButton)
            {
                LanguagePackDictionaryManager.SaveLanguagePackDictionaryToJson(ref LanguagePackDictionary);
                AssetDatabase.Refresh();
            }
            if (closeButton) Close();
            GUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
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