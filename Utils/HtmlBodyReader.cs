using System;
using System.Text.RegularExpressions;

namespace MailFormatter.Utils
{
    /// <summary>
    /// Locates real body tags without rewriting Outlook's markup. Comments, quoted
    /// attributes and raw-text elements are skipped. This is not an HTML sanitizer.
    /// </summary>
    internal static class HtmlBodyReader
    {
        private static readonly Regex Attribute = new Regex(
            @"\G\s+(?<name>[^\s=/>]+)(?:\s*=\s*(?:""(?<value>[^""]*)""|'(?<value>[^']*)'|(?<value>[^\s>]+)))?",
            RegexOptions.CultureInvariant, TimeSpan.FromSeconds(1));

        public static bool TryGetRange(string html, out int contentStart, out int contentEnd)
        {
            contentStart = contentEnd = -1;
            int position = 0;
            while ((position = html.IndexOf('<', position)) >= 0)
            {
                if (html.IndexOf("<!--", position, StringComparison.Ordinal) == position)
                {
                    int commentEnd = html.IndexOf("-->", position + 4, StringComparison.Ordinal);
                    if (commentEnd < 0) return false;
                    position = commentEnd + 3;
                    continue;
                }
                int end = FindTagEnd(html, position);
                if (end < 0) return false;
                bool closing = position + 1 < html.Length && html[position + 1] == '/';
                int nameStart = position + (closing ? 2 : 1);
                int nameEnd = nameStart;
                while (nameEnd < end && !char.IsWhiteSpace(html[nameEnd]) && html[nameEnd] != '/') nameEnd++;
                string name = html.Substring(nameStart, nameEnd - nameStart);
                if (name.Equals("body", StringComparison.OrdinalIgnoreCase))
                {
                    if (closing)
                    {
                        contentEnd = position;
                        return contentStart >= 0;
                    }
                    if (contentStart >= 0) return false;
                    contentStart = end + 1;
                }
                position = end + 1;
                if (!closing && IsRawText(name))
                {
                    position = FindRawTextClose(html, name, position);
                    if (position < 0) return false;
                }
            }
            return false;
        }

        public static bool StartsWithBookmark(string body, string bookmark)
        {
            int start = 0;
            while (start < body.Length && char.IsWhiteSpace(body[start])) start++;
            if (start + 2 >= body.Length || body[start] != '<' || char.ToLowerInvariant(body[start + 1]) != 'a' || !char.IsWhiteSpace(body[start + 2]))
                return false;
            int end = FindTagEnd(body, start);
            if (end < 0) return false;
            string attributes = body.Substring(start + 2, end - start - 2);
            foreach (Match match in Attribute.Matches(attributes))
            {
                if (match.Groups["name"].Value.Equals("name", StringComparison.OrdinalIgnoreCase))
                    return match.Groups["value"].Value.Equals(bookmark, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private static int FindTagEnd(string html, int start)
        {
            char quote = '\0';
            for (int index = start + 1; index < html.Length; index++)
            {
                char current = html[index];
                if (quote != '\0')
                {
                    if (current == quote) quote = '\0';
                    continue;
                }
                if (current == '"' || current == '\'') quote = current;
                else if (current == '>') return index;
                else if (current == '<') return -1;
            }
            return -1;
        }

        private static bool IsRawText(string name)
        {
            return name.Equals("script", StringComparison.OrdinalIgnoreCase) || name.Equals("style", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("textarea", StringComparison.OrdinalIgnoreCase) || name.Equals("title", StringComparison.OrdinalIgnoreCase);
        }

        private static int FindRawTextClose(string html, string name, int position)
        {
            string closing = "</" + name;
            while ((position = html.IndexOf(closing, position, StringComparison.OrdinalIgnoreCase)) >= 0)
            {
                int afterName = position + closing.Length;
                if (afterName < html.Length && (html[afterName] == '>' || char.IsWhiteSpace(html[afterName]))) return position;
                position = afterName;
            }
            return -1;
        }
    }
}
