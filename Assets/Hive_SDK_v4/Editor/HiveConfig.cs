/**
 * @file    HiveConfig.cs
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
    using System.IO;
    using System.Xml;
    using System.ComponentModel;
    #if UNITY_EDITOR
    using UnityEditor;
    #endif
    using UnityEngine;

    #if UNITY_EDITOR
    [InitializeOnLoad]
    #endif

    /// <summary>
    /// Hive settings.
    /// </summary>
    public class HiveConfig : ScriptableObject
    {
        private const string HiveConfigPath = "Hive_SDK_v4/Editor/Resources";
        private const string HiveConfigAssetName = "HiveConfig";
        private const string HiveConfigAssetExtension = ".asset";
        private static HiveConfig instance;

        private static HiveConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load(HiveConfigAssetName) as HiveConfig;
                    if (instance == null)
                    {
                        // If not found, autocreate the asset object.
                        instance = ScriptableObject.CreateInstance<HiveConfig>();
                        #if UNITY_EDITOR
//                        string properPath = Path.Combine(Application.dataPath, HiveConfigPath);
//                        Debug.Log(properPath);
//                        if (!Directory.Exists(properPath))
//                        {
//                            Directory.CreateDirectory(properPath);
//                        }
//                        //Debug.Log(fullPath);
//                        string fullPath = Path.Combine(
//                            Path.Combine("Assets", HiveConfigPath),
//                            HiveConfigAssetName + HiveConfigAssetExtension);
//                        Debug.Log(fullPath);
//                        Debug.Log(instance);
//                        AssetDatabase.CreateAsset(instance, fullPath);

                        #endif
                    }
                }

                return instance;
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
			
        #if UNITY_EDITOR
        [MenuItem("Hive/Edit Config")]
        public static void Edit()
        {
            Selection.activeObject = Instance;
        }

        [MenuItem("Hive/HIVE Developers Page")]
        public static void OpenAppPage()
        {
            string url = "https://developers.withhive.com/";
            //if (HiveConfig.AppIds[HiveConfig.SelectedAppIndex] != "0")
             //   url += HiveConfig.AppIds[HiveConfig.SelectedAppIndex];
            Application.OpenURL(url);
        }

        [MenuItem("Hive/SDK Documentation")]
        public static void OpenDocumentation()
        {
			string url = "https://developers.withhive.com/api/hive-api/";
            Application.OpenURL(url);
        }

        [MenuItem("Hive/FAQ")]
        public static void OpenFAQ()
        {
            string url = "http://developers.withhive.com/faq/";
            Application.OpenURL(url);
        }
        #endif

    }
}
