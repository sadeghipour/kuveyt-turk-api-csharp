using KuveytTurkAPICSharp.Internal;
using KuveytTurkAPICSharp.Objects;
using KuveytTurkAPICSharp.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KuveytTurkAPICSharp.Internal
{
    class OAuth2
    {
        internal const string ResponseType = "code";
        internal const string ConnectAuthorizePath = "connect/authorize?";
        internal const string ConnectToken = "connect/token";
        internal const string DefaultScopes = "public offline_access";

        /// <summary>
        /// Token object
        /// </summary>
        internal OAuth2Token Token { get; set; }
        /// <summary>
        /// Get Kuveyt Turk Customer Login Uri
        /// </summary>
        /// <param name="options"></param>
        /// <param name="client"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        internal static Uri GetKuveytTurkCustomerLoginUri(ConnectionOptions options, Client client, List<string> scopes)
        {
            Dictionary<string, string> query = new Dictionary<string, string>()
            {
                {"client_id",client.Id },
                {"scope",GetScopeQueryValue(scopes) },
                {"response_type",ResponseType },
                {"redirect_uri",client.RedirectUri }
            };
            if (options == null) options = ConnectionOptions.Default;
            StringBuilder sb = new StringBuilder(Utils.GetUrl(options.IdentityServerUrl, ConnectAuthorizePath));
            sb.Append(Request.CreateQueryString(query));
            return new Uri(sb.ToString());
        }
        /// <summary>
        /// return scopes as string
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns></returns>
        internal static string GetScopeQueryValue(List<string> scopes)
        {
            if (scopes == null)
                scopes = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (var item in scopes)
            {
                sb.Append(item);
                sb.Append(" ");
            }
            sb.Append(DefaultScopes);
            return sb.ToString();
        }
        /// <summary>
        /// return Access Token Uri
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static Uri GetAccessTokenUrl(ConnectionOptions options)
        {
            if (options == null) options = ConnectionOptions.Default;
            return new Uri(Utils.GetUrl(options.IdentityServerUrl, ConnectToken));
        }
        /// <summary>
        /// Get Token With Client Credential
        /// </summary>
        /// <param name="client"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static IRestResponse GetTokenWithClientCredential(Client client, ConnectionOptions options)
        {
            var response = Request.HttpPost(GetAccessTokenUrl(options), Request.UrlEncodedContentType,
                                            Request.UrlEncodedContentType, new Dictionary<string, object>(),
                                            Request.GenerateClientCredentialParameters(client), options);
            return response;
        }
        /// <summary>
        /// Get Token With Authorization Code
        /// </summary>
        /// <param name="client"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static IRestResponse GetTokenWithAuthorizationCode(Client client, ConnectionOptions options)
        {
            var response = Request.HttpPost(GetAccessTokenUrl(options), Request.UrlEncodedContentType,
                                            Request.UrlEncodedContentType, new Dictionary<string, object>(),
                                            Request.GenerateAuthorizationCodeParameters(client), options);
            return response;

        }
    }
}
