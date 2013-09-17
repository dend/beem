using Beem.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Nokia.Music.Tasks;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Beem.Utility
{
    public class ShellHelper
    {
        public static void SetCoreTile()
        {
            ShellTile appTile = ShellTile.ActiveTiles.First();

            if (appTile != null &&
                CoreViewModel.Instance.CurrentStation != null &&
                CoreViewModel.Instance.CurrentStation.NowPlaying != null)
            {
                ShellTileData data = new FlipTileData
                {
                    BackTitle = CoreViewModel.Instance.CurrentStation.Name,
                    BackContent = CoreViewModel.Instance.CurrentStation.NowPlaying.FullTrackName,
                    WideBackContent = CoreViewModel.Instance.CurrentStation.NowPlaying.FullTrackName,
                    WideBackBackgroundImage = new System.Uri("Images/Tile/plain_wide_bg.png",System.UriKind.Relative),
                    BackBackgroundImage = new System.Uri("Images/Tile/plain_wide_bg.png", System.UriKind.Relative)
                };

                appTile.Update(data);
            }
        }

        public static void SearchXboxMusic(string term)
        {
            MarketplaceSearchTask task = new MarketplaceSearchTask();
            task.ContentType = MarketplaceContentType.Music;

            task.SearchTerms = term;
            try
            {
                task.Show();
            }
            catch
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Couldn't access Xbox Music at this time, please try again later.", "Find Track", MessageBoxButton.OK);
                });
            }
        }

        public static void SearchNokiaMusic(string term)
        {
            MusicSearchTask searchTask = new MusicSearchTask();
            searchTask.SearchTerms = term;
            try
            {
                searchTask.Show();
            }
            catch
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Couldn't access Nokia Music at this time, please try again later.", "Find Track", MessageBoxButton.OK);
                });
            }
        }

        internal static void ShareTrack()
        {
            CustomMessageBox shareBox = new CustomMessageBox();
            shareBox.Message = "Share with the world the fact that you're rocking out to some amazing EDM with Beem!\n\nHow would you like to share the current station and track?";
            shareBox.Caption = "Share";

            StackPanel panel = new StackPanel();
            panel.Margin = new Thickness(0, 12, 0, 0);

            Button btnNativeShare = new Button { Content = "native sharing" };
            btnNativeShare.Click += btnNativeShare_Click;
            panel.Children.Add(btnNativeShare);

            Button btnLastFmScrobble = new Button { Content = "last.fm scrobble" };
            btnLastFmScrobble.Click += btnLastFmScrobble_Click;
            panel.Children.Add(btnLastFmScrobble);

            panel.Children.Add(new Button { Content = "NFC" });
            panel.Children.Add(new Button { Content = "beem-tranced" });

            shareBox.Content = panel;
            shareBox.Show();

        }

        static void btnLastFmScrobble_Click(object sender, RoutedEventArgs e)
        {
            // ANALYTICS
            if (CoreViewModel.Instance.CurrentAppSettings.EnableAnalytics)
            {
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Stations", "ShareThroughLastFm", CoreViewModel.Instance.CurrentStation.NowPlaying.FullTrackName, 0);
            }

            LastFmHelper.ScrobbleCurrentTrack();
        }

        static void btnNativeShare_Click(object sender, RoutedEventArgs e)
        {
            if (CoreViewModel.Instance.CurrentAppSettings.EnableAnalytics)
            {
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Stations", "ShareThroughWindowsPhone", CoreViewModel.Instance.CurrentStation.NowPlaying.FullTrackName, 0);
            }

            ShareLinkTask shareLink = new ShareLinkTask();
            shareLink.LinkUri = new Uri("http://bitly.com/BeemPlus");
            shareLink.Message = "Listening to " + CoreViewModel.Instance.CurrentStation.NowPlaying.FullTrackName + " with #Beem.";
            shareLink.Show();
        }
    }
}
