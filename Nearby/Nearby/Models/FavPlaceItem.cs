using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nearby.Models
{
    public class FavPlaceItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PlaceID { get; set; }
        public string SavedOn { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Vicinity { get; set; }
        public double DistanceFromCurrentLocation { get; set; }
    }
}
