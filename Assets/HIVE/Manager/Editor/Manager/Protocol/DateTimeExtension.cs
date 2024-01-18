using UnityEngine;
using System;

namespace hive.manager {

    public static class DateTimeExtension {
        public static DateTime ToDateTime(this double unixTimestamp) {
            DateTime dateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(unixTimestamp).ToLocalTime();
            return dateTime;
        }

        public static double ToUnixTimestamp(this DateTime dateTime) {
            return dateTime.Subtract(new DateTime(1970,1,1)).TotalMilliseconds;
        }
    }
}