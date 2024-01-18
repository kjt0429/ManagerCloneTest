/**
 * @file    LanguagePackDictionaryManager.cs
 * 
 * @date    2020-2023
 * @copyright Copyright © Com2uS Platform Corporation. All Right Reserved.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

namespace Hive.Unity.Editor
{
    using LanguagePackDictionary = Dictionary<LanguagePackCode, bool>;
    using LanguagePackSerializableDictionary = SerializableDictionary<string, bool>;

    public class LanguagePackDictionaryManager
    {
        
        private static string jsonDataPath = Path.Combine(Application.dataPath, "Hive_SDK_v4/Editor/LanguagePack/languagePackDictionary.json");

        public static LanguagePackDictionary LoadLanguagePackDictionaryFromJson()
        {
            var dictionary = new LanguagePackDictionary();
            Reset(ref dictionary);

            if (File.Exists(jsonDataPath))
            {
                string jsonDataString = File.ReadAllText(jsonDataPath);
                LanguagePackSerializableDictionary serializableDictionary = JsonUtility.FromJson<LanguagePackSerializableDictionary>(jsonDataString);

                foreach (KeyValuePair<string, bool> pair in serializableDictionary)
                {
                    LanguagePackCode LanguagePackCode;
                    if (Enum.TryParse(pair.Key, out LanguagePackCode))
                    {
                        if (Enum.IsDefined(typeof(LanguagePackCode), LanguagePackCode))
                        {
                            dictionary[LanguagePackCode] = pair.Value;
                        }
                        else
                        {
                            Debug.Log(String.Format("{0} is not an underlying value of the LanguagePackCode enumeration.", pair.Key));
                            continue;
                        }
                    }
                    else
                    {
                        Debug.Log(String.Format("{0} is not a member of the LanguagePackCode enumeration.", pair.Key));
                        continue;
                    }
                }

            }

            return dictionary;
        }

        public static void SaveLanguagePackDictionaryToJson(ref LanguagePackDictionary dictionary)
        {
            // convert dictionary to serializable-dictionary
            LanguagePackSerializableDictionary serializableDictionary = new LanguagePackSerializableDictionary();
            foreach (var pair in dictionary)
            {
                var name = Enum.GetName(typeof(LanguagePackCode), pair.Key);
                serializableDictionary[name] = pair.Value;
            }

            string LanguagePackDictionaryJsonString = JsonUtility.ToJson(serializableDictionary);
            File.WriteAllText(jsonDataPath, LanguagePackDictionaryJsonString);
        }

        public static void Reset(ref LanguagePackDictionary dictionary)
        {
            dictionary.Clear();

            foreach (LanguagePackCode i in Enum.GetValues(typeof(LanguagePackCode)))
            {
                dictionary[i] = true;
            }
        }

        public static string GetLanguageResourceConfigs()
        {
            bool isResEdited = false;
            LanguagePackDictionary dictionary = LoadLanguagePackDictionaryFromJson();

            StringBuilder sb = new StringBuilder();
            sb.Append("resConfigs ");

            int checkedItemCount = 0;

            foreach (KeyValuePair<LanguagePackCode, bool> item in dictionary)
            {
                if(item.Value == true)
                {
                    if(item.Key.ToString() == "zh_hans")
                    {
                        sb.Append("\"zh-rCN\"");
                        sb.Append(",");
                        sb.Append("\"zh-rSG\"");
                    }
                    else if(item.Key.ToString() == "zh_hant")
                    {
                        sb.Append("\"zh-rTW\"");
                        sb.Append(",");
                        sb.Append("\"zh-rHK\"");
                        sb.Append(",");
                        sb.Append("\"zh-rMO\"");
                    }
                    else
                    {
                        sb.Append("\"");
                        sb.Append(item.Key.ToString());
                        sb.Append("\"");
                    }
                    
                    sb.Append(",");
                }
                else
                {
                    // 언어 선택기 체크박스 항목이 변경 되었는지 확인(하나라도 있으면 true)
                    isResEdited = true;
                    checkedItemCount++;
                }
            }

            string result = "";
            if(isResEdited) {
                if(dictionary.Count != checkedItemCount)
                {
                    result = sb.ToString().TrimEnd(',');
                }
            }

            return result;
        }
    }
}