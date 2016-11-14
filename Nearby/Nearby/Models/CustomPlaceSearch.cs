using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.Models
{
    public class CustomPlaceSearch
    {
        public string Tags { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string PlaceId { get; set; }
        public double CoOrdinatesLat { get; set; }
        public double CoOrdinatesLng { get; set; }
    }
}
