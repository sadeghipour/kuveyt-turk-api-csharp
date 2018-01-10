using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuveytTurkAPICSharp.Objects
{
    public class Client
    {
        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Gets or sets the redirect uri
        /// </summary>
        public string RedirectUri { get; set; }
        /// <summary>
        /// Gets or sets the Authorization Code.
        /// </summary>
        public string AuthorizationCode { get; set; }
        /// <summary>
        /// Gets or sets the private key of certificate.
        /// </summary>
        public string CertificatePrivateKey { get; set; }
    }
}
