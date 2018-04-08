using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
namespace AuthenticationService.Models
{
    public class SSO_Redirect
    {
            public string SSO_URL
            {
                get{
                   return $"{DataObjects.Constants.EVE_AUTH_URL}/?response_type=code&redirect_uri={DataObjects.Constants.REDIRECT_URL}&client_id={DataObjects.Constants.CLIENT_ID}&scope={DataObjects.Constants.SCOPE}";
                }
            }
      
    }

}
