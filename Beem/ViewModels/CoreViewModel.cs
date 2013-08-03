using Beem.Core.Binding;
using Beem.Core.Models;
using Beem.Settings;

namespace Beem.ViewModels
{
    public class CoreViewModel : BindableBase
    {
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
    }
}

