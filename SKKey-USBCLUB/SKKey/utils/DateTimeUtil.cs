using System;

namespace SKKey.utils
{
    /**
     * 
     * 时间工具类
     * 
     **/
    sealed class DateTimeUtil
    {

        // 1467766472
        public static long getSystemTimestamp()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            Int64 timestamp = Convert.ToInt64((DateTime.Now - startTime).TotalSeconds);
            return timestamp;
        }

        // 1471073082764
        public static long getSystemTimestampMilli()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            Int64 timestamp = Convert.ToInt64((DateTime.Now - startTime).TotalMilliseconds);
            return timestamp;
        }
    }
}
