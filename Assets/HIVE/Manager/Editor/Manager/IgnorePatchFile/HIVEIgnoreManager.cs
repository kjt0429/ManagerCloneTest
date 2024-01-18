using UnityEditor;
using hive.manager.editor;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace hive.manager {
    class HIVEIgnoreManager {

        private static readonly Lazy<HIVEIgnoreManager> lazy = new Lazy<HIVEIgnoreManager>(()=> new HIVEIgnoreManager());

        public static HIVEIgnoreManager Instance { get { return lazy.Value; }}

        private HIVEIgnoreManager() {}
        
        static string ignoreFileName = Path.Combine("HIVE","Manager","Editor","HIVEManagerIgnore.txt");
        static string ignoreFileNameAssets = Path.Combine("Assets", ignoreFileName);
        static string ignoreFileNameAbs = Path.GetFullPath(ignoreFileNameAssets);

        //예외로 지정되면 안되는 중요파일 리스트
        static string[] importantFileList = {
            "Hive_SDK_v4/Plugins/Android/libs/HIVE_SDK.aar",
            "Hive_SDK_v4/Plugins/Android/libs/HIVE_SDK_UnityPlugin.aar",
            "Hive_SDK_v4/Plugins/iOS/framework/HIVE_SDK.framework",
            "Hive_SDK_v4/Plugins/iOS/framework/HIVE_SDK_UnityPlugin.framework",
            "Hive_SDK_v4/Plugins/iOS/framework/HIVEService.framework",
            "Hive_SDK_v4/Plugins/iOS/framework/HIVECore.framework",
            "Hive_SDK_v4/Plugins/iOS/framework/HIVEProtocol.framework",
            "Hive_SDK_v4/Plugins/iOS/framework/ProviderAdapter.framework",
            "Hive_SDK_v4/Editor/VersionHash.txt"
        };

        static string COMMENT_IGNORE_FILE = @"# HIVE Ignore file
# Enter the folder or directory path to be excluded when patching for HIVE Manager.
# The path entered in this file is ignored when patching for HIVE Manager
# Enter the path under Assets as the relative path.
# Folder ignore example
# foo/bar
# File ignore example
# foo/bar.txt";

        HIVEIgnorePatchFile ignorePatchFile;
        //무시목록으로 설정할 수 없는 무시목록 필터.
        HIVEIgnorePatchFile ignoreFilter;

        bool isCaseSensetiveSystem() {
            string file = Path.GetTempPath() + Guid.NewGuid().ToString().ToLower();
            File.CreateText(file).Close();
            bool isCaseSensitive = !File.Exists(file.ToUpper());
            File.Delete(file);
            return isCaseSensitive;
        }

        public void loadIgnore() {
            var isCasesensetive = isCaseSensetiveSystem();
            ignorePatchFile = new HIVEIgnorePatchFile(Path.GetFullPath(ignoreFileNameAssets), isCasesensetive);
            ignorePatchFile.parse();
            
            ignoreFilter = new HIVEIgnorePatchFile(isCasesensetive);
            for (int i=0;i<importantFileList.Length;++i) {
                ignoreFilter.insertIgnorePath(importantFileList[i]);
            }
        }

        public bool isIgnore(string ignore) {
            //ignore filter 목록에 걸리면 중요파일이므로 ignore 검사를 하지 않고 return false를 무시목록에서 제외한다.
            if (ignoreFilter.isIgnore(ignore)) {
                return false;
            }
            return ignorePatchFile.isIgnore(ignore);
        }

        public void EditIgnoreFile() {
            if (!File.Exists(ignoreFileNameAbs)) {
                File.WriteAllText(ignoreFileNameAbs, COMMENT_IGNORE_FILE);
                AssetDatabase.Refresh();
            }
            var asset = AssetDatabase.LoadMainAssetAtPath(ignoreFileNameAssets);
            AssetDatabase.OpenAsset(asset);
        }
    }   
}