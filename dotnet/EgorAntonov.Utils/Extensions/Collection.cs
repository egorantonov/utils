using System.Collections.Specialized;
using System.Web;

namespace EgorAntonov.Utils.Extensions
{
    public static class Collection
    {
        /// <summary>
        /// Returns empty IEnumerable if collection collection is null
        /// </summary>
        /// <typeparam name="T">Type of elements</typeparam>
        /// <param name="collection">Source collection</param>
        /// <returns>Original collection as IEnumerable if not null or empty collection</returns>
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T>? collection)
        {
            return collection ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Returns true if IEnumerable is null or empty
        /// </summary>
        /// <typeparam name="T">Type of elements</typeparam>
        /// <param name="collection">Source collection</param>
        /// <returns>True if IEnumerable is null or empty</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Creates query string from NameValueCollection
        /// </summary>
        /// <param name="collection">Source collection</param>
        /// <param name="pairSeparator">Pair separator</param>
        /// <param name="keyValueSeparator">Key-Value separator</param>
        /// <param name="full">Adds '?' symbol to start of the query</param>
        /// <returns>Query string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string CreateQuery(
            this NameValueCollection? collection,
            string pairSeparator = "&",
            string keyValueSeparator = "=",
            bool full = true)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            var keyValues = collection.AllKeys.Where(key => !string.IsNullOrWhiteSpace(key))
                .SelectMany(key => collection.GetValues(key)?
                    .Where(value => !string.IsNullOrWhiteSpace(value))
                    .Select(value => $"{key}{keyValueSeparator}{HttpUtility.JavaScriptStringEncode(value)}")
                 ?? Enumerable.Empty<string>());

            var query = string.Join(pairSeparator, keyValues);

            return string.IsNullOrWhiteSpace(query) 
                ? string.Empty 
                : $"{(full ? "?" : string.Empty)}{query}";
        }

        /// <summary>
        /// Creates query string from IDictionary
        /// </summary>
        /// <param name="collection">Source collection</param>
        /// <param name="pairSeparator">Pair separator</param>
        /// <param name="keyValueSeparator">Key-Value separator</param>
        /// <param name="full">Adds '?' symbol to start of the query</param>
        /// <returns>Query string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string CreateQuery(
            this IDictionary<string, string> collection,
            string pairSeparator = "&",
            string keyValueSeparator = "=",
            bool full = true)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            var keyValues = collection
                .Where(pair => !string.IsNullOrWhiteSpace(pair.Key) && !string.IsNullOrWhiteSpace(pair.Value))
                .Select(pair => $"{pair.Key}{keyValueSeparator}{HttpUtility.JavaScriptStringEncode(pair.Value)}");

            var query = string.Join(pairSeparator, keyValues);

            return string.IsNullOrWhiteSpace(query)
                ? string.Empty
                : $"{(full ? "?" : string.Empty)}{query}";
        }
    }
}