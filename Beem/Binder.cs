using Beem.Core.Models;
using Beem.Settings;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace Beem
{
    public class Binder : INotifyPropertyChanged
    {
        static Binder instance = null;
        static readonly object padlock = new object();

        public Binder()
        {
            FavoriteStations = new ObservableCollection<Station>();
            Recorded = new ObservableCollection<string>();
            RecordingContents = new MemoryStream();
            Stations = new ObservableCollection<Station>();
            CurrentAppSettings = new BeemSettings();
            DISettings = new DISettingsCache();
        }

        public static Binder Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Binder();
                    }
                    return instance;
                }
            }
        }


        private MemoryStream _recordingContents;
        public MemoryStream RecordingContents 
        {
            get
            {
                return _recordingContents;
            }
            set
            {
                if (_recordingContents != value)
                {
                    _recordingContents = value;
                    NotifyPropertyChanged("RecordingContents");
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
                if (_diSettings != value)
                {
                    _diSettings = value;
                    NotifyPropertyChanged("DISettings");
                }
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
                if (_currentAppSettings != value)
                {
                    _currentAppSettings = value;
                    NotifyPropertyChanged("CurrentAppSettings");
                }
            }
        }

        private int _recordingLength;
        public int RecordingLength
        {
            get
            {
                return _recordingLength;
            }
            set
            {
                if (_recordingLength != value)
                {
                    _recordingLength = value;
                    NotifyPropertyChanged("RecordingLength");
                }
            }
        }

        private int _skyDriveUploadProgress;
        public int SkyDriveUploadProgress
        {
            get
            {
                return _skyDriveUploadProgress;
            }
            set
            {
                if (_skyDriveUploadProgress != value)
                {
                    _skyDriveUploadProgress = value;
                    NotifyPropertyChanged("SkyDriveUploadProgress");
                }
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
                if (_microsoftAccountName != value)
                {
                    _microsoftAccountName = value;
                    NotifyPropertyChanged("MicrosoftAccountName");
                }
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
                if (_microsoftAccountImage != value)
                {
                    _microsoftAccountImage = value;
                    NotifyPropertyChanged("MicrosoftAccountImage");
                }
            }
        }

        private string currentlyUploading;
        public string CurrentlyUploading
        {
            get
            {
                return currentlyUploading;
            }
            set
            {
                if (currentlyUploading != value)
                {
                    currentlyUploading = value;
                    NotifyPropertyChanged("CurrentlyUploading");
                }
            }
        }

        private bool _isRecording;
        public bool IsRecording
        {
            get
            {
                return _isRecording;
            }
            set
            {
                if (_isRecording != value)
                {
                    _isRecording = value;
                    NotifyPropertyChanged("IsRecording");
                }
            }
        }

        private ObservableCollection<string> recorded;
        public ObservableCollection<string> Recorded
        {
            get
            {
                return recorded;
            }
            set
            {
                if (recorded != value)
                {
                    recorded = value;
                    NotifyPropertyChanged("Recorded");
                }
            }
        }

        private ObservableCollection<Station> stations;
        public ObservableCollection<Station> Stations
        {
            get
            {
                return stations;
            }
            set
            {
                if (stations != value)
                {
                    stations = value;
                    NotifyPropertyChanged("Stations");
                }
            }
        }

        private ObservableCollection<Station> favoriteStations;
        public ObservableCollection<Station> FavoriteStations
        {
            get
            {
                return favoriteStations;
            }
            set
            {
                if (favoriteStations != value)
                {
                    favoriteStations = value;
                    NotifyPropertyChanged("FavoriteStations");
                }
            }
        }

        private Station currentStation;
        public Station CurrentStation
        {
            get
            {
                return currentStation;
            }
            set
            {
                if (currentStation != value)
                {
                    currentStation = value;
                    NotifyPropertyChanged("CurrentStation");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { PropertyChanged(this, new PropertyChangedEventArgs(info)); });
            }
        }
    }
}
