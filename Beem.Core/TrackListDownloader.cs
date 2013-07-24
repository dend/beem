using System;
using System.Json;
using System.Net;
using System.Windows;
using Beem.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beem.Core
{
    public class TrackListDownloader
    {
        public async Task<List<Track>> GetTracksForStation(string stationId)
        {
            WebClient client = new WebClient();
            string data = await client.DownloadStringTaskAsync("http://api.v2.audioaddict.com/v1/di/track_history/channel/" + stationId + ".json?callback=_API_TrackHistory_getChannel");
            var list = GetTrackList(data);

            return list;
        }


        List<Track> GetTrackList(string sourceJson)
        {
            List<Track> tracks = new List<Track>();

            try
            {
                string rawJson = sourceJson;
                JsonArray rootArray = (JsonArray)JsonObject.Parse(rawJson);

                foreach (JsonValue value in rootArray)
                {
                    if (value["type"].ToString() != "\"advertisement\"")
                    {
                        Track track = new Track();
                        if (value["artist"] != null)
                            track.Artist = value["artist"].ToString().Replace("\"", ""); ;
                        track.FullTrackName = value["track"].ToString().Replace("\"", "");
                        if (value["title"] != null)
                            track.Title = value["title"].ToString().Replace("\"", "");
                        if (value["duration"] != null)
                        {
                            try
                            {
                                string secondDuration = value["duration"].ToString().Replace("\"", "");
                                int intDuration = Convert.ToInt32(secondDuration);
                                TimeSpan span = TimeSpan.FromSeconds(intDuration);

                                track.Duration = string.Format("{0}m {1}s",
                                    ((int)span.TotalMinutes).ToString(),
                                    span.Seconds.ToString());
                            }
                            catch
                            {
                                track.Duration = "00:00";
                            }
                        }

                        tracks.Add(track);
                    }
                }
            }
            catch
            { // The tracklist was not downloaded for some reason.
            }

            return tracks;
        }
    }
}
