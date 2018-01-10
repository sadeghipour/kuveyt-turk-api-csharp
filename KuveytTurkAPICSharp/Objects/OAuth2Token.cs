using KuveytTurkAPICSharp.Internal;


namespace KuveytTurkAPICSharp.Objects
{
    /// <summary>
    /// The OAuth2 token, which is usually used for Application-only authentication.
    /// </summary>
    public class OAuth2Token : TokenBase
    {
        /// <summary>
        /// Initializes a new instance of the OAuth2Token class.
        /// </summary>
        public OAuth2Token() { }

        /// <summary>
        /// Initializes a new instance of the OAuth2Token class with a specified token.
        /// </summary>
        /// <param name="e">The token.</param>
        public OAuth2Token(OAuth2Token e)
            : this()
        {
            this.AccessToken = e.AccessToken;
        }
        /// <summary>
        /// Create Authorization Header
        /// </summary>
        /// <returns></returns>
        public string CreateAuthorizationHeader()
        {
            return "Bearer " + this.AccessToken;
        }
        /// <summary>
        /// Makes an instance of OAuth2Tokens.
        /// </summary>
        /// <param name="consumerKey">The consumer key.</param>
        /// <param name="consumerSecret">The consumer secret.</param>
        /// <param name="bearer">The bearer token.</param>
        public static OAuth2Token Create(string consumerKey, string consumerSecret, string bearer)
        {
            return new OAuth2Token()
            {
                AccessToken = bearer
            };
        }
    }
}
