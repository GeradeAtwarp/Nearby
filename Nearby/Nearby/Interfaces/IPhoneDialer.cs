using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.Interfaces
{
    public interface IPhoneDialer
    {
        bool LaunchCall(string tel);
    }
}
