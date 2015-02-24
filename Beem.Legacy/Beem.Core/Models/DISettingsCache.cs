using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Beem.Core.Models
{
    public class DISettingsCache : INotifyPropertyChanged
    {
        // Determines whether to enable the premium 
        // streaming. This is also bound to the auth
        // key, which might or might not be wrong
        private bool _isPremiumEnabled;
        public bool IsPremiumEnabled
        {
            get
            {
                return _isPremiumEnabled;
            }
            set
            {
                if (_isPremiumEnabled != value)
                {
                    _isPremiumEnabled = value;
                    NotifyPropertyChanged("IsPremiumEnabled");
                }
            }
        }

        // This is the core premium key that is used to 
        // get the high quality stream.
        private string _premiumKey;
        public string PremiumKey
        {
            get
            {
                return _premiumKey;
            }
            set
            {
                if (_premiumKey != value)
                {
                    _premiumKey = value;
                    NotifyPropertyChanged("PremiumKey");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
