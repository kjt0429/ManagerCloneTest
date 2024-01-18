/**
 * @file    LanguagePackBuildPostProcessor.cs
 * 
 * @author  pnpsinki
 * @date    2016-2022
 * @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 * @defgroup UnityEditor.HiveEditor
 * @{
 * @brief PostPrcessing on BuildTime <br/><br/>
 */

namespace UnityEditor.HiveEditor
{
    using System.IO;
    using UnityEditor;
    using Debug = UnityEngine.Debug;
    using System;
    using Hive.Unity.Editor;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;
    using UnityEditor.Android;

    public class LanguagePackBuildPostProcessor : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder => 1000;
   
        void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path)
        {
#if UNITY_ANDROID
            resolveLauncherBuildGradle(path);
#endif
        }

        private static void resolveLauncherBuildGradle(string path)
        {
            string launcherPath = path + "/../launcher/";
            string launcherBuildGradle = launcherPath + "build.gradle";
            string tmpBuildGradle = launcherPath + "tmpBuild2.gradle";
            
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

            bool checkDuplicateResConfigs = checkDuplicateHiveSetting(launcherBuildGradle, "ADDED_HIVE_RESOURCE_SETTING"); // ADDED_HIVE_RESOURCE_SETTING 선언 여부 확인
            string resConfigsStr = LanguagePackDictionaryManager.GetLanguageResourceConfigs() + " // ADDED_HIVE_RESOURCE_SETTING";

            IEnumerator ienum = lines.GetEnumerator();
            while (ienum.MoveNext())
            {
                string line = (string)ienum.Current;

                // ADDED_HIVE_RESOURCE_SETTING 이 없는 경우에만 write
                if (!line.Contains("ADDED_HIVE_RESOURCE_SETTING"))
                {
                    writer.WriteLine(line);
                }

                /** 
                 *  1. 기존에 언어 선택기를 통해 "ADDED_HIVE_RESOURCE_SETTING" 주석이 선언되어 있는지를 체크
                 *  2. ADDED_HIVE_RESOURCE_SETTING 선언된 적이 없는 경우 : 언어가 하나라도 선택이 안되어 있으면 기존의 resConfigs 설정 추가, 모두 선택된 디폴트 상태라면 resConfigs 설정 안하고 "ADDED_HIVE_RESOURCE_SETTING" 주석 추가.
                 *  3. 이미 ADDED_HIVE_RESOURCE_SETTING 선언되어 있는 경우 : 언어가 하나라도 선택이 안되어 있으면 기존의 resConfigs 설정을 갱신, 모두 선택된 디폴트 상태라면 resConfigs 설정 안하고 "ADDED_HIVE_RESOURCE_SETTING" 주석은 유지함.
                 */
                if (!checkDuplicateResConfigs)
                {
                    Debug.Log("ResConfigs checkDuplicateResConfigs is " + checkDuplicateResConfigs);
                    if (line.Contains("defaultConfig"))
                    {
                        writer.WriteLine(resConfigsStr);
                    }
                }
                else
                {
                    if (line.Contains("ADDED_HIVE_RESOURCE_SETTING"))
                    {
                        string replaceLine = line.Replace(line, resConfigsStr);
                        writer.WriteLine(replaceLine);
                    }
                }
            }

            writer.Flush();
            writer.Close();

            File.Delete(launcherBuildGradle);
            File.Move(tmpBuildGradle, launcherBuildGradle);
        }

        private static bool checkDuplicateHiveSetting(string fileName, string checkStr)
        {
            IEnumerable<string> lines = null;
            bool isDuplicate = false;
            try
            {
                lines = File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    if (line.Contains(checkStr))
                    {
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
}
