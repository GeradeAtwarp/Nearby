using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.Utils.Entities
{
    public class UserPreferences : BaseEntity
    {
        public DateTime Created { get; set; }
        public bool IsManualLocationEnabled { get; set; }
        public string CustomLatitude { get; set; }
        public string CustomLongitude { get; set; }

        public UserPreferences()
        { }
    }
}
