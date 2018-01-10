using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace KuveytTurkAPICSharp.Utility
{
    internal static class EnumerableUtils
    {
        /// <summary>
        /// Join To String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static string JoinToString<T>(this IEnumerable<T> source)
        {
            var sb = new StringBuilder();
            foreach (var x in source)
            {
                var s = x?.ToString();
                if (s != null) sb.Append(s);
            }
            return sb.ToString();
        }

        internal static string JoinToString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source.Select(x => x.ToString()).ToArray());
        }
    }
}
