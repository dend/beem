using Beem.Core.Binding;
using Beem.Core.Models;
using System.Collections.ObjectModel;

namespace Beem.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        static MainPageViewModel instance = null;
        static readonly object padlock = new object();

        public MainPageViewModel()
        {

        }

        public static MainPageViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MainPageViewModel();
                    }
                    return instance;
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
                SetProperty(ref _skyDriveUploadProgress, value);
            }
        }


        private string _currentlyUploading;
        public string CurrentlyUploading
        {
            get
            {
                return _currentlyUploading;
            }
            set
            {
                SetProperty(ref _currentlyUploading, value);
            }
        }

        private ObservableCollection<string> _recorded;
        public ObservableCollection<string> Recorded
        {
            get
            {
                return _recorded;
            }
            set
            {
                SetProperty(ref _recorded, value);
            }
        }

        private ObservableCollection<Station> _stations;
        public ObservableCollection<Station> Stations
        {
            get
            {
                return _stations;
            }
            set
            {
                SetProperty(ref _stations, value);
            }
        }

        private ObservableCollection<Station> _favoriteStations;
        public ObservableCollection<Station> FavoriteStations
        {
            get
            {
                return _favoriteStations;
            }
            set
            {
                SetProperty(ref _favoriteStations, value);
            }
        }
    }
}
