/**
 * @file    HiveConfigXML.cs
 * 
 * @author  nanomech
 * @date    2016-2022
 * @copyright	Copyright © Com2uS Platform Corporation. All Right Reserved.
 * @defgroup Hive.Unity.Editor
 * @{
 * @brief hive_config,xml 생성/편집/출력 기능 <br/><br/>
 */
namespace Hive.Unity.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.ComponentModel;
    using UnityEngine;
    using UnityEditor;

    public class HiveConfigXML
    {
        #if UNITY_2021_1_OR_NEWER
        private const string HiveConfigAndroidResourcePath = "HiveSDK/hive.androidlib/src/main/res/raw/";
        #else
        private const string HiveConfigAndroidResourcePath = "Plugins/Android/res/raw/";
        #endif
        private const string HiveConfigiOSResourcePath = "Plugins/iOS/";
        private const string HiveConfigmacOSResourcePath = "Plugins/macOS/res/";
        private const string HiveConfigWindowsResourcePath = "Plugins/Windows/res/";
        private const string HiveConfigXMLName = "hive_config";
        private const string HiveConfigXMLExtension = ".xml";
        private static HiveConfigXML instanceAndroid = null;
        private static HiveConfigXML instanceiOS = null;
        private static HiveConfigXML instancemacOS = null;
        private static HiveConfigXML instanceWidowsOS = null;

        public static HiveConfigXML Android{
            get{
                if (instanceAndroid == null)
                {
                    instanceAndroid = new HiveConfigXML(HiveConfigAndroidResourcePath);
                }
                return instanceAndroid;
            }
        }

        public static HiveConfigXML iOS{
            get{
                if (instanceiOS == null)
                {
                    instanceiOS = new HiveConfigXML(HiveConfigiOSResourcePath);
                }
                return instanceiOS;
            }
        }
        public static HiveConfigXML macOS
        {
            get
            {
                if (instancemacOS == null)
                {
                    instancemacOS = new HiveConfigXML(HiveConfigmacOSResourcePath);
                }
                return instancemacOS;
            }
        }
        public static HiveConfigXML Windows
        {
            get
            {
                if (instanceWidowsOS == null)
                {
                    instanceWidowsOS = new HiveConfigXML(HiveConfigWindowsResourcePath);
                }
                return instanceWidowsOS;
            }
        }
        private XmlDocument configxml = null;
        private XmlNode properties = null;
        private string xmlFilePath = string.Empty;

        private HiveConfigXML(string path)
        {
            string outputPath = Path.Combine("Assets", path);
            if( !File.Exists(outputPath) )
                Directory.CreateDirectory(outputPath);
            xmlFilePath = Path.Combine(outputPath, HiveConfigXMLName + HiveConfigXMLExtension);
            if( File.Exists(xmlFilePath) )
                load();
            else{
                //Create Default
                createDefault();
            }
        }

        public void load()
        {            
            configxml = new XmlDocument();
            configxml.Load(xmlFilePath);
            if (configxml == null)
            {
                Debug.LogWarning("Couldn't load " + xmlFilePath);
            }
            else
            {
                Debug.Log("Load : " + xmlFilePath);
                properties = FindChildNode(configxml, "properties");
                migrationXML();
            }
        }

        public void migrationXML() {
            // hive_config.xml 마이그레이션 - AuthV4
            {
                if ((FindChildNode (properties, "providers") == null) && useAuthv4) {

                    XmlNode authNode = configxml.CreateElement("providers");
                    // Facebook
                    {
                        XmlNode providerNode = configxml.CreateElement("facebook");
                        XmlAttribute id = configxml.CreateAttribute("id");

                        XmlNode appId = FindChildNode(properties, "facebookAppId");
                        if (appId != null) {  id.Value = appId.InnerXml; }

                        XmlNode clientToken = FindChildNode(properties, "facebookClientToken");
                        if (clientToken != null) { id.Value = clientToken.InnerXml; }

                        XmlNode permissions = FindChildNode(properties, "facebookPermissions");
                        if (permissions != null) {
                            providerNode.RemoveAll();

                            XmlNode newPermissionNode = configxml.CreateElement("permissions");
                            foreach (XmlNode childNode in permissions.ChildNodes) {
                                XmlNode permission = configxml.CreateElement("permission");
                                XmlAttribute nameNode = configxml.CreateAttribute("name");
                                nameNode.Value = childNode.InnerXml;
                                permission.Attributes.RemoveAll();
                                permission.Attributes.SetNamedItem(nameNode);
                                newPermissionNode.AppendChild(permission);
                            }
                            providerNode.AppendChild(newPermissionNode);
                        }

                        providerNode.Attributes.RemoveAll();
                        providerNode.Attributes.SetNamedItem(id);
                        authNode.AppendChild(providerNode);
                    }
                    // Google
                    {
                        XmlNode providerNode = configxml.CreateElement("google");
                        XmlAttribute playAppId = configxml.CreateAttribute("playAppId");
                        XmlAttribute clientId = configxml.CreateAttribute("clientId");
                        XmlAttribute serverClientId = configxml.CreateAttribute("serverClientId");
                        XmlAttribute reversedClientId = configxml.CreateAttribute("reversedClientId");

                        XmlNode playAppIdNode = FindChildNode(properties, "googlePlayAppId");
                        if (playAppIdNode != null) { playAppId.Value = playAppIdNode.InnerXml; }


                        XmlNode clientIdNode = FindChildNode(properties, "googleClientId");
                        if (clientIdNode != null) { clientId.Value = clientIdNode.InnerXml; }

                        XmlNode serverClientIdNode = FindChildNode(properties, "googleServerClientId");
                        if (serverClientIdNode != null) { serverClientId.Value = serverClientIdNode.InnerXml; }


                        XmlNode reversedClientIdNode = FindChildNode(properties, "googleReversedClientId");
                        if (reversedClientIdNode != null) { reversedClientId.Value = reversedClientIdNode.InnerXml; }

                        providerNode.Attributes.RemoveAll();
                        providerNode.Attributes.SetNamedItem(playAppId);
                        providerNode.Attributes.SetNamedItem(clientId);
                        providerNode.Attributes.SetNamedItem(serverClientId);
                        providerNode.Attributes.SetNamedItem(reversedClientId);
                        authNode.AppendChild(providerNode);
                    }

                    // QQ
                    {
                        XmlNode providerNode = configxml.CreateElement("qq");
                        XmlAttribute id = configxml.CreateAttribute("id");

                        XmlNode appId = FindChildNode(properties, "qqAppId");
                        if (appId != null) { id.Value = appId.InnerXml; }

                        providerNode.Attributes.RemoveAll();
                        providerNode.Attributes.SetNamedItem(id);
                        authNode.AppendChild(providerNode);
                    }
                    // VK
                    {
                        XmlNode providerNode = configxml.CreateElement("vk");
                        XmlAttribute id = configxml.CreateAttribute("id");

                        XmlNode appId = FindChildNode(properties, "vkAppId");
                        if (appId != null) { id.Value = appId.InnerXml; }

                        providerNode.Attributes.RemoveAll();
                        providerNode.Attributes.SetNamedItem(id);
                        authNode.AppendChild(providerNode);
                    }

                    // WeChat
                    {
                        XmlNode providerNode = configxml.CreateElement("wechat");
                        XmlAttribute id = configxml.CreateAttribute("id");
                        XmlAttribute secret = configxml.CreateAttribute("secret");
                        XmlAttribute paymentKey = configxml.CreateAttribute("paymentKey");

                        XmlNode appId = FindChildNode(properties, "wechatAppId");
                        if (appId != null) { id.Value = appId.InnerXml; }

                        XmlNode secretNode = FindChildNode(properties, "wechatAppSecret");
                        if (secretNode != null) { secret.Value = secretNode.InnerXml; }

                        XmlNode paymentKeyNode = FindChildNode(properties, "wechatPaymentKey");
                        if (paymentKeyNode != null) { paymentKey.Value = paymentKeyNode.InnerXml; }

                        providerNode.Attributes.RemoveAll();
                        providerNode.Attributes.SetNamedItem(id);
                        providerNode.Attributes.SetNamedItem(secret);
                        providerNode.Attributes.SetNamedItem(paymentKey);
                        authNode.AppendChild(providerNode);
                    }
                    properties.AppendChild(authNode);

                    commit();
                }
            }
            // hive_config.xml 마이그레이션 - tracker
            {
                XmlNode analyticsNode = FindChildNode (properties, "providers");
                if (analyticsNode == null) {
                    analyticsNode = configxml.CreateElement("providers");
                }

                XmlNode trackersNode = FindChildNode(properties, "trackers");

                if(trackersNode != null) {
                    foreach (XmlNode trackerNode in trackersNode.ChildNodes) {
                    var name = GetNamedItem(trackerNode.Attributes, "name", "");
                    if (String.IsNullOrEmpty(name)) {
                        continue;
                    }

                    XmlNode provider = configxml.CreateElement(name);
                    // provider.Attributes.set = trackerNode.Attributes;

                    provider.Attributes.RemoveAll();
                    foreach (XmlAttribute attribute in trackerNode.Attributes) {
                        //name항목을 엘리먼트 이름으로 쓰게 변경됨으로 이름항목은 스킵
                        if (attribute.Value == "name") {
                            continue;
                        }
                        provider.Attributes.SetNamedItem(attribute.CloneNode(true));
                    }

                    foreach (XmlNode childNode in trackerNode.ChildNodes) {
                        provider.AppendChild(childNode);
                    }
                    analyticsNode.AppendChild(provider);
                }
                properties.AppendChild(analyticsNode);
                properties.RemoveChild(trackersNode);
                commit();
                }

                
            }

            // hive_config.xml 마이그레이션 - appId
            if (FindChildNode (properties, "appId") == null) {
                XmlNode hiveAppIdNode = FindChildNode(properties, "HiveAppId");

                XmlNode appIdNode = configxml.CreateElement("appId");
                if (hiveAppIdNode != null)
                {
                    appIdNode.InnerXml = hiveAppIdNode.InnerXml;
                    properties.RemoveChild(hiveAppIdNode);
                }

                properties.AppendChild(appIdNode);
                
                commit();
            }

        }

        public void createDefault()
        {
            configxml = new XmlDocument();
            if (configxml == null)
            {
                Debug.LogError("Couldn't create hive_config.xml");
            }
            else{
                properties = configxml.CreateElement("properties");
                configxml.AppendChild(properties);
            }
        }

        public void commit()
        {
            if( configxml != null )
            {
                configxml.Save(xmlFilePath);
            }
        }

        private XmlNode FindChildNode(XmlNode parent, string name, string attributeName = null)
        {
            XmlNode curr = parent.FirstChild;
            while (curr != null)
            {
                if( attributeName != null )
                {
                    XmlAttributeCollection attributes = curr.Attributes;
                    if (curr.Name.Equals(name) &&
                        attributes != null &&
                        attributes.GetNamedItem("name") != null &&
                        attributes.GetNamedItem("name").Value != null &&
                        attributes.GetNamedItem("name").Value.Equals(attributeName) )
                    {
                        return curr;
                    }
                }
                else{
                    if (curr.Name.Equals(name))
                    {
                        return curr;
                    }
                }
                

                curr = curr.NextSibling;
            }

            return null;
        }

        private static string PROVIDERS_KEY="providers";

        //프로바이더 노드 얻기
        private XmlNode GetProviderNode(string provider) {
            var providers = FindChildNode(properties, PROVIDERS_KEY);
            XmlNode providerNode = null;
            if ( providers != null ) {
                providerNode = FindChildNode(providers, provider);
            }
            return providerNode;
        }

        //프로바이더의 어트리뷰트 얻기.
        private string GetProviderValue(string provider, string attributeName) {
            var providerNode = GetProviderNode(provider);
            if (providerNode != null) {
                return GetNamedItem(providerNode.Attributes,attributeName,"");
            }
            return "";
        }

        private void SetProviderValue(string provider, string attributeName, string value) {
            var providers = FindChildNode(properties, PROVIDERS_KEY);
            if ( providers == null ) {
                providers = configxml.CreateElement(PROVIDERS_KEY);
                properties.AppendChild(providers);
            }

            var providerNode = FindChildNode(providers, provider);
            if (providerNode == null) {
                providerNode = configxml.CreateElement(provider);
                providers.AppendChild(providerNode);
            }

            var item = providerNode.Attributes.GetNamedItem(attributeName);
            if (item == null) {
                Debug.Log(provider +" : "+value);
                var v = CreateAttribute(configxml, attributeName, value);
                providerNode.Attributes.SetNamedItem(v);
            } else {
                item.Value = value;
            }
        }

		// Added in HIVE SDK 4.5.0

		public Boolean useAuthv1{
			get{
				XmlNode node = FindChildNode(properties, "useAuthv1");
				if( node != null && node.InnerXml != null )
					return Boolean.Parse(node.InnerXml);
				else
					return false;
			}
			set{
				XmlNode node = FindChildNode(properties, "useAuthv1");
				if( node == null )
				{
					node = configxml.CreateElement("useAuthv1");
					properties.AppendChild(node);
				}
				node.InnerXml = value ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();

				// set auth v4 as opposit value of auth v1
				if (useAuthv1 == useAuthv4) {
					useAuthv4 = !useAuthv1;
				}
			}
		}

		// Added in HIVE SDK 4.5.0

		public Boolean useAuthv4{
			get{
				XmlNode node = FindChildNode(properties, "useAuthv4");
				if( node != null && node.InnerXml != null )
					return Boolean.Parse(node.InnerXml);
				else
					return true;
			}
			set{
				XmlNode node = FindChildNode(properties, "useAuthv4");
				if( node == null )
				{
					node = configxml.CreateElement("useAuthv4");
					properties.AppendChild(node);
				}
				node.InnerXml = value ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
				// set auth v1 as opposit value of auth v4
				if (useAuthv1 == useAuthv4) {
					useAuthv1 = !useAuthv4;
				}
			}
		}

        // Added in HIVE SDK 4.5.0
		public String HIVEAppID {
			get {
				XmlNode node = FindChildNode (properties, "appId");
				String reAppID = "";
				if (node != null && node.InnerXml != null && node.InnerXml != "") {
					reAppID = node.InnerXml;
				} else {
					BuildTarget nowBuildTarget = EditorUserBuildSettings.activeBuildTarget;
					if ((this == instanceiOS) && (nowBuildTarget == BuildTarget.iOS)) {
                        #if UNITY_2018_4_OR_NEWER
						reAppID = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
                        #elif UNITY_5_6
                        reAppID = PlayerSettings.applicationIdentifier;
                        #else
                        reAppID = PlayerSettings.applicationIdentifier;
                        #endif
					} else if ((this == instanceAndroid) && (nowBuildTarget == BuildTarget.Android)) {
                        #if UNITY_2018_4_OR_NEWER
						reAppID = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
                        #elif UNITY_5_6
                        reAppID = PlayerSettings.applicationIdentifier;
                        #else
                        reAppID = PlayerSettings.applicationIdentifier;
                        #endif
					}
					if (reAppID == "com.Company.ProductName") {
						reAppID = "";
					}
				}
				return reAppID;
			}
			set {
				XmlNode node = FindChildNode (properties, "appId");
				if (node == null) {
					node = configxml.CreateElement ("appId");
					properties.AppendChild (node);
				}
				node.InnerXml = value.ToString ();
			}
		}

		// Added in HIVE SDK 4.5.0
		public String facebookAppID {
			// TODO
			get {
                if (useAuthv1) {
                    XmlNode node = FindChildNode (properties, "facebookAppId");
                    if (node != null && node.InnerXml != null) {
                        return node.InnerXml;
                    } else {
                        return "";
                    }
                } else {
                    return GetProviderValue("facebook","id");
                }
			}
			set {
				XmlNode node = FindChildNode (properties, "facebookAppId");
				if (node == null) {
					node = configxml.CreateElement ("facebookAppId");
					properties.AppendChild (node);
				}
				node.InnerXml = value.ToString ();
                SetProviderValue("facebook","id",value);
			}
		}

        // Added in HIVE SDK 4.15.8.1
        public String facebookClientToken {
            get {
                if (useAuthv1) {
                    XmlNode node = FindChildNode(properties, "facebookClientToken");
                    if (node != null && node.InnerXml != null) {
                        return node.InnerXml;
                    } else {
                        return "";
                    }
                } else {
                    return GetProviderValue("facebook", "clientToken");
                }
            }
            set {
                XmlNode node = FindChildNode(properties, "facebookClientToken");
                if (node == null) {
                    node = configxml.CreateElement("facebookClientToken");
                    properties.AppendChild(node);
                }
                node.InnerXml = value.ToString();
                SetProviderValue("facebook", "clientToken", value);
            }
        }

        public HiveConfigEditor.FacebookPermissions facebookPermissions
        {
            get
            {
                HiveConfigEditor.FacebookPermissions permissions = new HiveConfigEditor.FacebookPermissions();
                
                XmlNode providersNode = FindChildNode(properties, PROVIDERS_KEY);
                if(providersNode != null){
                    XmlNode facebookNode = FindChildNode(providersNode,"facebook");
                    
                    if(facebookNode != null){
                        XmlNode permissionsNode = FindChildNode(facebookNode,"permissions");

                        if(permissionsNode != null && permissionsNode.HasChildNodes){
                            permissions.permissions = new List<string>();

                            XmlNode permissionNode = permissionsNode.FirstChild;
                            while(permissionNode != null)
                            {
                                if(permissionNode.Name.Equals("permission"))
                                {
                                    XmlAttribute nameAttr = (XmlAttribute)permissionNode.Attributes.GetNamedItem("name");
                                    if(nameAttr != null && nameAttr.Value != null)
                                    {
                                        permissions.permissions.Add(nameAttr.Value);
                                    }
                                }

                                permissionNode = permissionNode.NextSibling;
                            }
                        }
                    }
                }
                
                return permissions;
            }
            set
            {
                XmlNode providersNode = FindChildNode(properties, PROVIDERS_KEY);
                if(providersNode == null){
                    providersNode = configxml.CreateElement(PROVIDERS_KEY);
                    properties.AppendChild(providersNode);
                }

                XmlNode facebookNode = FindChildNode(providersNode,"facebook");
                if(facebookNode == null){
                    facebookNode = configxml.CreateElement("facebook");
                    properties.AppendChild(facebookNode);
                }

                if(value == null)
                {
                    providersNode.RemoveChild(facebookNode);
                }
                else
                {
                    if(value.permissions != null)
                    {
                        XmlNode permissionsNode = FindChildNode(facebookNode, "permissions");
                        if(permissionsNode == null)
                        {
                            permissionsNode = configxml.CreateElement("permissions");
                            facebookNode.AppendChild(permissionsNode);
                        }

                        if(permissionsNode.HasChildNodes)
                            permissionsNode.RemoveAll();

                        foreach(string permission in value.permissions)
                        {
                            XmlNode permissionNode = configxml.CreateElement("permission");
                            
                            XmlAttribute nameAttr = configxml.CreateAttribute("name");
                            nameAttr.Value = permission;

                            permissionNode.Attributes.SetNamedItem(nameAttr);
                            
                            permissionsNode.AppendChild(permissionNode);
                        }
                    }
                }
            
            }
        }

		// Added in HIVE SDK 4.5.0
		public String googlePlayAppID {
			get {
                if (useAuthv1) {
                    XmlNode node = FindChildNode (properties, "googlePlayAppId");
                    if (node != null && node.InnerXml != null) {
                        return node.InnerXml;
                    } else {
                        return "";
                    }
                } else {
                    return GetProviderValue("google","playAppId");
                }
			}
			set {
				XmlNode node = FindChildNode (properties, "googlePlayAppId");
				if (node == null) {
					node = configxml.CreateElement ("googlePlayAppId");
					properties.AppendChild (node);
				}
				node.InnerXml = value.ToString ();
                SetProviderValue("google","playAppId",value);
			}
		}

		// Added in HIVE SDK 4.5.0
		public String googleServerClientID {
			get {
                if (useAuthv1) {
                    XmlNode node = FindChildNode (properties, "googleServerClientId");
                    if (node != null && node.InnerXml != null) {
                        return node.InnerXml;
                    } else {
                        return "";
                    }
                } else {
                    return GetProviderValue("google","serverClientId");
                }
			}
			set {
				XmlNode node = FindChildNode (properties, "googleServerClientId");
				if (node == null) {
					node = configxml.CreateElement ("googleServerClientId");
					properties.AppendChild (node);
				}
				node.InnerXml = value.ToString ();
                SetProviderValue("google","serverClientId",value);
			}
		}

        // Added in HIVE SDK 4.21.0.0
        public String googlePlayGamesAppID
        {
            get
            {
                if (useAuthv1)
                {
                    XmlNode node = FindChildNode(properties, "googlePlayGamesAppId");
                    if (node != null && node.InnerXml != null)
                    {
                        return node.InnerXml;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return GetProviderValue("googleplaygames", "playAppId");
                }
            }
            set
            {
                XmlNode node = FindChildNode(properties, "googlePlayGamesAppId");
                if (node == null)
                {
                    node = configxml.CreateElement("googlePlayGamesAppId");
                    properties.AppendChild(node);
                }
                node.InnerXml = value.ToString();
                SetProviderValue("googleplaygames", "playAppId", value);
            }
        }

        // Added in HIVE SDK 4.21.0.0
        public String googlePlayGamesServerClientID
        {
            get
            {
                if (useAuthv1)
                {
                    XmlNode node = FindChildNode(properties, "googlePlayGamesServerClientId");
                    if (node != null && node.InnerXml != null)
                    {
                        return node.InnerXml;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return GetProviderValue("googleplaygames", "serverClientId");
                }
            }
            set
            {
                XmlNode node = FindChildNode(properties, "googlePlayGamesServerClientId");
                if (node == null)
                {
                    node = configxml.CreateElement("googlePlayGamesServerClientId");
                    properties.AppendChild(node);
                }
                node.InnerXml = value.ToString();
                SetProviderValue("googleplaygames", "serverClientId", value);
            }
        }

        // Added in HIVE SDK 4.5.0
        public String googleClientID {
			get {
                if (useAuthv1) {
                    XmlNode node = FindChildNode (properties, "googleClientId");
                    if (node != null && node.InnerXml != null) {
                        return node.InnerXml;
                    } else {
                        return "";
                    }
                } else {
                    return GetProviderValue("google","clientId");
                }
			}
			set {
				XmlNode node = FindChildNode (properties, "googleClientId");
				if (node == null) {
					node = configxml.CreateElement ("googleClientId");
					properties.AppendChild (node);
				}
				node.InnerXml = value.ToString ();
                SetProviderValue("google","clientId",value);
			}
		}
		public String googleReversedClientID {
			get {
                if (useAuthv1) {
                    XmlNode node = FindChildNode (properties, "googleReversedClientId");
                    if (node != null && node.InnerXml != null) {
                        return node.InnerXml;
                    } else {
                        return "";
                    }
                } else {
                    return GetProviderValue("google","reversedClientId");
                }
			}
			set {
				XmlNode node = FindChildNode (properties, "googleReversedClientId");
				if (node == null) {
					node = configxml.CreateElement ("googleReversedClientId");
					properties.AppendChild (node);
				}
				node.InnerXml = value.ToString ();
                SetProviderValue("google","reversedClientId",value);
			}
		}

		// Added in HIVE SDK 4.5.0
		public String qqAppId {
			get {
                if (useAuthv1) {
                    XmlNode node = FindChildNode (properties, "qqAppId");
                    if (node != null && node.InnerXml != null) {
                        return node.InnerXml;
                    } else {
                        return "";
                    }
                } else {
                    return GetProviderValue("qq","id");
                }
			}
			set {
				XmlNode node = FindChildNode (properties, "qqAppId");
				if (node == null) {
					node = configxml.CreateElement ("qqAppId");
					properties.AppendChild (node);
				}
				node.InnerXml = value.ToString ();
                SetProviderValue("qq","id",value);
			}
		}

        public String vkAppId { 
            get {
                if (useAuthv1) {
                    XmlNode node = FindChildNode (properties, "vkAppId");
                    if (node != null && node.InnerXml != null) {
                        return node.InnerXml;
                    } else {
                        return "";
                    }
                } else {
                    return GetProviderValue("vk","id");
                }
            }
            set {
                XmlNode node = FindChildNode (properties, "vkAppId");
                if (node == null) {
                    node = configxml.CreateElement ("vkAppId");
                    properties.AppendChild (node);
                }
                node.InnerXml = value.ToString();
                SetProviderValue("vk","id",value);
            }
        }

        public String weChatAppId {
            get {
                if (useAuthv1) {
                    XmlNode node = FindChildNode (properties, "wechatAppId");
                    if (node != null && node.InnerXml != null) {
                        return node.InnerXml;
                    } else {
                        return "";
                    }
                } else {
                    return GetProviderValue("wechat","id");
                }
            }
            set {
                XmlNode node = FindChildNode (properties, "wechatAppId");
                if (node == null) {
                    node = configxml.CreateElement("wechatAppId");
                    properties.AppendChild (node);
                }
                node.InnerXml = value.ToString();
                SetProviderValue("wechat","id",value);
            }
        }

        public String weChatAppSecret {
            get {
                if (useAuthv1) {
                    XmlNode node = FindChildNode (properties, "wechatAppSecret");
                    if (node != null && node.InnerXml != null) {
                        return node.InnerXml;
                    } else {
                        return "";
                    }
                } else {
                    return GetProviderValue("wechat","secret");
                }
            }
            set {
                XmlNode node = FindChildNode (properties, "wechatAppSecret");
                if (node == null) {
                    node = configxml.CreateElement("wechatAppSecret");
                    properties.AppendChild (node);
                }
                node.InnerXml = value.ToString();
                SetProviderValue("wechat","secret",value);
            }
        }

        public String universalLink {
            get {
                XmlNode node = FindChildNode (properties, "universalLink");
                if (node != null && node.InnerXml != null) {
                    return node.InnerXml;
                } else {
                    return "";
                }
            }
            set {
                XmlNode node = FindChildNode (properties, "universalLink");
                if (node == null) {
                    node = configxml.CreateElement("universalLink");
                    properties.AppendChild (node);
                }
                node.InnerXml = value.ToString();
            }
        }

        public String weChatPaymentKey {
            get {
                if (useAuthv1) { 
                    XmlNode node = FindChildNode (properties, "wechatPaymentKey");
                    if (node != null && node.InnerXml != null) {
                       return node.InnerXml;
                    } else {
                        return "";
                    }
                } else {
                    return GetProviderValue("wechat","paymentKey");
                }
            }
            set {
                XmlNode node = FindChildNode (properties, "wechatPaymentKey");
                if (node == null) {
                    node = configxml.CreateElement("wechatPaymentKey");
                    properties.AppendChild (node);
                }
                node.InnerXml = value.ToString();
                SetProviderValue("wechat","paymentKey",value);
            }
        }

         public String lineChannelId {
            get {
                return GetProviderValue("line","channelId");
            }
            set {
                SetProviderValue("line","channelId",value);
            }
        }

        // Added in HIVE SDK 4.15.2
        public String weverseClientId {
            get {
                return GetProviderValue("weverse","clientId");
            }
            set {
                SetProviderValue("weverse","clientId",value);
            }
        }

        // Added in HIVE SDK 4.15.6
        public String signInWithAppleServiceId {
            get {
                return GetProviderValue("signinwithapple","serviceid");
            }
            set {
                SetProviderValue("signinwithapple","serviceid",value);
            }
        }

        public const bool USE_HIVE_UI = false;
        public const bool USE_CUSTOM_UI = true;

        public bool authV4Helper_signIn {
            get {
            XmlNode node = FindChildNode(properties, "useCustomUI");
            if (node!= null && node.InnerXml != null) {
                XmlNode subNode = FindChildNode(node, "signIn");
                if (subNode != null && subNode.InnerXml != null) {
                    return Boolean.Parse(subNode.InnerXml);
                }
            }
                return USE_HIVE_UI;

            } set {
                XmlNode node = FindChildNode(properties, "useCustomUI");
                if (node == null) {
                    node = configxml.CreateElement("useCustomUI");
                    properties.AppendChild(node);
                }

                XmlNode subNode = FindChildNode(node, "signIn");
                if (subNode == null) {
                    subNode = configxml.CreateElement("signIn");
                    node.AppendChild(subNode);
                }

                subNode.InnerXml = value ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
            }
        }

        public bool authV4Helper_connect {
            get {
            XmlNode node = FindChildNode(properties, "useCustomUI");
            if (node!= null && node.InnerXml != null) {
                XmlNode subNode = FindChildNode(node, "connect");
                if (subNode != null && subNode.InnerXml != null) {
                    return Boolean.Parse(subNode.InnerXml);
                }
            }
                return USE_HIVE_UI;

            } set {
                XmlNode node = FindChildNode(properties, "useCustomUI");
                if (node == null) {
                    node = configxml.CreateElement("useCustomUI");
                    properties.AppendChild(node);
                }

                XmlNode subNode = FindChildNode(node, "connect");
                if (subNode == null) {
                    subNode = configxml.CreateElement("connect");
                    node.AppendChild(subNode);
                }
                
                subNode.InnerXml = value ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
            }
        }

        public bool authV4Helper_achievement {
            get {
            XmlNode node = FindChildNode(properties, "useCustomUI");
            if (node!= null && node.InnerXml != null) {
                XmlNode subNode = FindChildNode(node, "achievement");
                if (subNode != null && subNode.InnerXml != null) {
                    return Boolean.Parse(subNode.InnerXml);
                }
            }
                return USE_HIVE_UI;

            } set {
                XmlNode node = FindChildNode(properties, "useCustomUI");
                if (node == null) {
                    node = configxml.CreateElement("useCustomUI");
                    properties.AppendChild(node);
                }

                XmlNode subNode = FindChildNode(node, "achievement");
                if (subNode == null) {
                    subNode = configxml.CreateElement("achievement");
                    node.AppendChild(subNode);
                }
                
                subNode.InnerXml = value ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
            }
        }

        public bool authV4Helper_syncAccount {
            get {
            XmlNode node = FindChildNode(properties, "useCustomUI");
            if (node!= null && node.InnerXml != null) {
                XmlNode subNode = FindChildNode(node, "syncAccount");
                if (subNode != null && subNode.InnerXml != null) {
                    return Boolean.Parse(subNode.InnerXml);
                }
            }
                return USE_HIVE_UI;

            } set {
                XmlNode node = FindChildNode(properties, "useCustomUI");
                if (node == null) {
                    node = configxml.CreateElement("useCustomUI");
                    properties.AppendChild(node);
                }

                XmlNode subNode = FindChildNode(node, "syncAccount");
                if (subNode == null) {
                    subNode = configxml.CreateElement("syncAccount");
                    node.AppendChild(subNode);
                }
                
                subNode.InnerXml = value ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
            }
        }

        public enum ZoneType{
            [Description("Sandbox")]
            sandbox,
            [Description("Real")]
            real
        }

        public ZoneType zone{
            get{
				XmlNode node = FindChildNode(properties, "zone");
				if( node != null && node.InnerXml != null )
                    return (ZoneType)Enum.Parse(typeof(ZoneType),node.InnerXml);
                else
                    return ZoneType.sandbox;
            }
            set{
				XmlNode node = FindChildNode(properties, "zone");
                if( node == null )
				{
                    node = configxml.CreateElement("zone");
                    properties.AppendChild(node);
                }
                node.InnerXml = value.ToString();
            }
        }

        public Boolean hivePermissionViewOn {
             get{
                XmlNode node = FindChildNode(properties, "hivePermissionViewOn");
                if( node != null && node.InnerXml != null )
                    return Boolean.Parse(node.InnerXml);
                else
                    return true;
            }
            set{
                XmlNode node = FindChildNode(properties, "hivePermissionViewOn");
                if( node == null )
                {
                    node = configxml.CreateElement("hivePermissionViewOn");
                    properties.AppendChild(node);
                }
                node.InnerXml = value ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
            }
        }

        public Boolean useLog{
            get{
                XmlNode node = FindChildNode(properties, "useLog");
                if( node != null && node.InnerXml != null )
                    return Boolean.Parse(node.InnerXml);
                else
                    return false;
            }
            set{
                XmlNode node = FindChildNode(properties, "useLog");
                if( node == null )
                {
                    node = configxml.CreateElement("useLog");
                    properties.AppendChild(node);
                }
                node.InnerXml = value ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
            }
        }

        public Boolean ageGateU13 {
            get{
                XmlNode node = FindChildNode(properties, "ageGateU13");
                if( node != null && node.InnerXml != null )
                    return Boolean.Parse(node.InnerXml);
                else
                    return false;
            }
            set{
                XmlNode node = FindChildNode(properties, "ageGateU13");
                if( node == null )
                {
                    node = configxml.CreateElement("ageGateU13");
                    properties.AppendChild(node);
                }
                node.InnerXml = value ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
            }
        }

        //sdwrite 만 처리.
        public Boolean sdwritePermission{
            get{
                XmlNode permissionsNode = FindChildNode(properties, "permissions");
                if( permissionsNode != null)
                {
                    XmlNode node = FindChildNode(permissionsNode,"sdwrite");
                    if( node != null && node.InnerXml != null )
                        return Boolean.Parse(node.InnerXml);
                }
                #if UNITY_EDITOR
                return PlayerSettings.Android.forceSDCardPermission;
                #else
                return false;
                #endif
            }
            set{
                XmlNode permissionsNode = FindChildNode(properties, "permissions");
                if( permissionsNode == null )
                {
                    permissionsNode = configxml.CreateElement("permissions");
                    properties.AppendChild(permissionsNode);
                }
                XmlNode sdwriteNode = FindChildNode(permissionsNode,"sdwrite");
                if( sdwriteNode == null )
                {
                    sdwriteNode = configxml.CreateElement("sdwrite");
                    permissionsNode.AppendChild(sdwriteNode);
                }
                sdwriteNode.InnerXml = value ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
                #if UNITY_EDITOR
                PlayerSettings.Android.forceSDCardPermission = value;
                #endif
            }
        }

        public enum MarketType{
            [Description("Google Play")]
            GO, // 구글플레이
            [Description("Com2us Lebi")]
            LE, // 러비
            [Description("Apple Appstore")]
            AP, // 애플스토어
            
        }
        public MarketType market{
            get{
                XmlNode node = FindChildNode(properties, "market");
                if( node != null && node.InnerXml != null )
                    return (MarketType)Enum.Parse(typeof(MarketType),node.InnerXml);
                else
                {
                    if( instanceAndroid == this )
                        return MarketType.GO;
                    else if( instanceiOS == this )
                        return MarketType.AP;
                    else
                        return MarketType.GO;
                }
            }
            set{
                XmlNode node = FindChildNode(properties, "market");
                if( node == null )
                {
                    node = configxml.CreateElement("market");
                    properties.AppendChild(node);
                }
                node.InnerXml = value.ToString();
            }
        }

        public enum HiveOrientationType {
            [Description("UNDEFINED")]
            undefined,
            [Description("ALL")]
            all,
            [Description("PORTRAIT")]
            portrait,
            [Description("LANDSCAPE")]
            landscape,
        }

        public HiveOrientationType hiveOrientation {
            get {
                XmlNode node = FindChildNode(properties, "hiveOrientation");
                if( node != null && node.InnerXml != null )
                    return (HiveOrientationType)Enum.Parse(typeof(HiveOrientationType),node.InnerXml);
                else
                    return HiveOrientationType.undefined;
            }
            set {
                XmlNode node = FindChildNode(properties, "hiveOrientation");
                if( node == null )
				{
                    node = configxml.CreateElement("hiveOrientation");
                    properties.AppendChild(node);
                }
                node.InnerXml = value.ToString();
            }
        }

        public int httpConnectTimeout{
            get{
                XmlNode node = FindChildNode(properties, "httpConnectTimeout");
                if( node != null && node.InnerXml != null )
                    return int.Parse(node.InnerXml);
                else
                {
                    return 8;//Default
                }
            }
            set{
                XmlNode node = FindChildNode(properties, "httpConnectTimeout");
                if( node == null )
                {
                    node = configxml.CreateElement("httpConnectTimeout");
                    properties.AppendChild(node);
                }
                node.InnerXml = value.ToString();
            }
        }

        public int httpReadTimeout{
            get{
                XmlNode node = FindChildNode(properties, "httpReadTimeout");
                if( node != null && node.InnerXml != null )
                    return int.Parse(node.InnerXml);
                else
                {
                    return 8;//Default
                }
            }
            set{
                XmlNode node = FindChildNode(properties, "httpReadTimeout");
                if( node == null )
                {
                    node = configxml.CreateElement("httpReadTimeout");
                    properties.AppendChild(node);
                }
                node.InnerXml = value.ToString();
            }
        }

        public int maxGameLogSize{
            get{
                XmlNode node = FindChildNode(properties, "maxGameLogSize");
                if( node != null && node.InnerXml != null )
                    return int.Parse(node.InnerXml);
                else
                {
                    return 50;//Default
                }
            }
            set{
                XmlNode node = FindChildNode(properties, "maxGameLogSize");
                if( node == null )
                {
                    node = configxml.CreateElement("maxGameLogSize");
                    properties.AppendChild(node);
                }
                node.InnerXml = value.ToString();
            }
        }

        public uint analyticsSendLimit {
            get{
                XmlNode node = FindChildNode(properties, "analyticsSendLimit");
                if( node != null && node.InnerXml != null )
                    return uint.Parse(node.InnerXml);
                else
                {
                    return 5;//Default
                }
            }
            set{
                XmlNode node = FindChildNode(properties, "analyticsSendLimit");
                if( node == null )
                {
                    node = configxml.CreateElement("analyticsSendLimit");
                    properties.AppendChild(node);
                }
                node.InnerXml = value.ToString();
            }
        }

        public uint analyticsQueueLimit {
            get{
                XmlNode node = FindChildNode(properties, "analyticsQueueLimit");
                if( node != null && node.InnerXml != null )
                    return uint.Parse(node.InnerXml);
                else
                {
                    return 50;//Default
                }
            }
            set{
                XmlNode node = FindChildNode(properties, "analyticsQueueLimit");
                if( node == null )
                {
                    node = configxml.CreateElement("analyticsQueueLimit");
                    properties.AppendChild(node);
                }
                node.InnerXml = value.ToString();
            }
        }

        public float analyticsSendCycle {
            get{
                XmlNode node = FindChildNode(properties, "analyticsSendCycle");
                if( node != null && node.InnerXml != null )
                    return float.Parse(node.InnerXml);
                else
                {
                    return 1;//Default
                }
            }
            set{
                XmlNode node = FindChildNode(properties, "analyticsSendCycle");
                if( node == null )
                {
                    node = configxml.CreateElement("analyticsSendCycle");
                    properties.AppendChild(node);
                }
                node.InnerXml = value.ToString();
            }
        }

        public Boolean useCrashReport{
            get{
                XmlNode node = FindChildNode(properties, "useCrashReport");
                if( node != null && node.InnerXml != null )
                    return Boolean.Parse(node.InnerXml);
                else
                    return true;
            }
            set{
                XmlNode node = FindChildNode(properties, "useCrashReport");
                if( node == null )
                {
                    node = configxml.CreateElement("useCrashReport");
                    properties.AppendChild(node);
                }
                node.InnerXml = value ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower();
            }
        }
        
        private void setupEvent(HiveConfigEditor.Tracker tracker, XmlNode trackerNode) {
            if( tracker != null && trackerNode != null )
            {
                XmlAttributeCollection trackerAttributes = trackerNode.Attributes;;

                XmlNode eventsNode = FindChildNode( trackerNode, "events");
                if( eventsNode != null && eventsNode.HasChildNodes )
                {
                    tracker.eventName = new List<string>();
                    tracker.eventValue = new List<string>();

                    XmlNode eventNode = eventsNode.FirstChild;
                    while (eventNode != null)
                    {
                        if (eventNode.Name.Equals("event") )
                        {
                            XmlAttributeCollection attributes = eventNode.Attributes;
                            XmlAttribute eventName = (XmlAttribute)attributes.GetNamedItem("name");
                            XmlAttribute eventValue = (XmlAttribute)attributes.GetNamedItem("value");

                            if( eventName != null && eventName.Value != null && eventValue != null && eventValue.Value != null )
                            {
                                tracker.eventName.Add(eventName.Value);
                                tracker.eventValue.Add(eventValue.Value);
                            }
                                
                        }
                        eventNode = eventNode.NextSibling;
                    }
                }
            }
        }

        private XmlNode setTrackerToXml(String trackerName, HiveConfigEditor.Tracker tracker) {
            XmlNode trackersNode = FindChildNode(properties, PROVIDERS_KEY);
            if ( trackersNode == null) {
                trackersNode = configxml.CreateElement(PROVIDERS_KEY);
                properties.AppendChild(trackersNode);
            }

            XmlNode trackerNode = FindChildNode(trackersNode, trackerName);
            if (trackerNode == null) {
                trackerNode = configxml.CreateElement(trackerName);
                trackersNode.AppendChild(trackerNode);
            }

            if (tracker == null) {
                trackersNode.RemoveChild(trackerNode);
            } else {
                if (tracker != null) {
                    if (tracker.eventName != null && tracker.eventName.Count > 0 ) {
                        XmlNode eventsNode = FindChildNode(trackerNode, "events");
                        if( eventsNode == null )
                        {
                            eventsNode = configxml.CreateElement("events");
                            trackerNode.AppendChild(eventsNode);
                        }
                        
                        if( eventsNode.HasChildNodes )
                            eventsNode.RemoveAll();

                        for( int i = 0 ; i < tracker.eventName.Count ; i++)
                        {
                            XmlAttribute eventName = configxml.CreateAttribute("name");
                            XmlAttribute eventValue = configxml.CreateAttribute("value");
                            eventName.Value = tracker.eventName[i];
                            eventValue.Value = tracker.eventValue[i];
                            XmlNode eventNode = configxml.CreateElement("event");
                            eventNode.Attributes.SetNamedItem(eventName);
                            eventNode.Attributes.SetNamedItem(eventValue);
                            eventsNode.AppendChild(eventNode);
                        }
                    }
                }
            }
            return trackerNode;
        }

        private XmlAttribute CreateAttribute(XmlDocument doc,string name, string defaultValue) {
            XmlAttribute attr = doc.CreateAttribute(name);
            attr.Value = defaultValue;
            return attr;
        }

        private String GetNamedItem(XmlNamedNodeMap node,string name, string defaultValue) {
            if (node == null) {
                return "";
            }
            XmlAttribute attr = (XmlAttribute)node.GetNamedItem(name);
            return attr != null ? attr.Value : "";
        }

        public HiveConfigEditor.Tracker Adjust{
            get{
                HiveConfigEditor.AdjustTracker tracker = new HiveConfigEditor.AdjustTracker();
                tracker.name = "Adjust";

                XmlNode trackersNode = FindChildNode(properties, PROVIDERS_KEY);
                if( trackersNode != null)
                {
                    XmlNode trackerNode = FindChildNode(trackersNode, "Adjust");
                    if( trackerNode != null )
                    {
                        XmlAttributeCollection trackerAttributes = trackerNode.Attributes;
                        // XmlAttribute id = (XmlAttribute)trackerAttributes.GetNamedItem("id");
                        tracker.key = GetNamedItem(trackerAttributes, "key", "");
                        tracker.secretId = GetNamedItem(trackerAttributes, "secretId", "");
                        tracker.secretInfo1 = GetNamedItem(trackerAttributes, "info1", "");
                        tracker.secretInfo2 = GetNamedItem(trackerAttributes, "info2", "");
                        tracker.secretInfo3 = GetNamedItem(trackerAttributes, "info3", "");
                        tracker.secretInfo4 = GetNamedItem(trackerAttributes, "info4", "");
                        setupEvent(tracker, trackerNode);
                        return tracker;
                    }
                }
                tracker.AddEvent("Purchase","");
                tracker.AddEvent("TutorialComplete","");
                tracker.AddEvent("Update","");
                return tracker;
            }
            set{
                var trackerNode = setTrackerToXml("Adjust",value);
                var tracker = value as HiveConfigEditor.AdjustTracker;
                if ( trackerNode != null && tracker != null && tracker.key != null ) {
                    XmlAttribute trackerName = CreateAttribute(configxml,"name",tracker.name);
                    XmlAttribute trackerId = CreateAttribute(configxml,"id","unused");
                    XmlAttribute trackerKey = CreateAttribute(configxml,"key",tracker.key);
                    XmlAttribute trackerSecretId = CreateAttribute(configxml,"secretId",tracker.secretId);
                    XmlAttribute trackerSecretInfo1 = CreateAttribute(configxml,"info1",tracker.secretInfo1);
                    XmlAttribute trackerSecretInfo2 = CreateAttribute(configxml,"info2",tracker.secretInfo2);
                    XmlAttribute trackerSecretInfo3 = CreateAttribute(configxml,"info3",tracker.secretInfo3);
                    XmlAttribute trackerSecretInfo4 = CreateAttribute(configxml,"info4",tracker.secretInfo4);

                    trackerNode.Attributes.RemoveAll();
                    trackerNode.Attributes.SetNamedItem(trackerName);
                    trackerNode.Attributes.SetNamedItem(trackerId);
                    trackerNode.Attributes.SetNamedItem(trackerKey);
                    trackerNode.Attributes.SetNamedItem(trackerSecretId);
                    trackerNode.Attributes.SetNamedItem(trackerSecretInfo1);
                    trackerNode.Attributes.SetNamedItem(trackerSecretInfo2);
                    trackerNode.Attributes.SetNamedItem(trackerSecretInfo3);
                    trackerNode.Attributes.SetNamedItem(trackerSecretInfo4);
                }
            }
        }

        public HiveConfigEditor.Tracker Singular {
            get {
                HiveConfigEditor.SingularTracker tracker = new HiveConfigEditor.SingularTracker();
                tracker.name = "Singular";

                XmlNode trackersNode = FindChildNode(properties, PROVIDERS_KEY);
                if (trackersNode != null) {
                    XmlNode trackerNode = FindChildNode(trackersNode, "Singular");
                    if (trackerNode != null) {
                        XmlAttributeCollection trackerAttributes = trackerNode.Attributes;
                        tracker.id = GetNamedItem(trackerAttributes,"id","");
                        tracker.key = GetNamedItem(trackerAttributes,"key","");
                        setupEvent(tracker, trackerNode);
                        return tracker;
                    }
                }
                tracker.AddEvent("Purchase","");
                tracker.AddEvent("TutorialComplete","");
                tracker.AddEvent("Update","");
                return tracker;
            }
            set {
                var trackerNode = setTrackerToXml("Singular", value);
                var tracker = value as HiveConfigEditor.SingularTracker;
                if ( trackerNode != null && tracker != null && tracker.key != null ) {
                    XmlAttribute trackerName = CreateAttribute(configxml ,"name",tracker.name);
                    XmlAttribute trackerId = CreateAttribute(configxml ,"id",tracker.id);
                    XmlAttribute trackerKey = CreateAttribute(configxml ,"key",tracker.key);

                    trackerNode.Attributes.RemoveAll();
                    trackerNode.Attributes.SetNamedItem(trackerName);
                    trackerNode.Attributes.SetNamedItem(trackerId);
                    trackerNode.Attributes.SetNamedItem(trackerKey);    
                }
            }
        }

        public HiveConfigEditor.Tracker AppsFlyer {
            get {
                HiveConfigEditor.AppsFlyerTracker tracker = new HiveConfigEditor.AppsFlyerTracker();
                tracker.name = "AppsFlyer";

                XmlNode trackersNode = FindChildNode(properties, PROVIDERS_KEY);
                if (trackersNode != null) {
                    XmlNode trackerNode = FindChildNode(trackersNode, "AppsFlyer");
                    if (trackerNode != null) {
                        XmlAttributeCollection trackerAttributes = trackerNode.Attributes;
                        tracker.id = GetNamedItem(trackerAttributes,"id","");
                        tracker.key = GetNamedItem(trackerAttributes,"key","");
                        tracker.itunesConnectAppId = GetNamedItem(trackerAttributes,"itunesConnectAppId","");
                        if (tracker.itunesConnectAppId == "") {
                            tracker.itunesConnectAppId = GetNamedItem(trackerAttributes,"itunseConnectAppId","");
                        }
                        
                        setupEvent(tracker, trackerNode);
                        return tracker;
                    }
                }
                tracker.AddEvent("TutorialComplete","");
                tracker.AddEvent("Update","");
                return tracker;
            }
            set {
                var trackerNode = setTrackerToXml("AppsFlyer", value);
                var tracker = value as HiveConfigEditor.AppsFlyerTracker;
                if ( trackerNode != null && tracker != null && tracker.key != null ) {
                    XmlAttribute trackerName = CreateAttribute(configxml ,"name",tracker.name);
                    XmlAttribute trackerId = CreateAttribute(configxml ,"id", "unused");
                    XmlAttribute trackerKey = CreateAttribute(configxml ,"key",tracker.key);
                    XmlAttribute trackerItunse = CreateAttribute(configxml, "itunesConnectAppId", tracker.itunesConnectAppId);

                    trackerNode.Attributes.RemoveAll();
                    trackerNode.Attributes.SetNamedItem(trackerName);
                    trackerNode.Attributes.SetNamedItem(trackerId);
                    trackerNode.Attributes.SetNamedItem(trackerKey);
                    trackerNode.Attributes.SetNamedItem(trackerItunse);
                }
            }
        }

        public HiveConfigEditor.Tracker Firebase {
            get {
                HiveConfigEditor.FirebaseTracker tracker = new HiveConfigEditor.FirebaseTracker();
                tracker.name = "Firebase";

                XmlNode trackersNode = FindChildNode(properties, PROVIDERS_KEY);
                if (trackersNode != null) {
                    XmlNode trackerNode = FindChildNode(trackersNode, tracker.name);
                    if (trackerNode != null) {
                        XmlAttributeCollection trackerAttributes = trackerNode.Attributes;
                        
                        setupEvent(tracker, trackerNode);
                        return tracker;
                    }
                }
                tracker.AddEvent("TutorialComplete", "tutorial_complete");
                tracker.AddEvent("Update", "update");
                tracker.AddEvent("Purchase", "purchase");
                tracker.AddEvent("Open", "open");
                return tracker;
            }
            set {
                var trackerNode = setTrackerToXml("Firebase", value);
                var tracker = value as HiveConfigEditor.FirebaseTracker;
                if (trackerNode != null && tracker != null) {
                    XmlAttribute trackerName = CreateAttribute(configxml ,"name",tracker.name);

                    trackerNode.Attributes.RemoveAll();
                    trackerNode.Attributes.SetNamedItem(trackerName);
                }
            }
        }

		public bool IsValidAppId
		{
			get
			{
				return !IsEmpty(this.HIVEAppID);
			}
		}

		public bool IsValidFacebookAppId
		{
			get{
				return !IsEmpty(this.facebookAppID);
			}
		}

        public bool IsValidFacebookClientToken
        {
            get {
                return !IsEmpty(this.facebookClientToken);
            }
        }

        public bool IsValidGooglePlayAppId
		{
			get{
				return !IsEmpty(this.googlePlayAppID);
			}
		}

		public bool IsValidGoogleClientId {
			get {
				return !IsEmpty (this.googleServerClientID);
			}
		}

		public bool IsValidGoogleReverseClientId {
			get {
				return !IsEmpty (this.googleReversedClientID);
			}
		}



		public bool IsValidQQAppId {
			get {
				return !IsEmpty (this.qqAppId);
			}
		}

        public bool IsValidVKAppId {
            get {
                return !IsEmpty (this.vkAppId);
            }
        }

        public bool IsValidWeChatAppId {
            get {
                return !IsEmpty (this.weChatAppId);
            }
        }

        public bool IsValidLineChannelId {
            get {
                return !IsEmpty (this.lineChannelId);
            }
        }

        public bool IsValidWeChatPaymentKey {
            get {
                return !IsEmpty (this.weChatPaymentKey);
            }
        }


		private static bool IsEmpty(string value)
		{
			if( value != null && string.Empty.CompareTo(value) != 0 && value.Length > 0 )
				return false;
			else
				return true;
		}
    }

}