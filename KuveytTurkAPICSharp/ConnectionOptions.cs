namespace KuveytTurkAPICSharp
{
    public class ConnectionOptions
    {
        internal static readonly ConnectionOptions Default = new ConnectionOptions();

        /// <summary>
        /// Gets or sets the URL of REST API.
        /// <para>Default: <c>"https://apitest.kuveytturk.com.tr/prep"</c></para>
        /// </summary>
        public string ApiUrl { get; set; } = "https://apitest.kuveytturk.com.tr/prep";

        /// <summary>
        /// Gets or sets the URL of identity Server API.
        /// <para>Default: <c>"https://idprep.kuveytturk.com.tr/api"</c></para>
        /// </summary>
        public string IdentityServerUrl { get; set; } = "https://idprep.kuveytturk.com.tr/api";


    }
}
