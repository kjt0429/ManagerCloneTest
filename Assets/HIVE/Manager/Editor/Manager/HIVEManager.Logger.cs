using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using NUnit.Framework;

namespace hive.manager {
    public partial class HIVEManager {

        #if UNITY_EDITOR_WIN
        const string EDITOR_PLATFORM = "windows";
        #elif UNITY_EDITOR_OSX
        const string EDITOR_PLATFORM = "mac";
        #else
        const string EDITOR_PLATFORM = "etc";
        #endif


        static string GetAppIDForHIVEXml(string platform) {
            string appid = "";
            try {
                Type type = Type.GetType("Hive.Unity.Editor.HiveConfigXML, Assembly-CSharp-Editor");
                var hiveAppIDProperty = type.GetProperty("HIVEAppID", BindingFlags.Public | BindingFlags.Instance);
                var platformProperty = type.GetProperty(platform, BindingFlags.Static | BindingFlags.Public);
                var m = platformProperty.GetGetMethod();
                var instanceObject = m.Invoke(null, null);
                if (instanceObject != null) {
                    m = hiveAppIDProperty.GetGetMethod();
                    var s = m.Invoke(instanceObject, null) as String;
                    if (!String.IsNullOrEmpty(s)) {
                        appid = s;
                    }
                }
            } catch(Exception e) {
                appid = "";
            }

            return appid;
        }

        /// <summary>
        /// 리플렉션을 통해서 HIVE SDK가 설치되어 있을경우 HIVE APP ID를 얻는다
        /// 우선순위는 Android -> iOS 순이고
        /// 리플렉션으로 얻지 못하거나 설치가 되어있지 않을경우 Application.identifier를 반환한다.
        /// </summary>
        /// <returns>App ID</returns>
        static string getAppID() {
            var appid = "";
            try {
                appid = GetAppIDForHIVEXml("Android");
                if(string.IsNullOrEmpty(appid)) {
                    appid = GetAppIDForHIVEXml("iOS");
                }
            } finally {
                if(string.IsNullOrEmpty(appid)) {
                    appid = Application.identifier;
                }
            }
            return appid;
        }

        public void SendSDKInstallLog(string installedVersion, DateTime installTime) {
            LogAnalyticsDownloadDone data = new LogAnalyticsDownloadDone();
            data.companyName = Application.companyName;
            data.productName = Application.productName;
            data.appid = getAppID();
            data.managerVersion = HIVEManagerVersion.Version;
            data.platform = EDITOR_PLATFORM;
            data.sdkVersion = installedVersion;
            data.installDateTime = installTime;
            requestDonwloadDoneAnalyticsLog(data, (exc) => {
                Debug.Log(exc);
            });
        }

        [TestFixture]
        class HIVEManagerLoggerTest {
            [Test] 
            public void getAppIdTest() {
                var s = getAppID();
                TestContext.WriteLine(s);
            }

            [Test]
            public void SendSDKInstallLogTest() {
                HIVEManager manager = new HIVEManager();
                manager.SendSDKInstallLog("4.14.0", DateTime.Now);
            }
        }
    }
}
