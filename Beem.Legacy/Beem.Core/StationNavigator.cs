using Beem.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Beem.Core
{
    public static class StationNavigator
    {
        public static Station GetStationByName(string name, IEnumerable<Station> source)
        {
           return (from c in source where c.Name == name select c).FirstOrDefault();
        }

        public static Station GetPreviousStation(string name, List<Station> source)
        {
            Station station = (from c in source where c.Name == name select c).FirstOrDefault();

            if (station != null)
            {
                int index = source.IndexOf(station);
                int previousStationIndex = index - 1;
                if (previousStationIndex >= 0)
                {
                    station = source[index - 1];
                }
            }

            return station;
        }

        public static Station GetNextStation(string name, List<Station> source)
        {
            Station station = (from c in source where c.Name == name select c).FirstOrDefault();

            if (station != null)
            {
                int index = source.IndexOf(station);
                int previousStationIndex = index + 1;
                if (previousStationIndex < source.Count)
                {
                    station = source[index + 1];
                }
            }

            return station;
        }
    }
}
