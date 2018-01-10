using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KuveytTurkAPICSharp.Utility;
using KuveytTurkAPICSharp.Objects;
using KuveytTurkAPICSharp.Types;
using System.Net;
using RestSharp;

namespace KuveytTurkAPICSharp.Internal
{
    internal static partial class Request
    {
        internal const string ContentType = "content-type";
        internal const string JsonContentType = "application/json";
        internal const string UrlEncodedContentType = "application/x-www-form-urlencoded";
        internal const string RefreshRequired = "refresh required";
        internal const string TokenIsExpired = "token is expired";
        internal const string ClientCredentials = "client_credentials";
        internal const string AuthorizationCode = "authorization_code";
        internal const string RefreshToken = "refresh_token";


        private static string CreateQueryStringBase(IEnumerable<KeyValuePair<string, string>> prm)
        {
            return prm.Select(x => UrlEncode(x.Key) + "=" + UrlEncode(x.Value)).JoinToString("&");
        }

        public static string CreateQueryString(IEnumerable<KeyValuePair<string, string>> prm)
        {
            if (prm == null)
                return string.Empty;
            return CreateQueryStringBase(prm.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)));
        }
        /// <summary>
        /// Generates the parameters.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="token">The token.</param>
        /// <returns>The parameters.</returns>
        internal static IEnumerable<KeyValuePair<string, string>> GenerateClientCredentialParameters(Client client)
        {

            var ret = new Dictionary<string, string>() {
                {"client_id", client.Id},
                {"client_secret", client.Secret},
                {"grant_type", ClientCredentials},
                {"scope", "public"}
            };
            return ret;
        }
        /// <summary>
        /// Generates the parameters.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="token">The token.</param>
        /// <returns>The parameters.</returns>
        internal static IEnumerable<KeyValuePair<string, string>> GenerateAuthorizationCodeParameters(Client client)
        {
            var ret = new Dictionary<string, string>() {
                {"client_id", client.Id},
                {"client_secret",  client.Secret},
                {"grant_type", AuthorizationCode},
                {"redirect_uri", client.RedirectUri},
                {"code", client.AuthorizationCode}
            };
            return ret;
        }
        /// <summary>
        /// Generates the parameters.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="token">The token.</param>
        /// <returns>The parameters.</returns>
        internal static IEnumerable<KeyValuePair<string, string>> GenerateRefreshTokenParameters(Client client, string refreshToken)
        {
            var ret = new Dictionary<string, string>() {
                {"client_id", client.Id},
                {"client_secret",  client.Secret},
                {"grant_type", RefreshToken},
                {"refresh_token", refreshToken}
            };
            return ret;
        }
        /// <summary>
        /// Generates the parameters.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="token">The token.</param>
        /// <returns>The parameters.</returns>
        internal static IEnumerable<KeyValuePair<string, object>> GenerateRequestHeaderParameters(OAuth2Token token, IEnumerable<KeyValuePair<string, object>> parameters, string signature)
        {
            var dic = parameters == null
                ? new Dictionary<string, object>()
                : (parameters as IDictionary<string, object>)
                    ?? parameters.ToDictionary(x => x.Key, x => x.Value); // Check key duplication

            var ret = new Dictionary<string, object>() {
                {RequestHeaderKey.Authorization, "Bearer " + token.AccessToken},
                {RequestHeaderKey.LanguageId, dic.ContainsKey(RequestHeaderKey.LanguageId)?dic[RequestHeaderKey.LanguageId].ToString():"2"},
                {RequestHeaderKey.DeviceId, dic.ContainsKey(RequestHeaderKey.DeviceId)?dic[RequestHeaderKey.DeviceId].ToString():"empty device id"},
                {RequestHeaderKey.Signature, signature},
            };
            return ret;
        }
        /// <summary>
        /// Encodes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The encoded text.</returns>
        internal static string UrlEncode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            return Encoding.UTF8.GetBytes(text)
                .Select(x => x < 0x80 && "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~"
                        .Contains(((char)x).ToString()) ? ((char)x).ToString() : ('%' + x.ToString("X2")))
                .JoinToString();
        }
        /// <summary>
        /// Execute post request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="parameterContentType"></param>
        /// <param name="headerParameters"></param>
        /// <param name="bodyParameters"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static IRestResponse HttpPost(Uri url, string contentType, string parameterContentType, IEnumerable<KeyValuePair<string, object>> headerParameters, IEnumerable<KeyValuePair<string, string>> bodyParameters, ConnectionOptions options)
        {
            return HttpPost(url, contentType, parameterContentType,
                parameterContentType.Equals(UrlEncodedContentType) ? CreateQueryString(bodyParameters) :
                (bodyParameters == null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(bodyParameters))
                        , headerParameters, options);
        }
        /// <summary>
        /// Execute post request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="parameterContentType"></param>
        /// <param name="content"></param>
        /// <param name="headerParameters"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static IRestResponse HttpPost(Uri url, string contentType, string parameterContentType, string content, IEnumerable<KeyValuePair<string, object>> headerParameters, ConnectionOptions options)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader(ContentType, contentType);
            foreach (var item in headerParameters)
            {
                if (item.Value != null && !string.IsNullOrEmpty(item.Value.ToString()))
                    request.AddHeader(item.Key, item.Value.ToString());
            }
            request.AddParameter(parameterContentType, content, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response;
        }
        /// <summary>
        /// Execute get request.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="headerParameters"></param>
        /// <param name="queryParameters"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static IRestResponse HttpGet(Uri url, string contentType, IEnumerable<KeyValuePair<string, object>> headerParameters, IEnumerable<KeyValuePair<string, string>> queryParameters, ConnectionOptions options)
        {
            return HttpGet(url, contentType, headerParameters, CreateQueryString(queryParameters), options);
        }
        /// <summary>
        /// Execute get request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="headerParameters"></param>
        /// <param name="queryString"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static IRestResponse HttpGet(Uri url, string contentType, IEnumerable<KeyValuePair<string, object>> headerParameters, string queryString, ConnectionOptions options)
        {
            var client = new RestClient(url + (string.IsNullOrEmpty(queryString) ? string.Empty : "?" + queryString));
            var request = new RestRequest(Method.GET);
            foreach (var item in headerParameters)
            {
                if (item.Value != null && !string.IsNullOrEmpty(item.Value.ToString()))
                    request.AddHeader(item.Key, item.Value.ToString());
            }
            request.AddHeader(ContentType, contentType);
            IRestResponse response = client.Execute(request);
            return response;
        }

    }
}
