using System;
using UnityEngine;

namespace hive.manager {
    /// <summary>
    /// 다운로드 가능한 SDK 공지 목록 (Json Array로 받아야합니다.)
    /// </summary>
    [System.Serializable]
    public class AvailableNotice : ISerializationCallbackReceiver, IComparable{
        /// <summary>
        /// 공지사항 ID
        /// </summary>
        public string id;
        /// <summary>
        /// 공지 노출 시작 일자 (Unix Timestamp)
        /// </summary>
        public DateTime periodFromDateTime;
        [SerializeField]
        private double periodFrom;
        /// <summary>
        /// 공지 노출 종료 일자 (Unix Timestamp)
        /// </summary>
        public DateTime periodToDateTime;
        [SerializeField]
        private double periodTo;
        
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
        private string koreanTitle = "";
        /// <summary>
        /// 제목 (영어)
        /// </summary>
        [SerializeField]
        private string englishTitle = "";
        /// <summary>
        /// 내용 (한글))
        /// </summary>
        [SerializeField]
        private string koreanDetails = "";
        /// <summary>
        /// 내용 (영어)
        /// </summary>
        [SerializeField]
        private string englishDetails = ""; 
        /// <summary>
        /// 다운로드 활성화 여부
        /// </summary>
        [NonSerialized]
        public bool isActivation;
        /// <summary>
        /// 다운로드 활성화 (0: 비활성화, 1:활성화)    
        /// </summary>
        [SerializeField]
        public int activation;
        /// <summary>
        /// 최초 등록 일자 (Unix Timestamp)
        /// </summary>
        public DateTime registrationDateTime;
        [SerializeField]
        private double registrationDate;
        /// <summary>
        /// 마지막 수정 일자 (Unix Timestamp)
        /// </summary>
        public DateTime lastUpdateDateTime;
        [SerializeField]
        private double lastUpdateDate;


        public void OnAfterDeserialize() {
            periodFromDateTime = periodFrom.ToDateTime();
            periodToDateTime = periodTo.ToDateTime();
            registrationDateTime = registrationDate.ToDateTime();
            lastUpdateDateTime = lastUpdateDate.ToDateTime();
            isActivation = activation == 1 ? true : false;
        }
        
        public void OnBeforeSerialize() {
            periodFrom = periodFromDateTime.ToUnixTimestamp();
            periodTo = periodToDateTime.ToUnixTimestamp();
            registrationDate = registrationDateTime.ToUnixTimestamp();
            lastUpdateDate = lastUpdateDateTime.ToUnixTimestamp();
            activation = isActivation ? 1 : 0;
        }

        public int CompareTo(object obj) {
            var other = (AvailableNotice)obj;
            return registrationDateTime.CompareTo(other.registrationDateTime);
        }
    }   
}