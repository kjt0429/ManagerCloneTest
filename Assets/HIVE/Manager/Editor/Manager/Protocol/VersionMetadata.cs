using System.Collections.Generic;
using System.Runtime.Serialization;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;
using System.Xml.Serialization;


namespace hive.manager {

    /// <summary>
    /// 특정 버전의 메타데이터. 
    /// 두가지 버전을 비교하거나 한가지 버전의 데이터를 내려준다.
    /// versions/{version}
    /// versions/{oldVersion}/to/{newVersion}
    /// </summary>
    [System.Serializable()]
    public class VersionMetadata : ISerializationCallbackReceiver {
        public VersionMetadata() {}

        [System.Serializable()]
        public class Files : ISerializationCallbackReceiver, System.IComparable {

            public Files() {}

            public enum ChangeMethod {
                Add,
                Modify,
                Delete
            }
            /// <summary>
            /// 파일 변경 점 (Add, Modify, Delete)
            /// </summary>
            [System.NonSerialized]
            public ChangeMethod changeType; //Add, Modify, Delete
            [SerializeField]
            private string change;
            /// <summary>
            /// 파일의 경로
            /// </summary>
            public string path;
            /// <summary>
            /// 파일 컨텐츠 해시 값 (SHA1)
            /// </summary>
            public string fileHash;

            public void OnAfterDeserialize() {
                var method = change.ToLower();
                //string to enum
                if (method == "add") {
                    changeType = ChangeMethod.Add;
                }
                else if (method == "modify") {
                    changeType = ChangeMethod.Modify;
                }
                else if (method == "delete") {
                    changeType = ChangeMethod.Delete;
                }
            }
            public void OnBeforeSerialize() {
                switch(changeType) {
                    case ChangeMethod.Add:
                    change = "add";
                    break;
                    case ChangeMethod.Modify:
                    change = "modify";
                    break;
                    case ChangeMethod.Delete:
                    change = "delete";
                    break;
                }
            }

            public int CompareTo(object obj) {
                if(obj == null) return 1;
                var other = (Files)obj;

                if (this.changeType == other.changeType &&
                    this.fileHash == other.fileHash &&
                    this.path == other.path) {
                        return 0;
                }

                if (changeType == ChangeMethod.Delete && other.changeType ==  ChangeMethod.Delete) {
                    return fileHash.CompareTo(other.fileHash);
                } else if (changeType == ChangeMethod.Delete) {
                    return -1;
                } else if (other.changeType == ChangeMethod.Delete) {
                    return 1;
                }
                return fileHash.CompareTo(other.fileHash);
            }
        }

        /// <summary>
        /// 현재 버전만 요청하였을때 받는 데이터
        /// </summary>
        [SerializeField]
        private string version;

        /// <summary>
        /// 현재 버전
        /// </summary>
        [SerializeField]
        private string oldVersion;
        [System.NonSerialized]
        public HIVESemanticVersion oldVersionCode;
        /// <summary>
        /// 비교할 버전 비교할 버전이 없을경우 현재버전과 동일함.
        /// </summary>
        [SerializeField]
        private string newVersion;
        [System.NonSerialized]
        public HIVESemanticVersion newVersionCode;
        /// <summary>
        /// SDK 파일 다운로드 URL
        /// </summary>
        public string downloadUrl;
        /// <summary>
        /// 파일 패치 데이터
        /// </summary>
        public List<Files> files;

        public void OnAfterDeserialize() {
            if (version != null) {
                oldVersionCode = version.ToSemanticVersion();
                newVersionCode = version.ToSemanticVersion();
            }
            if (newVersion != null) newVersionCode = newVersion.ToSemanticVersion();
            if (oldVersion != null) oldVersionCode = oldVersion.ToSemanticVersion();
        }

        public void OnBeforeSerialize() {
            version = oldVersionCode;
            newVersion = newVersionCode;
            oldVersion = oldVersionCode;
        }
    }

