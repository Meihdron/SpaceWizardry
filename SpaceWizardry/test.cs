using System;

namespace SpaceWizardry
{
    class test
    {
        const string EveAuthURL = "https://login.eveonline.com/oauth/authorize";
        const string RedirectURL = "http://localhost:50000";
        const string ClientID = "413ec8fc7c8441e49cd354bfc5c588ed";
        const string ClientSecret = "nz8nNSpXgNFOTtIJAHOq5oRXQu2Xq7jm05zOJGVk";
        const string Scope = "characterContactsRead characterContactsWrite";
        const string TokenAuthURL = "https://login.eveonline.com/oauth/token";

        public static void Main(string[] args)
        {
            //this far, this program will do the following :

            //this is just a class to wrap all the boilerplate stuff needed to authenticate against the EVE ESI API
            Auth.Authenticator auth = new Auth.Authenticator(ClientID, ClientSecret, EveAuthURL, TokenAuthURL, RedirectURL, Scope);

            //this is a very very primitive webserver that will listen to the browser redirect
            //After logging in on the eve login site, the browser gets redirected to the redirect URL that is part of the registration of 3rd party apps. 
            //The "webserver" will listen on the given port, and call a method in the authenticator object, which will obtain the actual access token. 
            //for debug porposes this is then returned to the browser so we can see if that actually worked
            Auth.LightweightWebServer ws = new Auth.LightweightWebServer(auth.getAuthKey, RedirectURL +"/");


            //start up the http listener
            ws.Run();

            //spawn a browser window and send the user to the eve login
            auth.redirectToLogin();

            //wait for 2 minutes, so the http listener is running and waiting for the redirect
            System.Threading.Thread.Sleep(120000);

            //cleanup...
            ws.Stop();



            //next steps :
            // - add database project to be able to deploy a database to a local (for the moment) sql server instance
            // - actually get data off the esi api
            // - write the class definitions for the data returned by esi
            // - CRUD code to save the esi data to the local DB
            // - actually do something usefull with all that crap

        }
    }
}
