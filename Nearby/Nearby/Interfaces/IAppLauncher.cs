using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.Interfaces
{
    public interface IAppLauncher
    {
        bool OpenNativeMaps(double lat, double longitude,string place);
        
        bool SendTweet(string tweet);
    }
}
