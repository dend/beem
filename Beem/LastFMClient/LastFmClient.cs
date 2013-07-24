using Beem.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Beem.LastFMClient
{
    public class LastFmClient
    {
        const string API_KEY = "";
        const string SECRET = "";
        const string CORE_URL = "https://ws.audioscrobbler.com/2.0/";

        public void GetMobileSession(string userName, string password, Action<LastFmAuthResponse> onCompletion)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("username", userName);
            parameters.Add("password", password);
            parameters.Add("method", "auth.getMobileSession");
            parameters.Add("api_key", API_KEY);

            string signature = GetSignature(parameters);

            string comboUrl = string.Concat(CORE_URL, "?method=auth.getMobileSession", "&api_key=", API_KEY,
                "&username=", userName, "&password=", password, "&api_sig=", signature);

            LastFmAuthResponse response = null;

            var client = new WebClient();
            client.UploadStringAsync(new Uri(comboUrl),string.Empty);
            client.UploadStringCompleted += (s, e) =>
                {
                    try
                    {
                        response = SerializationHelper.GetObjectFromString<LastFmAuthResponse>(e.Result);
                    }
                    catch (WebException ex)
                    {
                        HttpWebResponse exResponse = (HttpWebResponse)ex.Response;
                        using (StreamReader reader = new StreamReader(exResponse.GetResponseStream()))
                        {
                            Debug.WriteLine(reader.ReadToEnd());
                        }
                    }

                    onCompletion(response);
                };
        }

        public void ScrobbleTrack(string artist, string track, string sessionKey, Action<string> onCompletion)
        {
            string currentTimestamp = DateHelper.GetUnixTimestamp();

            var parameters = new Dictionary<string, string>();
            parameters.Add("artist[0]", artist);
            parameters.Add("track[0]", track);
            parameters.Add("timestamp[0]", currentTimestamp);
            parameters.Add("method", "track.scrobble");
            parameters.Add("api_key", API_KEY);
            parameters.Add("sk", sessionKey);

            string signature = GetSignature(parameters);

            string comboUrl = string.Concat(CORE_URL, "?method=track.scrobble", "&api_key=", API_KEY,
                "&artist[0]=", HttpUtility.UrlEncode(artist), "&track[0]=", HttpUtility.UrlEncode(track), "&sk=", sessionKey, "&timestamp[0]=", currentTimestamp,
                "&api_sig=", signature);

            var client = new WebClient();
            client.UploadStringAsync(new Uri(comboUrl), string.Empty);
            client.UploadStringCompleted += (s, e) =>
            {
                try
                {
                    onCompletion(e.Result);
                }
                catch (WebException ex)
                {
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        Debug.WriteLine(reader.ReadToEnd());
                    }
                }
            };
        }

        public string GetSignature(Dictionary<string, string> parameters)
        {
            string result = string.Empty;

            IOrderedEnumerable<KeyValuePair<string, string>> data = parameters.OrderBy(x=>x.Key);

            foreach (var s in data)
            {
                result += s.Key + s.Value;
            }

            result += SECRET;
            result = MD5Core.GetHashString(Encoding.UTF8.GetBytes(result));

            return result;
        }
    }
}
