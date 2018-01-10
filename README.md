## Kuveyt Turk API Banking Platform CSharp SDK
This C# SDK helps .NET developers build .NET Web and desktop applications that integrate with Kuveyt Turk API Banking Platform.

*Note: This sample does not necessarily demonstrate the best use but rather features of using Kuveyt Turk API Banking C# SDK. Always remember to handle exceptions.*

### Dependencies 

This library is dependent on:

- RestSharp (http://restsharp.org/)
- Newtonsoft.Json (http://json.codeplex.com/)

### Client Information For Request
To consume the APIs, you have to register to [Kuveyt Turk API Market](https://developer.kuveytturk.com.tr/#/). Upon, you can get client id and client secret after creating an application on the API Market.
For more information, you can see [authorization guide document](https://developer.kuveytturk.com.tr/#/documentation/general/Authorization%20Guide).
```csharp
public class Client
{
    public string Id { get; set; }
    public string Secret { get; set; }
    public string RedirectUri { get; set; }
    public string AuthorizationCode { get; set; }
    public string CertificatePrivateKey { get; set; }
}
```
### Endpoint Information For Request
You have to set path, method type and grant type information of endpoint for a request. The query and header parameters are optional. You can find more information on  [Kuveyt Turk API Market documantation page](https://developer.kuveytturk.com.tr/#/documentation)
```csharp
public class Endpoint
{
    public string Path { get; set; }
    public MethodType MethodType { get; set; }
    public GrantType GrantType { get; set; }
    public IEnumerable<KeyValuePair<string, string>> QueryParameters { get; set; }
    public IEnumerable<KeyValuePair<string, string>> BodyParameters { get; set; }
    public IEnumerable<KeyValuePair<string, object>> HeaderParameters { get; set; }
}
```

### Connection Option For Request
The default connection values are test addresses of Kuveyt Turk API Banking Platform. You have to change these links before production.
```csharp
public class ConnectionOptions
{
        internal static readonly ConnectionOptions Default = new ConnectionOptions();
        public string ApiUrl { get; set; } = "https://apitest.kuveytturk.com.tr/prep";
        public string IdentityServerUrl { get; set; } = "https://idprep.kuveytturk.com.tr/api";
}
```
### Execute Request
After preparing endpoint, connection option and client information, you are ready to consume the APIs.

```csharp
public async Task<GenericResponse<Response>> Execute<Response>(Endpoint endpoint, ConnectionOptions options, Client client)
```
#### Get access and refresh tokens

The execute method gets access and refresh tokens according to client and connection option information.
```csharp
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
```
#### Create signature
The execute method creates a signature using client keys and endpoint features. And the signature is added to request header.
```csharp
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
```
### Create Kuveyt Turk Customer Login Uri
Kuveyt Turk API Market currently supports two authorization flows:

* The authorization code flow
* Client Authentication flow

For the authorization flow, you need the authorization code to get access and refresh tokens from Kuveyt Turk Identity Server. For the authorization code, the customer has to login to Kuveyt Turk. You can create the login uri according to your application's features with the SDK.
```csharp
public static Uri GetKuveytTurkCustomerLoginUri(ConnectionOptions options, Client client, List<string> scopes)
{
    var uri = OAuth2.GetKuveytTurkCustomerLoginUri(options, client, scopes);
    return uri;
}
```


### Getting Started for Sample Project
Set the appropriate client's features in App_Data/client.json before running the sample.
```csharp
{
  "ClientId": "client Id",
  "ClientSecret": "client secret",
  "RedirectUri": "http://localhost:<port>/api/GetAuthorizationCodeAfterRedirect"
}

```
To call the APIs which require the authorization code flow, you have to add a signature to the request header. Add the public and private keys to App_Data folder. For more information [use the link](https://developer.kuveytturk.com.tr/#/documentation/general/API%20Header%20Parameters).


#### Prepare Client Information For Request

The client and certificate data are loading in Application_Start method in Global.asax

```json
string pemstr = File.ReadAllText(Server.MapPath("~/App_Data/private_key.pem")).Trim();
JObject clientObject = JObject.Parse(File.ReadAllText(Server.MapPath("~/App_Data/client.json")));

Application["Client"] = new Client()
{
    Id = clientObject["ClientId"].ToString(),
    Secret = clientObject["ClientSecret"].ToString(),
    RedirectUri = clientObject["RedirectUri"].ToString(),
    CertificatePrivateKey = pemstr
};

```
#### Open Customer Login Uri
To get authorization code, the customer must login to Kuveyt Turk Identity Server. The sample project opens with the OpenLoginUI action.

```csharp
public ActionResult OpenLoginUI()
{
    ViewBag.Title = "Home Page";
    List<string> scopes = new List<string>()
            {
                "accounts",
                "transfers",
                "loans"
            };
    Client client = System.Web.HttpContext.Current.Application["Client"] as Client;
    var uri = Process.GetKuveytTurkCustomerLoginUri(null, client, scopes);
}
```
#### Get authorization code after redirection
If login process is successful, the authorization code will be sent to redirect uri. The sample application catches the code with GetAuthorizationCodeAfterRedirect.

```csharp
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
```
#### Make a request
The sample application catches all get and post request with default action in values controller. This action prepares parameters and endpoint to execute the request.
```csharp
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
 ```
