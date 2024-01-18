/**
 * @file    Utility.cs
 * 
 * @author  nanomech
 * @date    2016-2022
 * @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 * @defgroup Hive.Unity.Editor
 * @{
 * @brief HIVE SDK Editor Utility <br/><br/>
 */


namespace Hive.Unity.Editor
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEngine;
    using System.Xml;
    using System.Xml.Schema;
    using UnityEditor.Build;

    

    public static class Utility
    {
        public static T Pop<T>(this IList<T> list)
        {
            if (!list.Any())
            {
                throw new InvalidOperationException("Attempting to pop item on empty list.");
            }

            int index = list.Count - 1;
            T value = list[index];
            list.RemoveAt(index);
            return value;
        }

        public static bool TryGetValue<T>(
            this IDictionary<string, object> dictionary,
            string key,
            out T value)
        {
            object resultObj;
            if (dictionary.TryGetValue(key, out resultObj) && resultObj is T)
            {
                value = (T)resultObj;
                return true;
            }

            value = default(T);
            return false;
        }

        public static string GetEnumDescription(Enum value)
        {
            DescriptionAttribute[] attributes = 
                (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static int GetUnityVersion()
        {
            //Unity 2017 = 2017000+
            //Unity 5 = 5000+
            //Unity 4 = 4000+
            //Unity 5.3.3 = 5330
            //Unity 4.7.1 = 4710
            //Unity 5.4.2f3 = 5420 //final version is not patch version.
            //Unity 5.6.3p2 = 5632
            //Unity 5.5.3p4 = 5534
            //Unity 5.4.2f2 = 5420
            //Unity 2017.3.0b8 = 2017300
            // UnityEngine.Debug.Log("FullVersion "+Application.unityVersion);
            char[] delimiterChars = {'.','p'};
            string[] Version = Application.unityVersion.Split(delimiterChars);
            int majorVersion;
            if (!int.TryParse(Version[0], out majorVersion))
            {
                majorVersion = 4;
            }
            int minorVersion;
            if (!int.TryParse(Version[1], out minorVersion))
            {
                minorVersion = 0;
            }
            int releaseVersion;
            if (!int.TryParse(Version[2], out releaseVersion))
            {
                // fixversion split
                char[] delimiterFixChars = {'a','b','f'};
                string[] SubVersion = Version[2].Split(delimiterFixChars);
                if (!int.TryParse(SubVersion[0], out releaseVersion))
                {
                    releaseVersion = 0;
                }
            }
            int patchVersion;
            if (Version.Length < 4 || !int.TryParse(Version[3], out patchVersion))
            {
                patchVersion = 0;
            } 
            return majorVersion*1000 + minorVersion*100 + releaseVersion*10 + patchVersion;
        }


        public class XmlValidator
        {
            private static XmlValidator _instance;
            private string schemaUri;       //  The URI that specifies the schema to load.
            private string inputUri;        //  The URI for the file containing the XML data.
            private XmlValidator()
            {
                this.schemaUri = "Assets/Hive_SDK_v4/Editor/config_schema.xsd";

                #if UNITY_ANDROID
                    #if UNITY_2021_1_OR_NEWER
                    this.inputUri = "Assets/HiveSDK/hive.androidlib/src/main/res/raw/hive_config.xml";
                    #else
                    this.inputUri = "Assets/Plugins/Android/res/raw/hive_config.xml";
                    #endif
                #endif

                #if UNITY_IOS
                this.inputUri = "Assets/Plugins/iOS/hive_config.xml";
                #endif
            }

            public static XmlValidator getInstance(){
                if(_instance == null){
                    _instance = new XmlValidator();
                }
                return _instance;
            }
            private void validationEventHandler(object sender, ValidationEventArgs e)
            {
                if (e.Severity == XmlSeverityType.Warning)
                {
                    throw new BuildFailedException("[hive_config.xml] WARNING: " + e.Message);
                }
                else if (e.Severity == XmlSeverityType.Error)
                {
                    throw new BuildFailedException("[hive_config.xml] ERROR: " + e.Message);
                }
            }

            public void execute()
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.Schemas.Add(null, schemaUri);
                readerSettings.ValidationType = ValidationType.Schema;
                readerSettings.ValidationEventHandler += new ValidationEventHandler(validationEventHandler);

                XmlReader xmlReader = XmlReader.Create(inputUri, readerSettings);

                while (xmlReader.Read()) { }
            }
        }
    }
}
