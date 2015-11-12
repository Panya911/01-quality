using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = new Dictionary<string, Command>
            {
                ["_"] = new Command("em", needFormatInto: true, paired: true),
                ["__"] = new Command("strong", needFormatInto: true, paired: true),
                ["`"] = new Command("code", needFormatInto: false, paired: true),
                ["$"] = new Command("tag", needFormatInto: false, paired: false)
            };
            var input = File.ReadAllText(args[0]);
            var result = MarkDownProcessor.ProcessText(input, commands);
            var html = HtmlFileBuilder.Build(result);
            File.WriteAllText("output.html", html,Encoding.UTF8);
        }
    }
}
