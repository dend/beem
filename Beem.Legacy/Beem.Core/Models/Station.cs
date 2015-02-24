using System;
using System.Windows;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Beem.Core.Models
{
    public class Station : INotifyPropertyChanged
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string JSONID { get; set; }
        public string PremiumLocation { get; set; }

        private ObservableCollection<Track> _trackList;
        [XmlIgnore()]
        public ObservableCollection<Track> TrackList
        {
            get
            {
                return _trackList;
            }
            set
            {
                if (_trackList != value)
                {
                    _trackList = value;
                    NotifyPropertyChanged("TrackList");
                }
            }
        }

        private Track _nowPlaying;
        public Track NowPlaying
        {
            get
            {
                return _nowPlaying;
            }
            set
            {
                if (_nowPlaying != value)
                {
                    _nowPlaying = value;
                    NotifyPropertyChanged("NowPlaying");
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
