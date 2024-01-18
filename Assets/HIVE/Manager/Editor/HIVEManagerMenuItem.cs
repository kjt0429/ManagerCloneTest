using UnityEditor;

namespace hive.manager
{
    public static class HIVEManagerMenuItem {
        #region MenuItem EnableCheck 
        [MenuItem("Hive/HIVE SDK Manager/SDK Upgrade", true)]
        static bool isShowUpdateListEnable() {
            return HIVEManagerController.isShowUpdateListEnable();
        }
        [MenuItem("Hive/HIVE SDK Manager/Integrity Verification", true)]
        static bool isShowIntegrityVerificationEnable() {
            return HIVEManagerController.isShowIntegrityVerificationEnable();
        }
        [MenuItem("Hive/HIVE SDK Manager/Version", true)]
        static bool isShowNowVersionEnable() {
            return HIVEManagerController.isShowNowVersionEnable();
        }
        [MenuItem("Hive/HIVE SDK Manager/Restore HIVE SDK",true)]
        static bool isShowRepairSDK() {
            return HIVEManagerController.isShowRepairMode();
        }
        #endregion
        
        #region MenuItem
        [MenuItem("Hive/HIVE SDK Manager/SDK Upgrade")]
        public static void ShowUpdateList() {
            HIVEManagerController.ShowUpdateList();
        }

        [MenuItem("Hive/HIVE SDK Manager/Integrity Verification")]
        public static void ShowIntegrityVerification() {
            HIVEManagerController.ShowIntegrityVerification();
        }


        [MenuItem("Hive/HIVE SDK Manager/Notice")]
        public static void ShowNotices() {
            HIVEManagerController.ShowNotices();
        }

        [MenuItem("Hive/HIVE SDK Manager/Version")]
        public static void ShowNowVersion() {
            HIVEManagerController.ShowNowVersion();
        }
        
        [MenuItem("Hive/HIVE SDK Manager/Restore HIVE SDK")]
        public static void ShowRepairSDK() {
            HIVEManagerController.RepairSDK();
        }

        [MenuItem("Hive/HIVE SDK Manager/Retry to connect Manager")]
        public static void RetryHIVEManagerConnection() {
            HIVEManagerController.RetryHIVEManagerConnection();
        }

        //Edit Ignore List
        [MenuItem("Hive/HIVE SDK Manager/Edit Ignore File")]
        public static void EditIgnore(){
            HIVEIgnoreManager.Instance.EditIgnoreFile();
        }
        #endregion
    }
}