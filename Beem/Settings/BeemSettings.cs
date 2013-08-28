using Beem.LastFMClient;
using System;
using System.ComponentModel;
using System.Windows;

namespace Beem.Settings
{
    public class BeemSettings : INotifyPropertyChanged
    {
        public BeemSettings()
        {
            Session = new LastFmSession();
        }

        private bool _canRunUnderLockScreen;
        public bool CanRunUnderLockScreen
        {
            get
            {
                return _canRunUnderLockScreen;
            }
            set
            {
                if (_canRunUnderLockScreen != value)
                {
                    _canRunUnderLockScreen = value;
                    NotifyPropertyChanged("CanRunUnderLockScreen");
                }
            }
        }

        private LastFmSession _session;
        public LastFmSession Session
        {
            get
            {
                return _session;
            }
            set
            {
                if (_session != value)
                {
                    _session = value;
                    NotifyPropertyChanged("Session");
                }
            }
        }

        private bool _scrobbleOnLaunch;
        public bool ScrobbleOnLaunch
        {
            get
            {
                return _scrobbleOnLaunch;
            }
            set
            {
                if (_scrobbleOnLaunch != value)
                {
                    _scrobbleOnLaunch = value;
                    NotifyPropertyChanged("ScrobbleOnLaunch");
                }
            }
        }

        private bool _showRecordingAlert;
        public bool ShowRecordingAlert
        {
            get
            {
                return _showRecordingAlert;
            }
            set
            {
                if (_showRecordingAlert != value)
                {
                    _showRecordingAlert = value;
                    NotifyPropertyChanged("ShowRecordingAlert");
                }
            }
        }

        private bool _firstLaunchFlag;
        public bool FirstLaunchFlag
        {
            get
            {
                return _firstLaunchFlag;
            }
            set
            {
                if (_firstLaunchFlag != value)
                {
                    _firstLaunchFlag = value;
                    NotifyPropertyChanged("FirstLaunchFlag");
                }
            }
        }

        private bool _enableAnalytics;
        public bool EnableAnalytics
        {
            get
            {
                return _enableAnalytics;
            }
            set
            {
                if (_enableAnalytics != value)
                {
                    _enableAnalytics = value;
                    NotifyPropertyChanged("EnableAnalytics");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => 
                { 
                    PropertyChanged(this, new PropertyChangedEventArgs(info)); 
                });
            }
        }
    }
}
