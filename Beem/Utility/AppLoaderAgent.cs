using Beem.Azure;
using Beem.Core.Models;
using Beem.Settings;
using Beem.ViewModels;
using Coding4Fun.Toolkit.Storage;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Windows.Storage;

namespace Beem.Utility
{
    /// <summary>
    /// This class is responsible for all the application initialization routines.
    /// </summary>

    public class AppLoaderAgent
    {
        public static bool AttemptFirstRun()
        {
            MessageBoxResult result = MessageBoxResult.Cancel;

            if (!SettingsManager.AttemptToLoadSettings())
            {
                Deployment.Current.Dispatcher.BeginInvoke(()=>
                    {
                        result = MessageBox.Show("Would you like Beem to run under lock screen? This way the streaming is not disrupted.", "Run Under Lock Screen", MessageBoxButton.OKCancel);
                    });

                if (result == MessageBoxResult.OK)
                {
                    CoreViewModel.Instance.CurrentAppSettings.CanRunUnderLockScreen = true;
                }
                else
                {
                    CoreViewModel.Instance.CurrentAppSettings.CanRunUnderLockScreen = false;
                }

                CoreViewModel.Instance.CurrentAppSettings.FirstLaunchFlag = true;
                CoreViewModel.Instance.CurrentAppSettings.ShowRecordingAlert = true;

                SettingsManager.StoreSettings();

                return true;
            }
            else
            {
                return false;
            }
        }

        public async static Task<bool> AttemptStationLoading(string defaultStation = "")
        {
            MainPageViewModel.Instance.FavoriteStations = Serialize.Open<ObservableCollection<Station>>("fav.xml");

            if (MainPageViewModel.Instance.Stations.Count == 0)
            {
                try
                {

                    List<Station> stations = (await MobileServiceClientHelper.GetAllStations()).ToList();
                    Debug.WriteLine(stations.GetType());

                    if (stations != null)
                    {
                        // Storing the station cache so that I can manipulate those
                        // with the back/forward buttons.
                        Serialize.Save<List<Station>>("stationcache.xml", stations);

                        HandleStationList(stations, ref defaultStation);
                        return true;
                    }
                    else
                    {
                        NotifyOfStationDownloadError(ref defaultStation);
                        return false;
                    }
                }
                catch
                {
                    NotifyOfStationDownloadError(ref defaultStation);

                    return false;
                }
            }
            else
            {
                // No need to load stations.
                return false;
            }
        }

        static void NotifyOfStationDownloadError(ref string customStationToLoad)
        {
            MessageBox.Show("Seems like Beem can't get the stations at this point. Try again later.",
                "Download Station List", MessageBoxButton.OK);

            customStationToLoad = string.Empty;

            MainPageViewModel.Instance.IsCurrentlyLoading = false;
        }

        static private void HandleStationList(IEnumerable<Station> stations, ref string customStationToLoad)
        {

            MainPageViewModel.Instance.Stations = new System.Collections.ObjectModel.ObservableCollection<Station>(stations);

            if (!string.IsNullOrEmpty(customStationToLoad))
            {
                // We can't use ref in a query, so we need to "unref" the custom station string.
                string _uRefStationToLoad = customStationToLoad;

                Station currentStation = (from c in MainPageViewModel.Instance.Stations where c.Name == _uRefStationToLoad select c).FirstOrDefault();

                if (currentStation != null)
                {
                    CoreViewModel.Instance.CurrentStation = currentStation;
                    App.RootFrame.Navigate(new Uri("/Views/StationPlayer.xaml", UriKind.Relative));
                }
            }

            customStationToLoad = string.Empty;

            MainPageViewModel.Instance.IsCurrentlyLoading = false;
        }

        // Used to initialize the station from Xbox Music
        internal static string GetCustomStation()
        {
            string customStationToLoad = string.Empty;
            Page currentPage = (Page)App.RootFrame.Content;

            if (currentPage.NavigationContext.QueryString.ContainsKey("DI.FM"))
            {
                customStationToLoad = currentPage.NavigationContext.QueryString["DI.FM"];
            }

            return customStationToLoad;
        }

        public static void CheckForRatingPrompt()
        {
            RatingPromptValidator counter = Serialize.Open<RatingPromptValidator>("ratingcounter");
            if (counter.ShouldShowPrompt)
            {
                if (!counter.AlreadyRated)
                {
                    counter.CurrentLaunchCount = 0;
                    counter.ShouldShowPrompt = false;

                    MessageBoxResult promptResult = MessageBoxResult.No;
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            promptResult = MessageBox.Show("I like working on Beem. You like using Beem. Want to help me out and rate the app?",
                                "Beem", MessageBoxButton.OKCancel);

                            if (promptResult == MessageBoxResult.OK)
                            {
                                counter.AlreadyRated = true;

                                MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
                                reviewTask.Show();
                            }
                        });
                }
            }
            else
            {
                counter.CurrentLaunchCount++;
                if (counter.CurrentLaunchCount == 5)
                {
                    counter.ShouldShowPrompt = true;
                }
            }

            // All conditions considered, update the rating container.
            Serialize.Save<RatingPromptValidator>("ratingcounter", counter);
        }
    }
}