    [TestFixture]
    class VersionMetadataTest {
        [Test] 
        public void JsonSerializationTest() {

            var oldVersion = new HIVESemanticVersion("4.11.0");
            var newVersion = new HIVESemanticVersion("4.12.0");
            var downloadUrl = "https://www.withhive.com";

            var filePath1 = "filePathString/file/file";
            var fileHash1 = "abcdefg";
            var filePath2 = "filePathString/aaaaaaaaa";
            var fileHash2 = "gfedcba";

            var testJson = @"
            {
                ""oldVersion"":"""+oldVersion+@""",
                ""newVersion"":"""+newVersion+@""",
                ""downloadUrl"":"""+downloadUrl+@""",
                ""files"": [
                    {
                        ""change"":""Add"",
                        ""path"":"""+filePath1+@""",
                        ""fileHash"":"""+fileHash1+@"""
                    },
                    {
                        ""change"":""Modify"",
                        ""path"":"""+filePath2+@""",
                        ""fileHash"":"""+fileHash2+@"""
                    },
                    {
                        ""change"":""Delete"",
                        ""path"":""filePathString"",
                        ""fileHash"":""adsfasdfasdfasdfasfasdfadsfasdf""
                    }
                ]
            }
            ";

            var testObject = JsonUtility.FromJson<VersionMetadata>(testJson);
            Assert.AreEqual(oldVersion, testObject.oldVersionCode);
            Assert.AreEqual(newVersion, testObject.newVersionCode);
            Assert.AreEqual(downloadUrl, testObject.downloadUrl);

            Assert.AreEqual(VersionMetadata.Files.ChangeMethod.Add, testObject.files[0].changeType);
            Assert.AreEqual(filePath1, testObject.files[0].path);
            Assert.AreEqual(fileHash1, testObject.files[0].fileHash);
            
            Assert.AreEqual(VersionMetadata.Files.ChangeMethod.Modify, testObject.files[1].changeType);
            Assert.AreEqual(filePath2, testObject.files[1].path);
            Assert.AreEqual(fileHash2, testObject.files[1].fileHash);

            Assert.AreEqual(VersionMetadata.Files.ChangeMethod.Delete, testObject.files[2].changeType);
        }

        [Test]
        public void JsonDeserializationTest() {
            var testObject = new VersionMetadata();
            testObject.downloadUrl = "https://withhive.com";
            testObject.newVersionCode = "4.12.0";
            testObject.oldVersionCode = "4.11.0";
            testObject.files = new List<VersionMetadata.Files>() {
                new VersionMetadata.Files() {
                    changeType = VersionMetadata.Files.ChangeMethod.Add,
                    path = "abcd/cc/bdcd",
                    fileHash = "aabbcc"
                },
                new VersionMetadata.Files() {
                    changeType = VersionMetadata.Files.ChangeMethod.Modify,
                    path = "abcd",
                    fileHash = "ccddaa"
                },
                new VersionMetadata.Files() {
                    changeType = VersionMetadata.Files.ChangeMethod.Delete,
                    path = "abcd/cc/bdcd",
                    fileHash = "aabbcc"
                }
            };

            var json = JsonUtility.ToJson(testObject);
            var objectFromTestJson = JsonUtility.FromJson<VersionMetadata>(json);

            var successJson = @"{
                ""oldVersion"":""4.11.0"",
                ""newVersion"":""4.12.0"",
                ""downloadUrl"":""https://withhive.com"",
                ""files"":[
                    {""change"":""Add"",""path"":""abcd/cc/bdcd"",""fileHash"":""aabbcc""},
                    {""change"":""Modify"",""path"":""abcd"",""fileHash"":""ccddaa""},
                    {""change"":""Delete"",""path"":""abcd/cc/bdcd"",""fileHash"":""aabbcc""}
                ]
            }";
            var successObject = JsonUtility.FromJson<VersionMetadata>(successJson);
            
            Assert.AreEqual(objectFromTestJson.oldVersionCode, successObject.oldVersionCode);
            Assert.AreEqual(objectFromTestJson.newVersionCode, successObject.newVersionCode);
            Assert.AreEqual(objectFromTestJson.downloadUrl, successObject.downloadUrl);

            Assert.AreEqual(objectFromTestJson.files[0].changeType,     successObject.files[0].changeType);
            Assert.AreEqual(objectFromTestJson.files[0].fileHash,   successObject.files[0].fileHash);
            Assert.AreEqual(objectFromTestJson.files[0].path,       successObject.files[0].path);

            Assert.AreEqual(objectFromTestJson.files[1].changeType,     successObject.files[1].changeType);
            Assert.AreEqual(objectFromTestJson.files[1].fileHash,   successObject.files[1].fileHash);
            Assert.AreEqual(objectFromTestJson.files[1].path,       successObject.files[1].path);

            Assert.AreEqual(objectFromTestJson.files[2].changeType,     successObject.files[2].changeType);
            Assert.AreEqual(objectFromTestJson.files[2].fileHash,   successObject.files[2].fileHash);
            Assert.AreEqual(objectFromTestJson.files[2].path,       successObject.files[2].path);
        }
    }
}