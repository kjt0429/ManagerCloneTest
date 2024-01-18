using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hive.manager {
    class HIVEManagerSDKFile {
        public static string getDownloadedFilePathFromAssetsFilePath(string path) {
            return path.GetHashSha1();
        }
    }
}
