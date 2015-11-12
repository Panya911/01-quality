using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Markdown
{
    public static class MarkDownParagraphDivider
    {
        public static string[] DivideStringOnParagraph(string str)
        {
            var regex = new Regex(@"\s*\n\s*\n\s*");
            var result = regex.Split(str);
            return result;
        }
    }
}
