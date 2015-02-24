using Beem.Core.Models;
using System;
using System.Windows.Media.Imaging;

namespace Beem.Utility
{
    public static class FavoriteImageSetter
    {
        public static BitmapImage GetImage(Station station)
        {
            if (Utility.StationManager.CheckIfExists(station))
                return new BitmapImage(new Uri("/Images/appbar.heart.png", UriKind.Relative));
            else
                return new BitmapImage(new Uri("/Images/appbar.heart.outline.png", UriKind.Relative));
        }
    }
}
