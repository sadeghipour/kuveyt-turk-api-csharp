using KuveytTurkAPICSharp.Types;
using System.Collections.Generic;
namespace KuveytTurkAPICSharp
{
    public class Endpoint
    {
        /// <summary>
        /// Gets or sets the path of endpoint.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method type of endpoint
        /// </summary>
        public MethodType MethodType { get; set; }
        /// <summary>
        /// Gets or sets the HTTP method type of endpoint
        /// </summary>
        public GrantType GrantType { get; set; }

        /// <summary>
        /// Gets or sets the QueryParameters
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> QueryParameters { get; set; }
        /// <summary>
        /// Gets or sets the BodyParameters
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> BodyParameters { get; set; }
        /// <summary>
        /// Gets or sets the HeaderParameters
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> HeaderParameters { get; set; }
    }
}
