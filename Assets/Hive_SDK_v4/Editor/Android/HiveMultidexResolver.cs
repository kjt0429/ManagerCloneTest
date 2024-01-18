
namespace UnityEditor.HiveEditor
{
  using System;
  using System.Text.RegularExpressions;
  using System.Collections;
  using System.Collections.Generic;
  using System.Globalization;
  using System.IO;
  using System.Xml;
  using UnityEditor;
  using UnityEngine;

  public class UnityVersionManager 
  {
    public class Version {
      public int majorVersion = 0;
      public int midVersion = 0;
      public int minVersion = 0;
      public void setVersion(string version) {
        // ex, 5.6.3f2, 2017.1.2a2

        if (version == null) {
          return;
        }

        // 1. 버전 값만 추출한다.
        string[] parsedVersion = version.Split('a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z');

        if( parsedVersion == null || parsedVersion.Length < 1) {
          return;
        }
        string[] versionDetail = parsedVersion[0].Split('.');

        if (versionDetail.Length >= 3) {
          minVersion = int.Parse(versionDetail[2]);
        }
        if (versionDetail.Length >= 2) {
          midVersion = int.Parse(versionDetail[1]);
        } 
        if (versionDetail.Length >= 1) {
          majorVersion = int.Parse(versionDetail[0]);
        }


      }
      public void setVersion(int major, int mid, int min) {
        this.majorVersion = major;
        this.midVersion = mid;
        this.minVersion = min;
      } 

      public int getVersionCode() {
        // 대소 비교를 용이하게 하기 위해 Version Code를 생성한다.
        // majorVersion (4자리) midVersion (2자리) minVersion(2자리)
        return majorVersion * 1000 + midVersion * 100 + minVersion;
      }
    }

    // Unity 버전 관련 Class
    static public Version getUnityVersion() {
      Version reVer = new Version();
      reVer.setVersion(Application.unityVersion);
      return reVer;
    }

    static public bool isNewerThan(string version) {

      int nowVerionCode = getUnityVersion().getVersionCode();
      Version compareVersion = new Version();
      compareVersion.setVersion(version);

      int compareVersionCode = compareVersion.getVersionCode();


      return compareVersionCode < nowVerionCode ? true : false;
    }

    static public bool isNewerOrEqualThan(string version) {
      return isNewerThan(version) || isEqual(version);
    }

    static public bool isEqual(string version) {

      int nowVerionCode = getUnityVersion().getVersionCode();
      Version compareVersion = new Version();
      compareVersion.setVersion(version);

      int compareVersionCode = compareVersion.getVersionCode();


      return compareVersionCode == nowVerionCode ? true : false;
    }

    static public bool isOlderThan(string version) {

      int nowVerionCode = getUnityVersion().getVersionCode();
      Version compareVersion = new Version();
      compareVersion.setVersion(version);

      int compareVersionCode = compareVersion.getVersionCode();


      return compareVersionCode > nowVerionCode ? true : false;
    }

    static public bool isOlderOrEqualThan(string version) {
      return isOlderThan(version) || isEqual(version);
    }
  }
  public class HiveMultidexResolver
    {
        public static string EditorAndroidPath = Path.Combine(Application.dataPath, "Hive_SDK_v4/Editor/Android/");

        #if UNITY_2023_1_OR_NEWER
        public static string HiveMainTemplate = "HiveMainTemplate_2023_1_OR_NEWER.gradle";
        #elif UNITY_2022_2_OR_NEWER
        public static string HiveMainTemplate = "HiveMainTemplate_2022_2_OR_NEWER.gradle";
        #elif UNITY_2019_3_OR_NEWER
        public static string HiveMainTemplate = "HiveMainTemplate_2019_3_OR_NEWER.gradle";
        #else
        public static string HiveMainTemplate = "HiveMainTemplate.gradle";
        #endif
        public static string HiveMainTemplateFilePath = Path.Combine(EditorAndroidPath, HiveMainTemplate);
        
        #if UNITY_2022_2_OR_NEWER
        public static string HiveSettingsTemplate = "HiveSettingsTemplate_2022_2_OR_NEWER.gradle";
        #else
        public static string HiveSettingsTemplate = "HiveSettingsTemplate.gradle";
        #endif
        public static string HiveSettingsTemplateFilePath = Path.Combine(EditorAndroidPath, HiveSettingsTemplate);

