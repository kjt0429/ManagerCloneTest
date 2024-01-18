using System.Collections;
using UnityEngine;
using System;
using NUnit.Framework;

namespace hive.manager {
    /// <summary>
    /// /versions/available
    /// </summary>
    [System.Serializable]
    public class AvailableDownload : ISerializationCallbackReceiver, System.IComparable{
        public AvailableDownload() {}
        public AvailableDownload(long id, HIVESemanticVersion versionCode, long projectId,
                                long versionId, DateTime deployDateTime, 
                                string koreanTitle, string englishTitle, string koreanDetails, string englishDetails,
                                bool isActivation, DateTime registrationDateTime, DateTime lastUpdateDateTime) {
            this.id = id;
            this.versionCode = versionCode;
            this.projectId = projectId;
            this.versionId = versionId;
            this.deployDateTime = deployDateTime;
            this.koreanTitle = koreanTitle;
            this.englishTitle = englishTitle;
            this.koreanDetails = koreanDetails;
            this.englishDetails = englishDetails;
            this.isActivation = isActivation;
            this.registrationDateTime = registrationDateTime;
            this.lastUpdateDateTime = lastUpdateDateTime;
        }
        /// <summary>
        /// 다운로드 설정 ID
        /// </summary>
        public long id;
        /// <summary>
        /// 버전 명칭
        /// </summary>
        [NonSerialized]
        public HIVESemanticVersion versionCode;
        [SerializeField]
        private string version;
        /// <summary>
        /// 프로젝트 ID
        /// </summary>
        public long projectId;
        /// <summary>
        /// 버전 ID
        /// </summary>
        public long versionId;
        /// <summary>
        /// 배포 일자
        /// </summary>
        [NonSerialized]
        public DateTime deployDateTime;
        [SerializeField]
        private double deployDate;

        public string title {
            get {
                return koreanTitle.IfOSLanguageCodeMatchStringOrAltString("ko", englishTitle);
            }
        }
        
        public string details {
            get {
                return koreanDetails.IfOSLanguageCodeMatchStringOrAltString("ko",englishDetails);
            }
        }

        /// <summary>
        /// 제목 (한글)
        /// </summary>
        [SerializeField]
        private string koreanTitle;
        /// <summary>
        /// 제목 (영어)
        /// </summary>
        [SerializeField]
        private string englishTitle;
        /// <summary>
        /// 내용 (한글)
        /// </summary>
        [SerializeField]
        private string koreanDetails;
        /// <summary>
        /// 내용 (영어)
        /// </summary>
        [SerializeField]
        private string englishDetails;
        /// <summary>
        /// 다운로드 활성화 여부
        /// </summary>
        [NonSerialized]
        public bool isActivation;
        /// <summary>
        /// 다운로드 활성화 (0: 비활성화, 1: 활성화)
        /// </summary>
        [SerializeField]
        public int activation;
        /// <summary>
        /// 최초 등록 일자
        /// </summary>
        [NonSerialized]
        public DateTime registrationDateTime;
        [SerializeField]
        private double registrationDate;
        /// <summary>
        /// 마지막 수정 일자.
        /// </summary>
        [NonSerialized]
        public DateTime lastUpdateDateTime;
        [SerializeField]
        private double lastUpdateDate;

        public void OnAfterDeserialize() {
            versionCode = version.ToSemanticVersion();
            deployDateTime = deployDate.ToDateTime();
            registrationDateTime = registrationDate.ToDateTime();
            lastUpdateDateTime = lastUpdateDate.ToDateTime();
            isActivation = activation == 1 ? true : false;
        }
        
        public void OnBeforeSerialize() {
            version = versionCode;
            deployDate = deployDateTime.ToUnixTimestamp();
            registrationDate = registrationDateTime.ToUnixTimestamp();
            lastUpdateDate = lastUpdateDateTime.ToUnixTimestamp();
            activation = isActivation ? 1 : 0;
        }

        public int CompareTo(object obj) {
            var other = obj as AvailableDownload;
            return versionCode.CompareTo(other.versionCode);
        }
    }
    [TestFixture]
    class AvailableDownloadTest {
        AvailableDownload standard = new AvailableDownload(
            100,"4.0.0",100,400,new DateTime(2019,9,2),
            "한국어 타이틀", "english Title", "한국어 디테일", "english Details",
            true, new DateTime(2019,9,2), new DateTime(2019,9,2)
        );

        [Test]
        public void JsonSerializationTest() {
            string jsonString = JsonUtility.ToJson(standard);
            var parsed = JsonUtility.FromJson<AvailableDownload>(jsonString);

            TestContext.WriteLine("parsed Json");
            TestContext.WriteLine(jsonString);
            
            Assert.AreEqual(parsed.id, standard.id);
            Assert.AreEqual(parsed.versionCode, standard.versionCode);
            Assert.AreEqual(parsed.projectId, standard.projectId);
            Assert.AreEqual(parsed.versionId, standard.versionId);
            Assert.AreEqual(parsed.deployDateTime.ToLocalTime(), standard.deployDateTime.ToLocalTime());
            Assert.AreEqual(parsed.title, standard.title);
            Assert.AreEqual(parsed.details, standard.details);
            Assert.AreEqual(parsed.isActivation, standard.isActivation);
            Assert.AreEqual(parsed.registrationDateTime.ToLocalTime(), standard.registrationDateTime.ToLocalTime());
            Assert.AreEqual(parsed.lastUpdateDateTime.ToLocalTime(), standard.lastUpdateDateTime.ToLocalTime());
        }

        [Test]
        public void JsonDeSerializationTest() {
            var standardJson = @"{""id"":100,""version"":""4.0.0"",""projectId"":100,""versionId"":400,""deployDate"":1567382400000.0,""koreanTitle"":""한국어 타이틀"",""englishTitle"":""english Title"",""koreanDetails"":""한국어 디테일"",""englishDetails"":""english Details"",""activation"":1,""registrationDate"":1567382400000.0,""lastUpdateDate"":1567382400000.0}";
            var parsed = JsonUtility.FromJson<AvailableDownload>(standardJson);

            Assert.AreEqual(parsed.id, standard.id);
            Assert.AreEqual(parsed.versionCode, standard.versionCode);
            Assert.AreEqual(parsed.projectId, standard.projectId);
            Assert.AreEqual(parsed.versionId, standard.versionId);
            Assert.AreEqual(parsed.deployDateTime.ToLocalTime(), standard.deployDateTime.ToLocalTime());
            Assert.AreEqual(parsed.title, standard.title);
            Assert.AreEqual(parsed.details, standard.details);
            Assert.AreEqual(parsed.isActivation, standard.isActivation);
            Assert.AreEqual(parsed.registrationDateTime.ToLocalTime(), standard.registrationDateTime.ToLocalTime());
            Assert.AreEqual(parsed.lastUpdateDateTime.ToLocalTime(), standard.lastUpdateDateTime.ToLocalTime());
        }
    }
}   