using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace  UnityExtension
{
    public static class StringFormat
    {
        public static string FormatByDictionary(this string format, IDictionary<string, object> values)
        {
            var matches = Regex.Matches(format, @"\{(.+?)\}");
            List<string> words = (from Match match in matches select match.Groups[1].Value).ToList();

            foreach (var word in words)
            {
                if (!values.ContainsKey(word))
                {
                    return null;
                }
            }

            return words.Aggregate(
                format,
                (current, key) =>
                {
                    int colonIndex = key.IndexOf(':');
                    return current.Replace(
                        "{" + key + "}",
                        colonIndex > 0
                            ? string.Format("{0:" + key.Substring(colonIndex + 1) + "}", values[key.Substring(0, colonIndex)])
                            : values[key] == null ? string.Empty : values[key].ToString());
                });
        }
    }
}