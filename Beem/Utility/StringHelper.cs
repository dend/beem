using System.Net;
using System.Text.RegularExpressions;
using System;

namespace Beem.Utility
{
    public static class StringHelper
    {
        public static string EncodeToUpper(string raw)
        {
            raw = HttpUtility.UrlEncode(raw);
            return Regex.Replace(raw, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());
        }

        public static string SanitizeStatus(string status)
        {
            string blockedChars = @"!()*'";

            foreach (char c in blockedChars)
            {
                if (status.IndexOf(c) != -1)
                {
                    status = status.Replace(c.ToString(), "%" + String.Format("{0:X}", Convert.ToInt32(c)));
                }
            }

            return status;
        }

        public static string UNIXTimestamp
        {
            get
            {
                return Convert.ToString((int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
            }
        }
    }
}
