using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;


namespace BasicAuth.Auth
{
    public class Authenticator
    {

        
        private string _clientID;
        private string _clientSecret;
        private string _ssoURL;
        private string _oauthURL;
        private string _redirectURL;
        private string _authScope;
        private string _Token;
        /// <summary>
        /// This class is ment to handle all authentification against the eve ESI API
        /// </summary>
        /// <param name="clientID">Client ID from the eve developers application registration</param>
        /// <param name="clientSecret">Client Secret from the eve developers application registration</param>
        /// <param name="ssoUrl">URL to redirect the user to (login page)</param>
        /// <param name="oauthURL">URL to obtain the access token, once the auth key is recived by the callback after user login</param>
        /// <param name="redirectURL">URL to redirect to</param>
        /// <param name="scope">Permission Scope</param>
        public Authenticator(string clientID, string clientSecret, string ssoUrl, string oauthURL, string redirectURL, string scope)
        {
            _clientID = clientID;
            _clientSecret = clientSecret;
            _ssoURL = ssoUrl;
            _oauthURL = oauthURL;
            _authScope = scope;
            _redirectURL = redirectURL;
        }
        
        /// <summary>
        /// Simple wrapper to redirect to Eve Online SSO login page
        /// </summary>
        public void redirectToLogin()
        {
            string url = $"{_ssoURL}/?response_type=code&redirect_uri={_redirectURL}&client_id={_clientID}&scope={_authScope}";
            System.Diagnostics.Process.Start(url);
        }

        /// <summary>
        /// Delegate to catch the Auth Key by the EVE Login redirect, and obtain the Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string getAuthKey(HttpListenerRequest request)
        {
            System.Diagnostics.Debug.Write($"request recieved{request.Url}");
            
            this.getToken(request.QueryString[0]);
            return $"auth Key recieved: [{request.QueryString[0]}] <p> the resulting Token is : [{_Token}] <p> you may now close this browser tab";
        }


        /// <summary>
        /// Simple Wrapper to obtain an Oauth Token, based on the SSO auth Key
        /// </summary>
        /// <param name="AuthKey"></param>
        /// <returns></returns>
        public void getToken(string AuthKey)
        {

            // now that this we hopefully got an auth code, lets get a token :
            using (WebClient client = new WebClient())
            {
                //build http header
                string authHeader = $"{_clientID}:{_clientSecret}";
                string authHeader_64 = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(authHeader))}";
                client.Headers[HttpRequestHeader.Authorization] = authHeader_64;

                //build post parameters
                System.Collections.Specialized.NameValueCollection postParams = new System.Collections.Specialized.NameValueCollection();
                postParams.Add("grant_type", "authorization_code");
                postParams.Add("code", AuthKey);
                               
                _Token = Encoding.UTF8.GetString(client.UploadValues(_oauthURL, "POST", postParams));
            }
       }
    }
}



