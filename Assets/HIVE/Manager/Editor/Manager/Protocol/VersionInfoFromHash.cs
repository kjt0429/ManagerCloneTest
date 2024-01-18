using UnityEngine;
using System;

namespace hive.manager
{
    /// <summary>
    /// 해시에 해당하는 버전정보
    /// </summary>
    [System.Serializable]
    public class VersionInfoFromHash : ISerializationCallbackReceiver, System.IComparable {
        /// <summary>
        /// 버전 ID
        /// </summary>
        public long id;
        /// <summary>
        /// 프로젝트 ID
        /// </summary>
        public long projectId;
        /// <summary>
        /// 버전 명칭
        /// </summary>
        public HIVESemanticVersion versionCode;
        [SerializeField]
        private string version;
        /// <summary>
        /// 파일 컨텐츠 해시 값
        /// </summary>
        public string fileHash;
        /// <summary>
        /// 파일 업로드 일자 (Unix Timnestamp)
        /// </summary>
        public DateTime uploadDateTime;
        [SerializeField]
        private double uploadDate;
        /// <summary>
        /// 배포 일자 (Unix Timestamp)
        /// </summary>
        public DateTime deployDateTime;
        [SerializeField]
        private double deployDate;
        /// <summary>
        /// 파일 다운로드 URL
        /// </summary>
        public string downloadUrl;

        public void OnAfterDeserialize() {
            deployDateTime = deployDate.ToDateTime();
            uploadDateTime = uploadDate.ToDateTime();
            versionCode = version;
        }
        public void OnBeforeSerialize() {
            deployDate = deployDateTime.ToUnixTimestamp();
            uploadDate = uploadDateTime.ToUnixTimestamp();
            version = versionCode;
        }

        public int CompareTo(object obj) {
            return versionCode.CompareTo(obj);
        }
    }
}