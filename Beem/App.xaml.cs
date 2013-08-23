using Beem.Core.Models;
using Beem.LastFMClient;
using Beem.Utility;
using Beem.ViewModels;
using Coding4Fun.Toolkit.Storage;
using Microsoft.Live;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using System.Windows;
using System.Windows.Navigation;

namespace Beem
{
    public partial class App : Application
    {
        public static PhoneApplicationFrame RootFrame { get; private set; }

        public static LiveConnectClient MicrosoftAccountClient;
        public static LiveConnectSession MicrosoftAccountSession;
        public static LastFmClient LFMClient;

        public static MobileServiceClient AzureClient;

        public App()
        {
            UnhandledException += Application_UnhandledException;

            InitializeComponent();

            InitializePhoneApplication();

            LFMClient = new LastFmClient();
        }

        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            // Attempt to open the DI.FM settings.
            CoreViewModel.Instance.DISettings = Serialize.Open<DISettingsCache>("di.xml");

            // Get the API keys from the internal XML key storage.
            CoreViewModel.Instance.ApiKeys = SerializationHelper.GetKeys("APIKeyManifest.xml");

            // Initialize the Azure Mobile Services client to pull the station list.
            AzureClient = new MobileServiceClient(CoreViewModel.Instance.ApiKeys.ZumoUrl,
            CoreViewModel.Instance.ApiKeys.ZumoKey);

            AppLoaderAgent.AttemptFirstRun();
        }

        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            Utility.StationManager.DeserializeCurrentStation();
        }

        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            if (CoreViewModel.Instance.CurrentStation != null)
                Utility.StationManager.SerializeCurrentStation();

            PlaybackPageViewModel.Instance.IsRecording = false;
        }

        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            throw e.ExceptionObject;
        }

        public static void MicrosoftAccount_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e.Status == Microsoft.Live.LiveConnectSessionStatus.Connected)
            {
                MicrosoftAccountClient = new LiveConnectClient(e.Session);
                MicrosoftAccountSession = e.Session;
                MicrosoftAccountClient.GetCompleted += client_GetCompleted;
                MicrosoftAccountClient.GetAsync("me", null);
            }
            else
            {
                CoreViewModel.Instance.MicrosoftAccountImage = "/Images/stock_user.png";
                CoreViewModel.Instance.MicrosoftAccountName = "no Microsoft Account connected";
                MicrosoftAccountClient = null;
            }
        }


        static void client_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    if (e.Result.ContainsKey("first_name") && e.Result.ContainsKey("last_name"))
                    {
                        if (e.Result["first_name"] != null && e.Result["last_name"] != null)
                        {
                            CoreViewModel.Instance.MicrosoftAccountName = e.Result["first_name"].ToString() + " " + e.Result["last_name"].ToString();
                            MicrosoftAccountClient.GetAsync("me/picture", null);
                        }
                    }
                    else if (e.Result.ContainsKey("location"))
                    {
                        CoreViewModel.Instance.MicrosoftAccountImage = e.Result["location"].ToString();
                    }
                }
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}