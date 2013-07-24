using Beem.Azure;
using Beem.Core.Models;
using Beem.Settings;
using Beem.Utility;
using Coding4Fun.Toolkit.Storage;
using Microsoft.Live;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Beem
{
    public partial class MainPage : PhoneApplicationPage
    {
        LiveConnectClient client;
        string customStationToLoad = string.Empty;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            btnSignIn.SessionChanged += App.MicrosoftAccount_SessionChanged;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (!SettingsManager.AttemptToLoadSettings())
            {
                if (MessageBox.Show("Would you like Beem to run under lock screen? This way the streaming is not disrupted.", "Run Under Lock Screen", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    Binder.Instance.CurrentAppSettings.CanRunUnderLockScreen = true;
                }
                else
                {
                    Binder.Instance.CurrentAppSettings.CanRunUnderLockScreen = false;
                }

                Binder.Instance.CurrentAppSettings.FirstLaunchFlag = true;
                Binder.Instance.CurrentAppSettings.ShowRecordingAlert = true;

                SettingsManager.StoreSettings();
            }

            Utility.StationManager.DeserializeFavorites();

            if (Binder.Instance.Stations.Count == 0)
            {
                LoadStations();
            }

            if (NavigationContext.QueryString.ContainsKey("DI.FM"))
            {
                customStationToLoad = NavigationContext.QueryString["DI.FM"];
            }
            base.OnNavigatedTo(e);
        }

        private async void LoadStations()
        {
            try
            {
                grdLoading.Visibility = System.Windows.Visibility.Visible;

                List<Station> stations = (await MobileServiceClientHelper.GetAllStations()).ToList();
                Debug.WriteLine(stations.GetType());

                if (stations != null)
                {
                    // Storing the station cache so that I can manipulate those
                    // with the back/forward buttons.
                    Serialize.Save<List<Station>>("stationcache.xml", stations);

                    HandleStationList(stations);
                }
                else
                    NotifyOfStationDownloadError();
            }
            catch
            {
                NotifyOfStationDownloadError();
            }
        }

        void NotifyOfStationDownloadError()
        {
            MessageBox.Show("Seems like Beem can't get the stations at this point. Try again later.",
       "Download Station List", MessageBoxButton.OK);

            customStationToLoad = string.Empty;
            grdLoading.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void HandleStationList(IEnumerable<Station> stations)
        {

            Binder.Instance.Stations = new System.Collections.ObjectModel.ObservableCollection<Station>(stations);

            if (!string.IsNullOrEmpty(customStationToLoad))
            {
                Station currentStation = (from c in Binder.Instance.Stations where c.Name == customStationToLoad select c).FirstOrDefault();

                if (currentStation != null)
                {
                    Binder.Instance.CurrentStation = currentStation;
                    NavigationService.Navigate(new Uri("/StationPlayer.xaml", UriKind.Relative));
                }
            }

            customStationToLoad = string.Empty;
            grdLoading.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void mnuPin_Click(object sender, RoutedEventArgs e)
        {
            Station station = ((MenuItem)sender).Tag as Station;

            Utility.StationManager.Pin(station);
        }

        private void mnuFav_Click(object sender, RoutedEventArgs e)
        {
            Station station = ((MenuItem)sender).Tag as Station;
            if (!Utility.StationManager.CheckIfExists(station))
            {
                Binder.Instance.FavoriteStations.Add(station);
                Utility.StationManager.SerializeFavorites();
            }
        }

        private void mnuRemFav_Click(object sender, RoutedEventArgs e)
        {
            Utility.StationManager.Remove(((MenuItem)sender).Tag as Station);
            Utility.StationManager.SerializeFavorites();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            PlayStation(sender);
        }

        private void stkStation_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PlayStation(sender);
        }

        private void PlayStation(object sender)
        {
            FrameworkElement tapStack = (FrameworkElement)sender;
            Station currentStation = (Station)tapStack.Tag;
            Binder.Instance.CurrentStation = currentStation;
            NavigationService.Navigate(new Uri("/StationPlayer.xaml", UriKind.Relative));
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((PivotItem)e.AddedItems[0]).Name == "pvtRecorded")
            {
                RecordManager.GetRecords();
            }
        }

        private void Player_Click(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Tag.ToString();

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string fullName = "Music/" + name;

                BackgroundAudioPlayer.Instance.Track = new AudioTrack(new Uri(fullName, UriKind.Relative), name,
                    "Beem", "Beem", new Uri("/Images/beem_media.jpg", UriKind.Relative));
                BackgroundAudioPlayer.Instance.Play();
            }
        }


        private void mnuRingtone_Click(object sender, RoutedEventArgs e)
        {
            string fileName = ((MenuItem)sender).Tag.ToString();
            string fullName = "isostore:/Music/" + fileName;

            SaveRingtoneTask ringtoneTask = new SaveRingtoneTask();
            ringtoneTask.Source = new Uri(fullName);
            ringtoneTask.DisplayName = fileName;
            ringtoneTask.Show();
        }

        private void mnuDelete_Click(object sender, RoutedEventArgs e)
        {
            string name = ((MenuItem)sender).Tag.ToString();
            if (MessageBox.Show("Are you sure you want to delete " + name + "?", "Delete File", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Binder.Instance.Recorded.Remove(name);
                IsolatedStorageFile.GetUserStoreForApplication().DeleteFile("Music/" + name);
            }
        }

        private void btnNowPlaying_Click(object sender, EventArgs e)
        {
            if (Binder.Instance.CurrentStation != null)
            {
                NavigationService.Navigate(new Uri("/StationPlayer.xaml", UriKind.Relative));
            }
            else
            {
                if (!TryGetCurrentStation())
                {
                    MessageBox.Show("There is nothing playing right now!", "Now Playing", MessageBoxButton.OK);
                }
                else
                {
                    NavigationService.Navigate(new Uri("/StationPlayer.xaml", UriKind.Relative));
                }
            }
        }

        private bool TryGetCurrentStation()
        {
            if (BackgroundAudioPlayer.Instance.Track != null)
            {
                Station station = (from c in Binder.Instance.Stations
                                   where c.Name == BackgroundAudioPlayer.Instance.Track.Artist
                                   select c).FirstOrDefault();
                if (station != null)
                {
                    Binder.Instance.CurrentStation = station;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        private void mnuUpload_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.MicrosoftAccountSession != null)
                {
                    Binder.Instance.CurrentlyUploading = ((MenuItem)sender).Tag.ToString();

                    client = new Microsoft.Live.LiveConnectClient(App.MicrosoftAccountSession);
                    client.GetCompleted += client_GetCompleted;
                    client.GetAsync("me/skydrive/quota", null);
                }
                else
                {
                    MessageBox.Show("Cannot upload because there is no Microsoft Account connected or you are not connected to a network.\nGo to Settings > SkyDrive and connect an account.", "Upload", MessageBoxButton.OK);

                }
            }
            catch
            {
                FailureAlert();
            }
        }

        void FailureAlert()
        {
            MessageBox.Show("Upload failed. Please try again later.", "Upload", MessageBoxButton.OK);
            grdUpload.Visibility = System.Windows.Visibility.Collapsed;
            Binder.Instance.SkyDriveUploadProgress = 0;
            ApplicationBar.IsVisible = true;
        }

        void client_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            try
            {
                if (e.Result.ContainsKey("available"))
                {
                    Int64 available = Convert.ToInt64(e.Result["available"]);
                    byte[] data = RecordManager.GetRecordByteArray(Binder.Instance.CurrentlyUploading);

                    if (available >= data.Length)
                    {
                        MemoryStream stream = new MemoryStream(data);

                        client = new LiveConnectClient(App.MicrosoftAccountSession);
                        client.UploadCompleted += MicrosoftAccountClient_UploadCompleted;
                        client.UploadProgressChanged += MicrosoftAccountClient_UploadProgressChanged;
                        client.UploadAsync("me/skydrive", Binder.Instance.CurrentlyUploading,
                            stream, OverwriteOption.Overwrite);
                        grdUpload.Visibility = System.Windows.Visibility.Visible;
                        ApplicationBar.IsVisible = false;
                    }
                    else
                    {
                        MessageBox.Show("Looks like you don't have enough space on your SkyDrive. Go to http://skydrive.com and either purchase more space or clean up the existing storage.", "Upload",
                            MessageBoxButton.OK);
                    }
                }
            }
            catch
            {
                FailureAlert();
            }
        }

        void MicrosoftAccountClient_UploadProgressChanged(object sender, Microsoft.Live.LiveUploadProgressChangedEventArgs e)
        {
            Binder.Instance.SkyDriveUploadProgress = e.ProgressPercentage;
        }

        void MicrosoftAccountClient_UploadCompleted(object sender, Microsoft.Live.LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                grdUpload.Visibility = System.Windows.Visibility.Collapsed;
                Binder.Instance.SkyDriveUploadProgress = 0;
                ApplicationBar.IsVisible = true;
            }
            else
            {
                FailureAlert();
            }
        }

        private void btnRefreshStationList_Click_1(object sender, EventArgs e)
        {
            LoadStations();
        }
    }
}