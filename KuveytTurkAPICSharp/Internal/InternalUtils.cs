using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuveytTurkAPICSharp.Internal
{
    internal static class Utils
    {
        /// <summary>
        /// GetUrl
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="rest"></param>
        /// <returns></returns>
        internal static string GetUrl(string baseUrl, string rest)
        {
            var result = new StringBuilder(baseUrl.TrimEnd('/'));

            result.Append('/');
            result.Append(rest);
            return result.ToString();
        }
        internal static Uri GetUri(string baseUrl, string rest)
        {
            return new Uri(Utils.GetUrl(baseUrl, rest));
        }
    }
}
