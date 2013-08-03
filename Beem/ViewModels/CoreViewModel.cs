using Beem.Core.Binding;
using Beem.Core.Models;
using Beem.Settings;

namespace Beem.ViewModels
{
    public class CoreViewModel : BindableBase
    {
        static CoreViewModel instance = null;
        static readonly object padlock = new object();

        public CoreViewModel()
        {

        }

        public static CoreViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CoreViewModel();
                    }
                    return instance;
                }
            }
        }

        private DISettingsCache _diSettings;
        public DISettingsCache DISettings
        {
            get
            {
                return _diSettings;
            }
            set
            {
                SetProperty(ref _diSettings, value);
            }
        }

        private BeemSettings _currentAppSettings;
        public BeemSettings CurrentAppSettings
        {
            get
            {
                return _currentAppSettings;
            }
            set
            {
                SetProperty(ref _currentAppSettings, value);
            }
        }

        private Station _currentStation;
        public Station CurrentStation
        {
            get
            {
                return _currentStation;
            }
            set
            {
                SetProperty(ref _currentStation, value);
            }
        }

        private string _microsoftAccountName;
        public string MicrosoftAccountName
        {
            get
            {
                return _microsoftAccountName;
            }
            set
            {
                SetProperty(ref _microsoftAccountName, value);
            }
        }

        private string _microsoftAccountImage;
        public string MicrosoftAccountImage
        {
            get
            {
                return _microsoftAccountImage;
            }
            set
            {
                SetProperty(ref _microsoftAccountImage, value);
            }
        }
    }
}

