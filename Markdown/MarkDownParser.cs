using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Markdown
{
    public class MarkDownParser
    {
        public static List<IMarkUpElement> Parse(string str)
        {
            bool successMatch;
            var parsedLexem = new List<IMarkUpElement>();
            var lastSuccessMatchIndex = 0;
            var expression = new Regex(BuildRegExForTags(MarkupTags.GetAllTags()));
            do
            {
                var match = expression.Match(str, lastSuccessMatchIndex);
                successMatch = match.Success;
                if (!successMatch) continue;
                parsedLexem.Add(new Text(str.Substring(lastSuccessMatchIndex, match.Index + match.Groups[1].Length - lastSuccessMatchIndex)));
                switch (match.Groups[2].Value)
                {
                    case "_":
                        parsedLexem.Add(MarkupTags.Em);
                        break;
                    case "__":
                        parsedLexem.Add(MarkupTags.Strong);
                        break;
                    case "`":
                        parsedLexem.Add(MarkupTags.Code);
                        break;
                }
                lastSuccessMatchIndex = match.Index + match.Length - match.Groups[3].Length;
            } while (successMatch);
            if (lastSuccessMatchIndex < str.Length)
                parsedLexem.Add(new Text(str.Substring(lastSuccessMatchIndex)));
            return parsedLexem;
        }

        private static string BuildRegExForTags(IEnumerable<Tag> tags)
        {
            var result = @"(^|[^d\\])(";
            foreach (var tag in tags)
                result += tag.MarkUpRepresentation + '|';
            result = result.Remove(result.Length - 1);
            result += @")(\D|$)";
            return result;
        }
    }
}
