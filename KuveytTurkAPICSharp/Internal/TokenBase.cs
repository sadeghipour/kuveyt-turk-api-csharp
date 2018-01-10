using KuveytTurkAPICSharp.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuveytTurkAPICSharp.Internal
{
    /// <summary>
    /// Represents an OAuth token. This is an <c>abstract</c> class.
    /// </summary>
    public abstract partial class TokenBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenBase"/> class.
        /// </summary>
        protected TokenBase()
        {
        }

        /// <summary>
        /// Gets or sets the grant type.
        /// </summary>
        public GrantType GrantType { get; set; }

        /// <summary>
        /// Gets or sets the remaining time before the media ID expires.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the token access token.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

    }
}
