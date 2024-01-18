namespace hive.manager {
    public partial class HIVEManager {
        static string apiUrl {
            get {
                return "https://sdk.withhive.com";
            }
        }
        static string baseUrl {
            get {
                return apiUrl+"/groups/SDK_V4";
            }
        }
    }
}