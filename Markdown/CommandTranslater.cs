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

        public enum CanDefineCommandAnswer
        {
            Yes,
            YesButCanBeLonger,
            No,
            Maybe,
            EscapeSymbol
        }

        public CanDefineCommandAnswer CanDefineCommand(string str)
        {
            if(str==@"\")
                return CanDefineCommandAnswer.EscapeSymbol;
            var possibleKeys = _commands.Keys.Where(key => key.Contains(str)).ToList();
            if (possibleKeys.Count > 1)
                return _commands.ContainsKey(str)
                    ? CanDefineCommandAnswer.YesButCanBeLonger
                    : CanDefineCommandAnswer.Maybe;
            if (possibleKeys.Count == 1)
                return possibleKeys.First() == str ? CanDefineCommandAnswer.Yes : CanDefineCommandAnswer.Maybe;
            return CanDefineCommandAnswer.No;
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