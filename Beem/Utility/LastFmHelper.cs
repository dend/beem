using Beem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Beem.Utility
{
    public static class LastFmHelper
    {
        internal static void ScrobbleCurrentTrack(bool showNotification = true)
        {
            if (CoreViewModel.Instance.CurrentAppSettings.Session != null &&
                !string.IsNullOrEmpty(CoreViewModel.Instance.CurrentAppSettings.Session.Key))
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        string trackArtist = string.IsNullOrEmpty(CoreViewModel.Instance.CurrentStation.NowPlaying.Artist) ?
                            CoreViewModel.Instance.CurrentStation.Name : CoreViewModel.Instance.CurrentStation.NowPlaying.Artist;

                        App.LFMClient.ScrobbleTrack(
                        trackArtist,
                        CoreViewModel.Instance.CurrentStation.NowPlaying.Title,
                        CoreViewModel.Instance.CurrentAppSettings.Session.Key,
                        (data) =>
                        {
                            if (showNotification)
                            {
                                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    MessageBox.Show("Scrobbled track to Last.fm!", "Beem", MessageBoxButton.OK);
                                });

                                // ANALYTICS
                                if (CoreViewModel.Instance.CurrentAppSettings.EnableAnalytics)
                                {
                                    GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Stations", "ScrobbleTrack", CoreViewModel.Instance.CurrentStation.NowPlaying.FullTrackName, 0);
                                }
                            }
                        });
                    }
                    catch
                    {
                        MessageBox.Show("Can't scrobble track. Make sure you are logged into Last.fm and you are connected to a network.",
                            "Beem", MessageBoxButton.OK);
                    }
                });
            }
            else
            {
                if (showNotification)
                {
                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Cannot scrobble track. Try authenticating with Last.fm first.", "Beem",
                            MessageBoxButton.OK);
                    });
                }
            }
        }
    }
}
