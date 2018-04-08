using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.DataObjects
{
    public static class Constants
    {
        
        public const string EVE_AUTH_URL = "https://login.eveonline.com/oauth/authorize";
        public const string REDIRECT_URL = "http://localhost:50000/api/SSO/Callback";

        public const string CLIENT_ID = "413ec8fc7c8441e49cd354bfc5c588ed";
        public const string CLIENT_SECRET = "nz8nNSpXgNFOTtIJAHOq5oRXQu2Xq7jm05zOJGVk";
        public const string SCOPE = "characterContactsRead characterContactsWrite";
        public const string TOKEN_AUTH_URL = "https://login.eveonline.com/oauth/token";
        public const string CONNECTION_STRING = "Data Source=(local)\\SQL2017;Initial Catalog=SpaceWizzards;Integrated Security=False;User=Test;Password=test;MultipleActiveResultSets=True";
     }
    
}
