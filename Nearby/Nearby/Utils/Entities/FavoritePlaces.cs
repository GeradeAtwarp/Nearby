using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.Utils.Entities
{
    public class FavoritePlaces : BaseEntity
    {
        public DateTime Created { get; set; }
        public string PlaceName { get; set; }
        public string PlaceId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Vicinity { get; set; }

        public FavoritePlaces()
        { }
    }
}
