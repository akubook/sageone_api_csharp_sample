﻿using System;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Text;
using System.IO;

namespace Sage_One_Authorisation_Client
{
    public class SageOneOAuth
    {
        private const string AUTHORIZE_URL      = "https://www.sageone.com/oauth2/auth";        // Authorisation URL
        private const string ACCESS_TOKEN_URL   = "https://api.sageone.com/oauth2/token";       // Acess Token URL
        private const string CALLBACK_URL       = "http://localhost:59793/callback.aspx";       // Call back URL - this should match the Callback URL reigstered against your application on https://developers.sageone.com/

        private string _clientID                = "YOUR_CLIENT_ID";                             // Client ID - this should match the Client ID reigstered against your application on https://developers.sageone.com/
        private string _clientSSecret           = "YOUR_CLIENT_SECRET";                         // Client Secret - this should match the Client Secret reigstered against your application on https://developers.sageone.com/
        private string _signingSecret           = "YOUR_SIGNING_SECRET";                        // Signing Secret - this should match the Signing Secret reigstered against your application on https://developers.sageone.com/
    
        private string _token = "";
        private string _code = "";

        #region Properties

        public string SigningSecret
        {
            get 
            {
                return _signingSecret;
            }
        }

        public string AccessTokenURL
        {
            get
            {
                return ACCESS_TOKEN_URL;

            }
        }

        public string AccessTokenPostData
        { 
            get
            {
                return string.Format("client_id={0}&client_secret={1}&code={2}&grant_type=authorization_code&redirect_uri={3}", this.ClientID, this.ClientSecret, _code, HttpUtility.UrlEncode(CALLBACK_URL));
            }
        }

        public string AuthorizationURL
        {
            get
            {
                return string.Format("{0}?response_type=code&client_id={1}&redirect_uri={2}&scope=full_access", AUTHORIZE_URL, this.ClientID, HttpUtility.UrlEncode(CALLBACK_URL)); 
            }
        }

        public string ClientID
        {
            get
            {
                return _clientID;
            }
        }

        public string ClientSecret 
        {
            get 
            {           
                return _clientSSecret;
            }
        }

        public string Token 
        {   
            get 
            { 
                return _token; 
            } 
            set 
            { 
                _token = value; 
            } 
        }

        #endregion

        /// <summary>
        /// Exchange the authorisation code for an access token.
        /// </summary>
        /// <param name="code">The code supplied by Sage One's authorization page following the callback.</param>
        public void GetAccessToken( string code )
        {
            SageOneWebRequest request = new SageOneWebRequest();

            _code = code;

            string postData = AccessTokenPostData;
            string response = request.PostData(AccessTokenURL, postData, "", "");
            
            if (response.Length > 0)
            {
                JObject jObject = JObject.Parse(response);
                string access_token = (string) jObject["access_token"];

                if (access_token != null)
                {
                    this.Token = access_token;
                }
            }
        }
     }
}