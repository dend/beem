using Beem.Core.Models;
using Beem.ViewModels;
using Coding4Fun.Toolkit.Storage;
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
                    Station verifier = (from c in MainPageViewModel.Instance.FavoriteStations where c.Name == name select c).Single();
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

            Station verifier = (from c in MainPageViewModel.Instance.FavoriteStations where c.Name == name select c).Single();
            MainPageViewModel.Instance.FavoriteStations.Remove(verifier);
        }

        public static void Pin(Station station)
        {
            StandardTileData data = new StandardTileData()
            {
                Title = station.Name,
                BackgroundImage = new Uri(station.Image),
                BackContent = station.Description
            };

            Uri shellUri = new Uri("/Views/StationPlayer.xaml?station=" + station.JSONID, UriKind.Relative);
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


        public static void SerializeCurrentStation()
        {
            Serialize.Save("current.xml", CoreViewModel.Instance.CurrentStation);
        }

        public static void DeserializeCurrentStation()
        {
            CoreViewModel.Instance.CurrentStation = new Station();

            var userStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (userStore.FileExists("current.xml"))
            {
                using (var stream = new IsolatedStorageFileStream("current.xml", FileMode.Open, userStore))
                {
                    XmlSerializer serializer = new XmlSerializer(CoreViewModel.Instance.CurrentStation.GetType());
                    CoreViewModel.Instance.CurrentStation = (Station)serializer.Deserialize(stream);
                }
            }
        }

        public static void SerializeFavorites()
        {
            Serialize.Save("fav.xml", MainPageViewModel.Instance.FavoriteStations);
        }
    }
}
