using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.Interfaces
{
    public interface ISharer
    {
        bool SendTweet(string tweet);

        bool PostToFacebook(string PostText);

        bool OpenUserName(string profile);
    }
}
