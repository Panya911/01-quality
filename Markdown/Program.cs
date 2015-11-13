using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText(args[0]);
            var result = MarkDownProcessor.ProcessText(input);
            var html = HtmlFileBuilder.Build(result);
            File.WriteAllText("output.html", html, Encoding.UTF8);
        }
    }
}
