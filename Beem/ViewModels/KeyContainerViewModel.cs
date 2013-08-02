using Beem.Core.Binding;

namespace Beem.ViewModels
{
    public class KeyContainerViewModel : BindableBase
    {
        static KeyContainerViewModel instance = null;
        static readonly object padlock = new object();

        public KeyContainerViewModel()
        {

        }

        public static KeyContainerViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new KeyContainerViewModel();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Microsoft SkyDrive API Key.
        /// </summary>
        private string _skyDriveKey;
        public string SkyDriveKey
        {
            get
            {
                return _skyDriveKey;
            }
            set
            {
                SetProperty(ref _skyDriveKey, value);
            }
        }

        /// <summary>
        /// Last.FM API Key.
        /// </summary>
        private string _lastFmKey;
        public string LastFmKey
        {
            get
            {
                return _lastFmKey;
            }
            set
            {
                SetProperty(ref _lastFmKey, value);
            }
        }

        /// <summary>
        /// DI.FM Premium Key.
        /// This is not stored in the appby default and should be provided by the user.
        /// In exceptional pre-bundle cases, the key can be specified in APIKeyManifest.xml
        /// </summary>
        private string _diFmKey;
        public string DiFmKey
        {
            get
            {
                return _diFmKey;
            }
            set
            {
                SetProperty(ref _diFmKey, value);
            }
        }

        /// <summary>
        /// Azure Mobile Services key.
        /// </summary>
        private string _zumoKey;
        public string ZumoKey
        {
            get
            {
                return _zumoKey;
            }
            set
            {
                SetProperty(ref _zumoKey, value);
            }
        }
    }
}
