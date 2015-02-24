using System;

namespace Beem.Utility
{
    public static class DateHelper
    {
        public static string GetUnixTimestamp()
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            return ((int)t.TotalSeconds).ToString();
        }
    }
}
