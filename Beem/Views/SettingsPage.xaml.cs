using Beem.Core.Models;
using Beem.Settings;
using Beem.ViewModels;
using Coding4Fun.Toolkit.Storage;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;
using System.Windows;

namespace Beem.Views
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        bool isPageLoading = true;

        public SettingsPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SettingsPage_Loaded);
            btnSignIn.SessionChanged += App.MicrosoftAccount_SessionChanged;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // ANALYTICS
            GoogleAnalytics.EasyTracker.GetTracker().SendView("SettingsPage");

            base.OnNavigatedTo(e);
        }

        void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            isPageLoading = false;
        }

        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            if (!isPageLoading)
            {
                CoreViewModel.Instance.CurrentAppSettings.CanRunUnderLockScreen = true;

                // ANALYTICS
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Settings", "RunUnderLockscreen", "Enabled", 0);

                StoreAndAlert();
            }
            else
            {
                isPageLoading = false;
            }
        }

        private void StoreAndAlert(bool showNotification = true)
        {
            SettingsManager.StoreSettings();

            if (showNotification)
                MessageBox.Show("You will need to restart the application for the lock screen settings to take effect.", 
                    "Beem", MessageBoxButton.OK);
        }

        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!isPageLoading)
            {
                CoreViewModel.Instance.CurrentAppSettings.CanRunUnderLockScreen = false;

                // ANALYTICS
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Settings", "RunUnderLockscreen", "Disabled", 0);

                StoreAndAlert();
            }
            else
            {
                isPageLoading = false;
            }
        }

        private void btnClearControls_Click(object sender, RoutedEventArgs e)
        {
            BackgroundAudioPlayer.Instance.Track = null;

            // ANALYTICS
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Settings", "ClearControls", "Clicked", 0);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear the local storage? All stored records will be deleted.", "Beem", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                // ANALYTICS
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Settings", "CleanStorage", "Clicked", 0);

                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (file.DirectoryExists("/Music"))
                    {
                        string[] names = file.GetFileNames("/Music/*");
                        foreach (string name in names)
                            file.DeleteFile("/Music/" + name);

                        MainPageViewModel.Instance.Recorded = new System.Collections.ObjectModel.ObservableCollection<string>();
                    }
                }
            }
        }

        private void tglScrobbleOnLaunch_Unchecked_1(object sender, RoutedEventArgs e)
        {
            if (!isPageLoading)
            {
                CoreViewModel.Instance.CurrentAppSettings.ScrobbleOnLaunch = false;

                // ANALYTICS
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Settings", "LastFmAutoScrobble", "Disabled", 0);

                StoreAndAlert(false);
            }
            else
            {
                isPageLoading = false;
            }
        }

        private void tglScrobbleOnLaunch_Checked_1(object sender, RoutedEventArgs e)
        {
            if (!isPageLoading)
            {
                CoreViewModel.Instance.CurrentAppSettings.ScrobbleOnLaunch = true;

                // ANALYTICS
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Settings", "LastFmAutoScrobble", "Enabled", 0);

                StoreAndAlert(false);
            }
            else
            {
                isPageLoading = false;
            }
        }

        private void btnLastFmLogin_Click_1(object sender, RoutedEventArgs e)
        {
            if (txtLastFmUsername.Text.Trim() == string.Empty)
            {
                MessageBox.Show("You need to specify a Last.fm username.", "Beem", MessageBoxButton.OK);
                return;
            }

            if (txtLastFmPassword.Password.Trim() == string.Empty)
            {
                MessageBox.Show("You need to specify a Last.fm password.", "Beem", MessageBoxButton.OK);
                return;
            }

            grdLoading.Visibility = System.Windows.Visibility.Visible;
            App.LFMClient.GetMobileSession(txtLastFmUsername.Text, txtLastFmPassword.Password, response =>
                {
                    Dispatcher.BeginInvoke(() =>
                        {
                            grdLoading.Visibility = System.Windows.Visibility.Collapsed;

                            if (response != null)
                            {
                                CoreViewModel.Instance.CurrentAppSettings.Session = response.Session;

                                // ANALYTICS
                                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Settings", "LastFmLogin", "Successful", 0);

                                SettingsManager.StoreSettings();
                            }
                            else
                            {
                                MessageBox.Show("Could not log in to Last.fm. Either your credentials are invalid, the service is down or you are not connected to a network.",
                                    "Beem", MessageBoxButton.OK);

                                // ANALYTICS
                                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Settings", "LastFmLogin", "Failed", 0);
                            }
                        });
                });
        }

        private void btnLastFmDeauth_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to deauthenticate Beem from using Last.fm?",
                "Beem", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                // ANALYTICS
                GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Settings", "LastFmLogin", "DeAuth", 0);

                CoreViewModel.Instance.CurrentAppSettings.ScrobbleOnLaunch = false;
                CoreViewModel.Instance.CurrentAppSettings.Session = new LastFMClient.LastFmSession();
                StoreAndAlert(false);
            }
        }

        // This will redirect the user to the key page, where he is able to grab it
        // and send it to Beem later.
        private void btnGetKey_Click(object sender, RoutedEventArgs e)
        {
            // ANALYTICS
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Settings", "DiFmKey", "GetKeyFromWeb", 0);

            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new System.Uri("http://www.di.fm/member/listen_key");
            task.Show();
        }

        private void btnAuthorize_Click(object sender, RoutedEventArgs e)
        {
            if (btnAuthorize.Content.ToString() == "Authorize")
            {
                if (!string.IsNullOrWhiteSpace(txtKey.Text))
                {
                    if (txtKey.Text.Length < 12)
                    {
                        MessageBox.Show("The DI.FM Premium key has to be at least 12 characters long.", "Beem", MessageBoxButton.OK);
                    }
                    else
                    {
                        CoreViewModel.Instance.DISettings.IsPremiumEnabled = true;
                        CoreViewModel.Instance.DISettings.PremiumKey = txtKey.Text;

                        Serialize.Save("di.xml", CoreViewModel.Instance.DISettings);

                        // ANALYTICS
                        GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Settings", "DiFmKey", "Stored", 0);
                    }
                }
                else
                {
                    MessageBox.Show("The DI.FM Premium key cannot be empty.", "Beem", MessageBoxButton.OK);
                }
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to deauthorize your DI.FM Premium key?", "Beem", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    CoreViewModel.Instance.DISettings = new DISettingsCache();
                    Serialize.Save("di.xml", CoreViewModel.Instance.DISettings);

                    // ANALYTICS
                    GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Settings", "DiFmKey", "DeAuth", 0);
                }
            }
        }

        private void SaveDISettings(object sender, RoutedEventArgs e)
        {
            Serialize.Save("di.xml", CoreViewModel.Instance.DISettings);
        }
    }
}