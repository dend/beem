using Beem.Core.Models;
using Microsoft.Phone.Shell;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Serialization;

namespace Beem.Utility
{
    public static class StationManager
    {
        public static bool CheckIfExists(Station station)
        {
            if (station != null)
            {
                string name = station.Name;

                try
                {
                    Station verifier = (from c in Binder.Instance.FavoriteStations where c.Name == name select c).Single();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return false;
        }

        public static void Remove(Station station)
        {
            string name = station.Name;

            Station verifier = (from c in Binder.Instance.FavoriteStations where c.Name == name select c).Single();
            Binder.Instance.FavoriteStations.Remove(verifier);
        }

        public static void Pin(Station station)
        {
            StandardTileData data = new StandardTileData()
            {
                Title = station.Name,
                BackgroundImage = new Uri(station.Image),
                BackContent = station.Description
            };

            Uri shellUri = new Uri("/StationPlayer.xaml?station=" + station.JSONID, UriKind.Relative);
            try
            {
                ShellTile.Create(shellUri, data);
            }
            catch
            {
                ShellTile tile = (from c in ShellTile.ActiveTiles where c.NavigationUri == shellUri select c).First();
                tile.Update(data);
            }
        }

        private static void Serialize(string fileName, object source)
        {
            var userStore = IsolatedStorageFile.GetUserStoreForApplication();

            using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Create, userStore))
            {
                XmlSerializer serializer = new XmlSerializer(source.GetType());
                serializer.Serialize(stream, source);
            }
        }

        public static void SerializeCurrentStation()
        {
            Serialize("current.xml", Binder.Instance.CurrentStation);
        }

        public static void DeserializeCurrentStation()
        {
            Binder.Instance.CurrentStation = new Station();
            var userStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (userStore.FileExists("current.xml"))
            {
                using (var stream = new IsolatedStorageFileStream("current.xml", FileMode.Open, userStore))
                {
                    XmlSerializer serializer = new XmlSerializer(Binder.Instance.CurrentStation.GetType());
                    Binder.Instance.CurrentStation = (Station)serializer.Deserialize(stream);
                }
            }
        }

        public static void SerializeFavorites()
        {
            Serialize("fav.xml", Binder.Instance.FavoriteStations);
        }

        public static void DeserializeFavorites()
        {
            Binder.Instance.FavoriteStations = new ObservableCollection<Station>();

            var userStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (userStore.FileExists("fav.xml"))
            {
                using (var stream = new IsolatedStorageFileStream("fav.xml", FileMode.Open, userStore))
                {
                    XmlSerializer serializer = new XmlSerializer(Binder.Instance.FavoriteStations.GetType());
                    var stations = (ObservableCollection<Station>)serializer.Deserialize(stream);

                    foreach (Station station in stations)
                    {
                        Binder.Instance.FavoriteStations.Add(station);
                    }
                }
            }
        }
    }
}
