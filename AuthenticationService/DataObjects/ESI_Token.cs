using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
namespace AuthenticationService.DataObjects

{
    public class ESI_Token : IDataObject
    {
       private const bool _isPersistable = true;


        private string _access_token;
        private string _token_type;
        private string _exipres_in;
        private string _refresh_token;
        private int _tokenID;
        private int _characterID;
        private string _characterOwner;

        public bool isPersistable
        {
            get
            {
                return _isPersistable;
            }
        }

        /// <summary>
        /// create new Token with using the supplied auth key
        /// </summary>
        public ESI_Token(string AuthKey){
            try
            {
                getToken(AuthKey);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                saveObject();
            }catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }
      }
        /// <summary>
        /// load a saved token by the primary key
        /// </summary>
        /// <param name="TokenID"></param>
        public ESI_Token(int TokenID)
        {
            this._tokenID = TokenID;
            try
            {
                loadObject();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void getToken(string AuthKey)
        {
            //using (WebClient client = new WebClient())
            using (HttpClient client= new HttpClient())
            {
                //build http header
                string authHeader = $"{Constants.CLIENT_ID}:{Constants.CLIENT_SECRET}";
                string authHeader_64 = $"{Convert.ToBase64String(Encoding.UTF8.GetBytes(authHeader))}";
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader_64);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //build post parameters
                var postParams = new System.Collections.Generic.Dictionary<string, string>();
                postParams.Add("grant_type", "authorization_code");
                postParams.Add("code", AuthKey);

                try {
                    //JObject jsonObj = JObject.Parse(Encoding.UTF8.GetString(client.UploadValues(Constants.TOKEN_AUTH_URL, "POST", postParams)));
                    var result = client.PostAsync(Constants.TOKEN_AUTH_URL, new FormUrlEncodedContent(postParams)).Result;
                    if(result.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception($"reading Token failed with statuscode {result.StatusCode.ToString()}");
                    }
                    JObject jsonObj =  JObject.Parse(result.Content.ReadAsStringAsync().Result);
                    _access_token = (string)jsonObj.SelectToken("access_token");
                    _token_type = (string)jsonObj.SelectToken("token_type");
                    _exipres_in = (string)jsonObj.SelectToken("exipres_in");
                    _refresh_token = (string)jsonObj.SelectToken("refresh_token");
                    validateToken();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool validateToken()
        {
            _characterID = -1; // needs to be retrieved by https://login.eveonline.com/oauth/verify
            using (HttpClient client = new HttpClient())
            {
                
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _access_token);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync("https://login.eveonline.com/oauth/verify").Result;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    refreshToken();
                    response = client.GetAsync("https://login.eveonline.com/oauth/verify").Result;
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return false;
                    }
                }
                
                JObject jsonObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                _characterID = (int)jsonObj.SelectToken("CharacterID");
                _characterOwner = (string)jsonObj.SelectToken("CharacterOwnerHash");
            }
            return true;
        }

        public void refreshToken()
        {
            using (HttpClient client = new HttpClient())
            {
                //build http header
                string authHeader = $"{Constants.CLIENT_ID}:{Constants.CLIENT_SECRET}";
                string authHeader_64 = $"{Convert.ToBase64String(Encoding.UTF8.GetBytes(authHeader))}";
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader_64);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //build post parameters
                var postParams = new System.Collections.Generic.Dictionary<string, string>();
                postParams.Add("grant_type", "refresh_token");
                postParams.Add("refresh_token", _refresh_token);

                try
                {
                    //JObject jsonObj = JObject.Parse(Encoding.UTF8.GetString(client.UploadValues(Constants.TOKEN_AUTH_URL, "POST", postParams)));
                    var result = client.PostAsync(Constants.TOKEN_AUTH_URL, new FormUrlEncodedContent(postParams)).Result;
                    if (result.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception($"reading Token failed with statuscode {result.StatusCode.ToString()}");
                    }
                    JObject jsonObj = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                    _access_token = (string)jsonObj.SelectToken("access_token");
                    _token_type = (string)jsonObj.SelectToken("token_type");
                    _exipres_in = (string)jsonObj.SelectToken("exipres_in");
                    _refresh_token = (string)jsonObj.SelectToken("refresh_token");
                    validateToken();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            }

        public void saveObject()
        {
            if (!_isPersistable)
            {
                throw new Exception("This Object can not be persisted");
            }


            using (SqlConnection con = new SqlConnection(Constants.CONNECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("exec auth.saveToken @tokenID,@characterID, @AccessToken, @TokenType, @refreshToken");
                cmd.Parameters.Add(new SqlParameter("TokenID", _tokenID));
                cmd.Parameters.Add(new SqlParameter("AccessToken", _access_token));
                cmd.Parameters.Add(new SqlParameter("TokenType", _token_type));
                cmd.Parameters.Add(new SqlParameter("refreshToken", _refresh_token));
                cmd.Parameters.Add(new SqlParameter("characterID", _characterID));
                SqlParameter outParam = new SqlParameter("retVal", SqlDbType.Int);
                outParam.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(outParam);

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                _tokenID = (int) outParam.Value;
            };
        }
        public void loadObject() {
            using (SqlConnection con = new SqlConnection(Constants.CONNECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("exec auth.LoadToken @tokenID");
                cmd.Parameters.Add(new SqlParameter("TokenID", _tokenID));
                
                cmd.Connection = con;
                con.Open();
                SqlDataReader r =  cmd.ExecuteReader();
                if (!r.HasRows) {
                    throw new Exception($"could not find token by ID ({_tokenID})");
                }
                r.Read();
                
                this._access_token = r.GetString(1);
                this._token_type = r.GetString(2); 
                this._refresh_token = r.GetString(3);
                if(!r.IsDBNull(4))
                {
                    this._characterID = r.GetInt32(4);
                }
                r.Close();
            };
        }
    }
}



