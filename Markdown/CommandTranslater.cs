using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    class CommandTranslater
    {
        private readonly Dictionary<string, Command> _commands;

        public CommandTranslater(Dictionary<string, Command> commands)
        {
            _commands = commands.Clone();
        }
        public enum Answer
        {
            Yes,
            YesButCanBeLonger,
            No,
            Maybe,
            EscapeSymbol
        }

        public Answer CanDefineCommand(string str)
        {
            if(str==@"\")
                return Answer.EscapeSymbol;
            var c = _commands.Keys.Where(key => key.Contains(str)).ToList();
            if (c.Count > 1)
                return _commands.ContainsKey(str)
                    ? Answer.YesButCanBeLonger
                    : Answer.Maybe;
            if (c.Count == 1)
                return c.First() == str ? Answer.Yes : Answer.Maybe;
            return Answer.No;
        }

        public string GetClosedForm(string command)
        {
            return _commands[command].ClosedForm;
        }

        public bool NeedFormatInto(string command)
        {
            return _commands[command].NeedFormateInto;
        }
        public bool IsCloseableCommand(string str)
        {
            return _commands[str].IsPaired;
        }

        public string GetOpenedForm(string command)
        {
            return _commands[command].OpenedForm;
        }
    }
}