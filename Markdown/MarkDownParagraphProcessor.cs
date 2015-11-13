using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class MarkDownParagraphProcessor
    {
        public static string Process(string str)
        {
            var markUpElements = MarkDownParser.Parse(str);
            MarkDownValidator.Validate(markUpElements);
            var builder = new StringBuilder();
            foreach (var markUpElement in markUpElements)
            {
                builder.Append(markUpElement.HtmlRepresentation);
            }
            return builder.ToString();
        }
    }
}