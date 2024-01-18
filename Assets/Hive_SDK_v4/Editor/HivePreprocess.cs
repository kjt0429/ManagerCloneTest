/**
 * @file    HivePreprocess.cs
 * 
 * @author  disker
 * @date    2019-2022
 * @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 * @defgroup UnityEditor.HiveEditor
 * @{
 * @brief PrePrcessing on BuildTime <br/><br/>
 */

namespace UnityEditor.HiveEditor
{
    using UnityEditor;
    using UnityEditor.Build;
    using Hive.Unity.Editor;
    using UnityEngine;
    using System.IO;

#if UNITY_IOS
    using UnityEditor.iOS;
    using UnityEditor.iOS.Xcode;
#endif

#if UNITY_2018_1_OR_NEWER
    using UnityEditor.Build.Reporting;
    public class HivePreProcess : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            if (HiveConfigEditor.isAutoValidation())
            {
                Utility.XmlValidator.getInstance().execute();
            }

            string legacyAndroidResPath = "Assets/Plugins/Android/res";
            string androidResPath = "Assets/HiveSDK/hive.androidlib/src/main/res";
            string legacyAndroidAssetsPath = "Assets/Plugins/Android/assets";
            string androidAssetsPath = "Assets/HiveSDK/hive.androidlib/src/main/assets";

    #if UNITY_2022_2_OR_NEWER
            // GCPPCLIEN-98, Unity 2022 이상에서 리소스 파일 이동하도록 대응
            if(Directory.Exists(legacyAndroidResPath)) {
                if(Directory.Exists(androidResPath)) {
                    Debug.Log("[HivePreprocess.cs][UNITY_2022_2_OR_NEWER] DeleteAsset \"" + legacyAndroidResPath + "\"");
                    AssetDatabase.DeleteAsset(legacyAndroidResPath);
                } else {
                    Debug.Log("[HivePreprocess.cs][UNITY_2022_2_OR_NEWER] Move from \"" + legacyAndroidResPath + "\" to \"" + androidResPath + "\"");
                    CopyFolder(legacyAndroidResPath, androidResPath);   // Directory.Move 하지 않고 복사, Move 사용 시 이슈 발생 (GCPPCLIEN-98 참고)
                    AssetDatabase.DeleteAsset(legacyAndroidResPath);    // AssetDatabase.DeleteAsset 처리로 폴더 recursive 하게 제거 및 폴더의 .meta도 제거
                }
            }

            if(Directory.Exists(legacyAndroidAssetsPath)) {
                if(Directory.Exists(androidAssetsPath)) {
                    Debug.Log("[HivePreprocess.cs][UNITY_2022_2_OR_NEWER] DeleteAsset \"" + legacyAndroidAssetsPath + "\"");
                    AssetDatabase.DeleteAsset(legacyAndroidAssetsPath);
                } else {
                    Debug.Log("[HivePreprocess.cs][UNITY_2022_2_OR_NEWER] Move from \"" + legacyAndroidAssetsPath + "\" to \"" + androidAssetsPath + "\"");
                    CopyFolder(legacyAndroidAssetsPath, androidAssetsPath);     // Directory.Move 하지 않고 복사, Move 사용 시 이슈 발생 (GCPPCLIEN-98 참고)
                    AssetDatabase.DeleteAsset(legacyAndroidAssetsPath);         // AssetDatabase.DeleteAsset 처리로 폴더 recursive 하게 제거 및 폴더의 .meta도 제거
                }
            }

    #elif UNITY_2021_1_OR_NEWER
            /*
                to support unity 2021: Deprecate "Assets/Plugins/Android/res", "Assets/Plugins/Android/assets" (v4.15.6)
                https://jira.com2us.com/jira/browse/GCPSDK4-678
            */
            if(Directory.Exists(legacyAndroidResPath)) {
                if(Directory.Exists(androidResPath)) {
                    Debug.Log("[HivePreProcess.cs][UNITY_2021_1_OR_NEWER] DeleteAsset \"" + legacyAndroidResPath + "\"");
                    AssetDatabase.DeleteAsset(legacyAndroidResPath);
                } else {
                    Debug.Log("[HivePreProcess.cs][UNITY_2021_1_OR_NEWER] Move from \"" + legacyAndroidResPath + "\" to \"" + androidResPath + "\"");
                    AssetDatabase.MoveAsset(legacyAndroidResPath, androidResPath);
                }
            }

            if(Directory.Exists(legacyAndroidAssetsPath)) {
                if(Directory.Exists(androidAssetsPath)) {
                    Debug.Log("[HivePreProcess.cs][UNITY_2021_1_OR_NEWER] DeleteAsset \"" + legacyAndroidAssetsPath + "\"");
                    AssetDatabase.DeleteAsset(legacyAndroidAssetsPath);
                } else {
                    Debug.Log("[HivePreProcess.cs][UNITY_2021_1_OR_NEWER] Move from \"" + legacyAndroidAssetsPath + "\" to \"" + androidAssetsPath + "\"");
                    AssetDatabase.MoveAsset(legacyAndroidAssetsPath, androidAssetsPath);
                }
            }
    #endif

        }

        private void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            string[] files = Directory.GetFiles(sourceFolder);
            string[] folders = Directory.GetDirectories(sourceFolder);

            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }

            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }
    }
#endif

//  IPreprocessBuild is Obsolete on (ver.2018.1~)
#if !UNITY_2018_1_OR_NEWER
    public class HivePreProcess : IPreprocessBuild
    {
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            if (HiveConfigEditor.isAutoValidation())
            {
                Utility.XmlValidator.getInstance().execute();
            }

        }
    }
#endif

}
