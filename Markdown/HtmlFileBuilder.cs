using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class HtmlFileBuilder
    {
        public static string Build(string content)
        {
            return "<html>" +
                   "<head>" +
                   "</head>" +
                   "<body>" +
                   content +
                   "</body>" +
                   "</html>";
        }
    }
}
