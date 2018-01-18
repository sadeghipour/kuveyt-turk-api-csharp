using KuveytTurkAPICSharp.Utility;
using KuveytTurkAPICSharp.Internal;
using KuveytTurkAPICSharp.Objects;
using KuveytTurkAPICSharp.Types;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace KuveytTurkAPICSharp
{
    public class Process
    {
        /// <summary>
        /// Token object instance
        /// </summary>
        private OAuth2 oAuth2;
        private const string CustomerLoginRequired = "Customer login required!";
        private const string RedirectLogin = "RedirectLogin";
        private const string UnsupportedHTTPMethod = "Unsupported HTTP method!";

        /// <summary>
        /// Construction 
        /// </summary>
        public Process()
        {
            oAuth2 = new OAuth2();
        }
        /// <summary>
        /// Make Request
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="options"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<GenericResponse<Response>> Execute<Response>(Endpoint endpoint, ConnectionOptions options, Client client)
        {
            try
            {
                GenericResponse<Response> returnObject;

                string signature = string.Empty;
                #region AuthorizationFlow
                if (oAuth2.Token == null)
                {
                    if (endpoint.GrantType == GrantType.ClientCredential)
                    {
                        var tokenResonse = GetToken(GrantType.ClientCredential, client, options);
                        if (tokenResonse.StatusCode != HttpStatusCode.OK)
                        {
                            returnObject = JsonConvert.DeserializeObject(tokenResonse.Content, typeof(GenericResponse<Response>)) as GenericResponse<Response>;
                            return returnObject;
                        }
                    }
                    else if (endpoint.GrantType == GrantType.AuthorizationCode)
                    {
                        if (client != null && !string.IsNullOrEmpty(client.AuthorizationCode))
                        {
                            var tokenResonse = GetToken(GrantType.AuthorizationCode, client, options);
                            if (tokenResonse.StatusCode != HttpStatusCode.OK)
                            {
                                returnObject = JsonConvert.DeserializeObject(tokenResonse.Content, typeof(GenericResponse<Response>)) as GenericResponse<Response>;
                                return returnObject;
                            }
                        }
                        else
                        {
                            throw new Exception(CustomerLoginRequired)
                            {
                                Source = RedirectLogin
                            };
                        }
                    }
                }
                #endregion
                #region Executing
                if (endpoint.MethodType == MethodType.Get)
                {
                    returnObject = ExecuteGetRequest<Response>(client, options, endpoint);
                    return returnObject;
                }
                else if (endpoint.MethodType == MethodType.Post)
                {
                    returnObject = ExecutePostRequest<Response>(client, options, endpoint);
                    return returnObject;
                }
                else
                {
                    throw new Exception(UnsupportedHTTPMethod);
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Gets Kuveyt Turk customer login page.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="client"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public static Uri GetKuveytTurkCustomerLoginUri(ConnectionOptions options, Client client, List<string> scopes)
        {
            var uri = OAuth2.GetKuveytTurkCustomerLoginUri(options, client, scopes);
            return uri;
        }
        /// <summary>
        /// Create Kuveyt Turk customer login uri and open on default browser.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="client"></param>
        /// <param name="scopes"></param>
        public static void OpenLoginUI(ConnectionOptions options, Client client, List<string> scopes)
        {
            var uri = OAuth2.GetKuveytTurkCustomerLoginUri(options, client, scopes);
            System.Diagnostics.Process.Start(uri.AbsoluteUri);
        }
        /// <summary>
        /// Refresh the OAuth 2 Bearer Token.
        /// </summary>
        /// <param name="options">The connection options for the request.</param>
        /// <param name="client">The consumer.</param>
        /// <param name="token">The token.</param>
        /// <returns>The tokens.</returns>
        private IRestResponse RefreshAccessToken(ConnectionOptions options, Client client)
        {

            var response = Request.HttpPost(OAuth2.GetAccessTokenUrl(options), Request.JsonContentType,
                                            Request.UrlEncodedContentType, new Dictionary<string, object>(),
                                            Request.GenerateRefreshTokenParameters(client, oAuth2.Token.RefreshToken), options);
            if (response != null && response.StatusCode == HttpStatusCode.OK
                && !String.IsNullOrEmpty(response.Content))
            {
                oAuth2.Token = JsonConvert.DeserializeObject<OAuth2Token>(response.Content);
                oAuth2.Token.GrantType = GrantType.AuthorizationCode;
            }
            return response;
        }
        /// <summary>
        /// Execute post request.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="options"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        private GenericResponse<Response> ExecutePostRequest<Response>(Client client, ConnectionOptions options, Endpoint endpoint)
        {

            IRestResponse response;
            GenericResponse<Response> returnObject;

            response = Request.HttpPost(Utils.GetUri((options ?? ConnectionOptions.Default).ApiUrl, endpoint.Path), Request.JsonContentType,
                                        Request.JsonContentType, Request.GenerateRequestHeaderParameters(oAuth2.Token, endpoint.HeaderParameters, CreateSignature(client, endpoint, MethodType.Post)),
                                        endpoint.BodyParameters, options);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                if (CheckExpireToken(response))
                {
                    var refreshResponse = RefreshAccessToken(options, client);
                    if (refreshResponse.StatusCode == HttpStatusCode.OK)
                        response = Request.HttpPost(Utils.GetUri((options ?? ConnectionOptions.Default).ApiUrl, endpoint.Path), Request.JsonContentType,
                                         Request.JsonContentType, Request.GenerateRequestHeaderParameters(oAuth2.Token, endpoint.HeaderParameters, CreateSignature(client, endpoint, MethodType.Post)),
                                        endpoint.BodyParameters, options);
                    else
                    {
                        returnObject = JsonConvert.DeserializeObject(refreshResponse.Content, typeof(GenericResponse<Response>)) as GenericResponse<Response>;
                        return returnObject;
                    }
                }
            }
            returnObject = JsonConvert.DeserializeObject(response.Content, typeof(GenericResponse<Response>)) as GenericResponse<Response>;
            return returnObject;
        }
        /// <summary>
        /// Execute get request.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="options"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        private GenericResponse<Response> ExecuteGetRequest<Response>(Client client, ConnectionOptions options, Endpoint endpoint)
        {
            IRestResponse response;
            GenericResponse<Response> returnObject;
            response = Request.HttpGet(Utils.GetUri((options ?? ConnectionOptions.Default).ApiUrl, endpoint.Path), Request.JsonContentType,
                                             Request.GenerateRequestHeaderParameters(oAuth2.Token, endpoint.HeaderParameters, CreateSignature(client, endpoint, MethodType.Get)), endpoint.QueryParameters, options);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                if (CheckExpireToken(response))
                {
                    var refreshResponse = RefreshAccessToken(options, client);
                    if (refreshResponse.StatusCode == HttpStatusCode.OK)
                        response = Request.HttpGet(Utils.GetUri((options ?? ConnectionOptions.Default).ApiUrl, endpoint.Path), Request.JsonContentType,
                                         Request.GenerateRequestHeaderParameters(oAuth2.Token, endpoint.HeaderParameters, CreateSignature(client, endpoint, MethodType.Get)), endpoint.QueryParameters, options);

                    else
                    {
                        returnObject = JsonConvert.DeserializeObject(refreshResponse.Content, typeof(GenericResponse<Response>)) as GenericResponse<Response>;
                        return returnObject;
                    }
                }
            }
            returnObject = JsonConvert.DeserializeObject(response.Content, typeof(GenericResponse<Response>)) as GenericResponse<Response>;
            return returnObject;
        }
        /// <summary>
        /// Check token status. You have to refresh token, if the token is expire.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool CheckExpireToken(IRestResponse response)
        {
            if (response.Headers.Any(i => i.Value.ToString().Contains(Request.RefreshRequired))
            || response.Headers.Any(i => i.Value.ToString().Contains(Request.TokenIsExpired))
            || response.Headers.Any(i => i.Value.ToString().ToLower().Contains(Request.TokenRefreshRequired)))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Create signature for adding request header.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="endpoint"></param>
        /// <param name="methodType"></param>
        /// <returns></returns>
        private string CreateSignature(Client client, Endpoint endpoint, MethodType methodType)
        {
            var signature = string.Empty;
            if (!string.IsNullOrEmpty(client.CertificatePrivateKey) && endpoint.GrantType == GrantType.AuthorizationCode)
            {
                if (methodType == MethodType.Post)
                    signature = CertificateUtils.CreateSignature(client.CertificatePrivateKey, oAuth2.Token.AccessToken, endpoint.BodyParameters == null ? null : JsonConvert.SerializeObject(endpoint.BodyParameters));
                else
                    signature = CertificateUtils.CreateSignature(client.CertificatePrivateKey, oAuth2.Token.AccessToken, (endpoint.QueryParameters == null || endpoint.QueryParameters.Count() == 0) ? null : "?" + Request.CreateQueryString(endpoint.QueryParameters));

            }
            return signature;
        }
        /// <summary>
        /// Get the token from Kuveyt Turk identity server.
        /// </summary>
        /// <param name="grantType"></param>
        /// <param name="client"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private IRestResponse GetToken(GrantType grantType, Client client, ConnectionOptions options)
        {
            IRestResponse tokenResonse;

            if (grantType == GrantType.ClientCredential)
                tokenResonse = OAuth2.GetTokenWithClientCredential(client, options);
            else
                tokenResonse = OAuth2.GetTokenWithAuthorizationCode(client, options);

            if (tokenResonse.StatusCode == HttpStatusCode.OK)
            {
                if (!String.IsNullOrEmpty(tokenResonse.Content))
                {
                    oAuth2.Token = JsonConvert.DeserializeObject<OAuth2Token>(tokenResonse.Content);
                    oAuth2.Token.GrantType = grantType;
                }
            }
            return tokenResonse;
        }

    }
}
