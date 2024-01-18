/**
 * @file    ExternalDependencyDictionaryManager.cs
 * 
 * @date    2020-2022
 * @copyright Copyright © Com2uS Platform Corporation. All Right Reserved.
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

    public class ExternalDependencyDictionaryManager
    {
        
        private static string jsonDataPath = Path.Combine(Application.dataPath, "Hive_SDK_v4/Editor/ExternalDependency/externalDependencyDictionary.json");
        private static string legacyJsonDataPath = Path.Combine(Application.dataPath, "Hive_SDK_v4/Editor/externalDependencyDictionary.json");

        public enum LegacyExternalDependencyType
        {
            // IDP
            Google,
            Facebook,
            Line,
            VK,
            QQ,
            Wechat,

            // Analytics
            Adjust,
            Singular,
            AppsFlyer,
            Firebase,

            // Push
            Fcm,

            // Etc
            Recaptcha,
            AndroidEmoji,
        }

        public static ExternalDependencyDictionary LoadExternalDependencyDictionaryFromJson()
        {
            var dictionary = new ExternalDependencyDictionary();
            Reset(ref dictionary);

            // ~Hive SDK v4.16.1 마이그레이션 처리
            if (File.Exists(legacyJsonDataPath))
            {
                string jsonDataString = File.ReadAllText(legacyJsonDataPath);
                ExternalDependencySerializableDictionary serializableDictionary = JsonUtility.FromJson<ExternalDependencySerializableDictionary>(jsonDataString);

                foreach (KeyValuePair<string, bool> pair in serializableDictionary)
                {
                    int i = 0;
                    bool isNumbericKey = int.TryParse(pair.Key, out i);

                    if(isNumbericKey)
                    {   
                        string legacyName = Enum.GetName(typeof(LegacyExternalDependencyType), i);
                        ExternalDependencyType externalDependencyType;
                        if (Enum.TryParse(legacyName, out externalDependencyType))
                        {
                            if (Enum.IsDefined(typeof(ExternalDependencyType), externalDependencyType))
                            {
                                dictionary[externalDependencyType] = pair.Value;
                            }
                            else
                            {
                                Debug.Log(String.Format("{0} is not an underlying value of the ExternalDependencyType enumeration.", pair.Key));
                                continue;
                            }
                        }
                        else
                        {
                            Debug.Log(String.Format("{0} is not a member of the ExternalDependencyType enumeration.", pair.Key));
                            continue;
                        }
                    }
                    else
                    {
                        Debug.Log(String.Format("{0} is broken.", legacyJsonDataPath));
                    }
                }

                try
                {
                    File.Delete(legacyJsonDataPath);
                }
                catch (Exception ex)
                {
                    Debug.Log(String.Format("File.Delete({0}) Exception. \n {1}", legacyJsonDataPath, ex.ToString()));
                }

            }
            else if (File.Exists(jsonDataPath))
            {
                string jsonDataString = File.ReadAllText(jsonDataPath);
                ExternalDependencySerializableDictionary serializableDictionary = JsonUtility.FromJson<ExternalDependencySerializableDictionary>(jsonDataString);

                foreach (KeyValuePair<string, bool> pair in serializableDictionary)
                {
                    ExternalDependencyType externalDependencyType;
                    if (Enum.TryParse(pair.Key, out externalDependencyType))
                    {
                        if (Enum.IsDefined(typeof(ExternalDependencyType), externalDependencyType))
                        {
                            dictionary[externalDependencyType] = pair.Value;
                        }
                        else
                        {
                            Debug.Log(String.Format("{0} is not an underlying value of the ExternalDependencyType enumeration.", pair.Key));
                            continue;
                        }
                    }
                    else
                    {
                        Debug.Log(String.Format("{0} is not a member of the ExternalDependencyType enumeration.", pair.Key));
                        continue;
                    }
                }

            }

            return dictionary;
        }

        public static void SaveExternalDependencyDictionaryToJson(ref ExternalDependencyDictionary externalDependencyDictionary)
        {
            // convert dictionary to serializable-dictionary
            ExternalDependencySerializableDictionary serializableDictionary = new ExternalDependencySerializableDictionary();

            foreach (var pair in externalDependencyDictionary)
            {
                var name = Enum.GetName(typeof(ExternalDependencyType), pair.Key);
                serializableDictionary[name] = pair.Value;
            }

            string externalDependencyDictionaryJsonString = JsonUtility.ToJson(serializableDictionary);
            File.WriteAllText(jsonDataPath, externalDependencyDictionaryJsonString);
        }

        public static void Reset(ref ExternalDependencyDictionary dictionary)
        {
            dictionary.Clear();

            foreach (ExternalDependencyType i in Enum.GetValues(typeof(ExternalDependencyType)))
            {
                if(i == ExternalDependencyType.Huawei) {
                    dictionary[i] = false;       
                } else {
                    dictionary[i] = true;
                }
            }
        }
    }

}