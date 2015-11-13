using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Markdown
{
    public static class MarkDownParagraphDivider
    {
        public static string[] DivideStringOnParagraph(string str)
        {
            var str1 = @"\s*" + Environment.NewLine + @"\s*" + Environment.NewLine + @"\s*";
            var regex = new Regex(str1);
            var result = regex.Split(str);
            return result;
        }
    }
}
