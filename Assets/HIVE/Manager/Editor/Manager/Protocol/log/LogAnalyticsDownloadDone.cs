using System;
using UnityEngine;

namespace hive.manager {
    /// <summary>
    /// 다운로드 완료 후 클라이언트 로그 
    /// </summary>
    [System.Serializable]
    public class LogAnalyticsDownloadDone : ISerializationCallbackReceiver {
        
        /// <summary>
        /// 전송하는 유니티 에디터에 설정된 프로덕트의 이름
        /// </summary>
        public string productName = "";

        /// <summary>
        /// 전송하는 유니티 에디터에 설정된 회사이름
        /// </summary>
        public string companyName = "";

        /// <summary>
        /// 전송하는 유니티 에디터의 설정된 bundle ID 또는 package name
        /// </summary>
        public string appid = "";

        /// <summary>
        /// 설치에 성공한 hive sdk의 버전
        /// </summary>
        public string sdkVersion = "";

        /// <summary>
        /// 현재 hive sdk manager의 버전
        /// </summary>
        public string managerVersion = "";

        public DateTime installDateTime;
        [SerializeField]
        /// <summary>
        /// 현재 hive sdk manager의 버전
        /// </summary>
        private double installDate = 0;

        /// <summary>
        /// manager를 사용하는 플랫폼
        /// (ex: windows, mac, etc)
        /// </summary>
        public string platform = "";

        public void OnAfterDeserialize() {
            installDateTime = installDate.ToDateTime();
        }
        
        public void OnBeforeSerialize() {
            //다운로드 시간은 utc-0로 설정한다.
            installDate = installDateTime.ToUniversalTime().ToUnixTimestamp();
        }
    }   
}