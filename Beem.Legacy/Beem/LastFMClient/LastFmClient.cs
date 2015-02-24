using Beem.Utility;
using Beem.ViewModels;
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
        const string CORE_URL = "https://ws.audioscrobbler.com/2.0/";

        public async void GetMobileSession(string userName, string password, Action<LastFmAuthResponse> onCompletion)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("username", userName);
            parameters.Add("password", password);
            parameters.Add("method", "auth.getMobileSession");
            parameters.Add("api_key", CoreViewModel.Instance.ApiKeys.LastFmKey);

            string signature = GetSignature(parameters);

            string comboUrl = string.Concat("method=auth.getMobileSession", "&api_key=", CoreViewModel.Instance.ApiKeys.LastFmKey,
                "&username=", userName, "&password=", password, "&api_sig=", signature);

            LastFmAuthResponse response = null;

            byte[] pendingPostContent = Encoding.UTF8.GetBytes(comboUrl);
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CORE_URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                await requestStream.WriteAsync(pendingPostContent, 0, pendingPostContent.Length);
            }

            request.BeginGetResponse(new AsyncCallback(n =>
                {
                    HttpWebResponse rawResponse = (HttpWebResponse)request.EndGetResponse(n);

                    string rawData = string.Empty;
                    using (StreamReader reader = new StreamReader(rawResponse.GetResponseStream()))
                    {
                        rawData = reader.ReadToEnd();
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(rawData))
                        {
                            response = SerializationHelper.GetObjectFromString<LastFmAuthResponse>(rawData);
                        }
                    }
                    catch
                    {
                        Debug.WriteLine(string.Format("[{0}]: LAST.FM - Failed to authenticate user.", DateTime.Now.ToString()));
                    }

                    onCompletion(response);

                }), null);
        }

        public async void ScrobbleTrack(string artist, string track, string sessionKey, Action<string> onCompletion)
        {
            string currentTimestamp = DateHelper.GetUnixTimestamp();

            var parameters = new Dictionary<string, string>();
            parameters.Add("artist[0]", artist);
            parameters.Add("track[0]", track);
            parameters.Add("timestamp[0]", currentTimestamp);
            parameters.Add("method", "track.scrobble");
            parameters.Add("api_key", CoreViewModel.Instance.ApiKeys.LastFmKey);
            parameters.Add("sk", sessionKey);

            string signature = GetSignature(parameters);

            string comboUrl = string.Concat("method=track.scrobble", "&api_key=", CoreViewModel.Instance.ApiKeys.LastFmKey,
                "&artist[0]=", HttpUtility.UrlEncode(artist), "&track[0]=", HttpUtility.UrlEncode(track), "&sk=", sessionKey, "&timestamp[0]=", currentTimestamp,
                "&api_sig=", signature);

            byte[] pendingPostContent = Encoding.UTF8.GetBytes(comboUrl);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CORE_URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                await requestStream.WriteAsync(pendingPostContent, 0, pendingPostContent.Length);
            }

            request.BeginGetResponse(new AsyncCallback(n =>
            {
                HttpWebResponse rawResponse = (HttpWebResponse)request.EndGetResponse(n);

                string rawData = string.Empty;
                using (StreamReader reader = new StreamReader(rawResponse.GetResponseStream()))
                {
                    rawData = reader.ReadToEnd();
                }

                try
                {
                    onCompletion(rawData);
                }
                catch
                {
                    Debug.WriteLine(string.Format("[{0}]: LAST.FM - Failed to scrobble track.", DateTime.Now.ToString()));
                }
            }), null);
        }

        public string GetSignature(Dictionary<string, string> parameters)
        {
            string result = string.Empty;

            IOrderedEnumerable<KeyValuePair<string, string>> data = parameters.OrderBy(x=>x.Key);

            foreach (var s in data)
            {
                result += s.Key + s.Value;
            }

            result += CoreViewModel.Instance.ApiKeys.LastFmSecret;
            result = MD5Core.GetHashString(Encoding.UTF8.GetBytes(result));

            return result;
        }
    }
}
