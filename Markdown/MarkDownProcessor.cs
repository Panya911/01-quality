using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class MarkDownProcessor
    {
        public static string ProcessText(string text, Dictionary<string, Command> commands)
        {
            var strBuilder = new StringBuilder();
            var paragraphs = MarkDownParagraphDivider.DivideStringOnParagraph(text);
            foreach (var paragraph in paragraphs)
            {
                var stringProcessor = new MarkDownStringProcessor(paragraph, commands);
                var result = stringProcessor.Process();
                strBuilder.Append("<p>" + result + "</p>");
            }
            return strBuilder.ToString();
        }
    }
}
