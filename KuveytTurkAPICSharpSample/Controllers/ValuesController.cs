using KuveytTurkAPICSharp;
using KuveytTurkAPICSharp.Objects;
using KuveytTurkAPICSharp.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;

namespace KuveytTurkAPICSharpSample.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpGet]
        [ActionName("Default")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return await Execute(MethodType.Get);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
        [HttpPost]
        [ActionName("Default")]
        public async Task<IHttpActionResult> Post()
        {
            try
            {
                return await Execute(MethodType.Post);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
        private async Task<IHttpActionResult> Execute(MethodType methodType)
        {
            GenericResponse<object> response;
            #region SetQueryAndBodyParameters
            IEnumerable<KeyValuePair<string, string>> queryParameters = null;
            IEnumerable<KeyValuePair<string, string>> bodyParameters = null;

            if (methodType == MethodType.Get)
                queryParameters = Request.GetQueryNameValuePairs();
            else
            {
                Stream req = System.Web.HttpContext.Current.Request.InputStream;
                req.Seek(0, System.IO.SeekOrigin.Begin);
                string json = new StreamReader(req).ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                    bodyParameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            #endregion
            #region SetHeaderParameters
            var headerParams = new Dictionary<string, object>();
            if (Request.Headers.Contains("LanguageId"))
                headerParams.Add("LanguageId", Request.Headers.GetValues("LanguageId").First());
            if (Request.Headers.Contains("DeviceId"))
                headerParams.Add("DeviceId", Request.Headers.GetValues("DeviceId").First());

            #endregion
            #region PrepareEndpoint
            Client client = System.Web.HttpContext.Current.Application["Client"] as Client;
            Endpoint endpoint = new Endpoint()
            {
                Path = Request.RequestUri.AbsolutePath.Remove(0, 1),
                MethodType = methodType,
                GrantType = string.IsNullOrEmpty(client.AuthorizationCode) ? GrantType.ClientCredential : GrantType.AuthorizationCode,
                QueryParameters = queryParameters,
                BodyParameters = bodyParameters,
                HeaderParameters = headerParams
            };
            #endregion

            KuveytTurkAPICSharp.Process sdk = System.Web.HttpContext.Current.Application["SDKInstance"] as Process;
            response = await sdk.Execute<object>(endpoint, null, client);
            System.Web.HttpContext.Current.Application["SDKInstance"] = sdk;


            return Json(response, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        [HttpGet]
        [Route("api/GetAuthorizationCodeAfterRedirect")]
        public HttpResponseMessage GetAuthorizationCodeAfterRedirect(string code)
        {
            Client client = System.Web.HttpContext.Current.Application["Client"] as Client;
            client.AuthorizationCode = code;
            System.Web.HttpContext.Current.Application["Client"] = client;
            //clear token and get a new one with new auth code.
            KuveytTurkAPICSharp.Process sdk = System.Web.HttpContext.Current.Application["SDKInstance"] as Process;
            sdk = new Process();
            System.Web.HttpContext.Current.Application["SDKInstance"] = sdk;


            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri("http://localhost:84");
            return response;

        }

    }
}