        public static string PluginsAndroidPath = Path.Combine(Application.dataPath, "Plugins/Android/");
        public static string mainTemplate = "mainTemplate.gradle";
        public static string MainTemplateFilePath = Path.Combine(PluginsAndroidPath, mainTemplate);
        public static string settingsTemplate = "settingsTemplate.gradle";
        public static string SettingsTemplateFilePath = Path.Combine(PluginsAndroidPath, settingsTemplate);
        public static string AndroidManifestFilePath = Path.Combine(PluginsAndroidPath, "AndroidManifest.xml");

        private static string HIVE_TOKEN = @"HIVE";

        static public void resolveMultidex()
        {
            // Multidex 이슈 해결을 위한 Gradle 파일 생성 및 Manifest 파일 수정
            if (resolveMainTemplate())
            {
                #if UNITY_2019_3_OR_NEWER
                resolveSettingsTemplate();
                #endif

                resolveManifest();
            }

        }


        private static void resolveSettingsTemplate()
        {
            if (File.Exists(HiveSettingsTemplateFilePath))
            {
                if (File.Exists(SettingsTemplateFilePath))
                {
                  File.Delete(SettingsTemplateFilePath);
                }
                FileUtil.CopyFileOrDirectory(HiveSettingsTemplateFilePath, SettingsTemplateFilePath);
            }
            else
            {
                Debug.Log("Not exist HiveSettingsTemplate.gradle");
            }
        }

        private static bool resolveMainTemplate()
        {
            if (File.Exists(HiveMainTemplateFilePath))
            {
                if (File.Exists(MainTemplateFilePath))
                {
                    IEnumerable<string> lines = null;
                    try
                    {
                        lines = File.ReadAllLines(MainTemplateFilePath);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(String.Format("Unable to read lines {0} ({1})", MainTemplateFilePath, ex.ToString()));
                    }

                    Regex hiveToken = new Regex(HIVE_TOKEN);
                    foreach (var line in lines)
                    {
                        if (hiveToken.IsMatch(line))
                        {
                            return true;
                        }
                    }

                    // Hive mainTemplate.gradle 베이스가 아닌 경우
                    File.Delete(MainTemplateFilePath);
                }
                else
                {
                    GooglePlayServices.PlayServicesResolver.DeleteResolvedLibrariesSync();
                }
                FileUtil.CopyFileOrDirectory(HiveMainTemplateFilePath, MainTemplateFilePath);
                #if UNITY_2020_1_OR_NEWER
                FixMainBuildGradle(MainTemplateFilePath);
                #endif

                return true;
            }
            else
            {
                Debug.Log("Not exist hive mainTemplate.gradle");
                return false;
            }
        }

        private static void resolveManifest()
        {
            // Android Manifest 파일의 Application 항목 수정
            var manifestFilePath = Path.Combine(Application.dataPath, AndroidManifestFilePath);

            XmlDocument doc = new XmlDocument();
            doc.Load(manifestFilePath);

            if (doc == null)
            {
                Debug.LogError("Couldn't load " + manifestFilePath);
                return;
            }

            XmlNode manifestNode = FindChildNode(doc, "manifest");
            XmlNode applicationNode = FindChildNode(manifestNode, "application");

            if (applicationNode == null)
            {
                Debug.LogError("Error parsing " + manifestFilePath);
                return;
            }

            // Save the document formatted
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(manifestFilePath, settings))
            {
                doc.Save(xmlWriter);
            }
        }

        /*
        * Resolve launcher's build.gradle (#GCPTAM-391)
        * - unity 2020.1에서 aaptOptions STREAMING_ASSETS 예약어가 사라짐
        */
        private static void FixMainBuildGradle(string path){
          
            string mainBuildGradle = path;
            string tmpBuildGradle = mainBuildGradle + ".tmp";

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
                } else if(line.Contains("**PROGUARD_DEBUG**")) {
                  // Skip & Remove Line
                } else if(line.Contains("**PROGUARD_RELEASE**")) {
                  // Skip & Remove Line
                }
                else {
                  writer.WriteLine(line);
                }

                
            }
            writer.Flush();
            writer.Close();

            File.Delete(mainBuildGradle);
            File.Move(tmpBuildGradle, mainBuildGradle);

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
    }
}