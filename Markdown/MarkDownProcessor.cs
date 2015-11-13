using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class MarkDownProcessor
    {
        public static string ProcessText(string text)
        {
            var strBuilder = new StringBuilder();
            var paragraphs = MarkDownParagraphDivider.DivideStringOnParagraph(text);
            foreach (var handledParagraph in paragraphs.Select(MarkDownParagraphProcessor.Process))
                strBuilder.Append("<p>" + handledParagraph + "</p>");
            return strBuilder.ToString();
        }
    }
}
