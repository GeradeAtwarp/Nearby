using Amazon;
using Amazon.CognitoIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nearby.Helpers
{
    public class AWSUtils
    {
        private static CognitoAWSCredentials _credentials;

        public static CognitoAWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                    _credentials = new CognitoAWSCredentials("us-west-2:79d0b137-d1a9-4ca1-b4aa-9e8f4347ca4b", RegionEndpoint.USWest2);

                return _credentials;
            }
        }
    }
}
