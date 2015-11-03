using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Command
    {
        public readonly string OpenedForm;
        public readonly string ClosedForm;
        public readonly bool NeedFormateInto;

        public Command(string openedForm, bool needFormatInto)
        {
            OpenedForm = openedForm;
            ClosedForm = null;
            NeedFormateInto = needFormatInto;
        }

        public Command(string openedForm, string closedForm, bool needFormatInto)
        {
            OpenedForm = openedForm;
            ClosedForm = closedForm;
            NeedFormateInto = needFormatInto;
        }

        public bool IsCloseable => ClosedForm != null;
    }


    public class MarkDownProcessor
    {
        private readonly CommandTranslater _translater;
        public MarkDownProcessor(Dictionary<string, Command> commands)
        {
            _translater = new CommandTranslater(commands);
        }

        public string HanldeBlock(string str)
        {
            throw new NotImplementedException();
        }

        public string ProcessString(string str)
        {
            var result = new StringBuilder();
            var openedTags = new Stack<string>();
            var temp = "";
            var needFormat = true;
            for (var i = 0; i < str.Length; i++)
            {
                temp += str[i];
                if (temp == "\\")
                {
                    result.Append(str[i + 1]);
                    i++;
                    temp = "";
                    continue;
                }
                var answer = _translater.CanDefineCommand(temp);
                switch (answer)
                {
                    case CommandTranslater.Answer.No:
                        result.Append(temp);
                        temp = "";
                        break;
                    case CommandTranslater.Answer.Yes:
                        {
                            var cmd = TranslateCommandToNeedForm(temp, openedTags);
                            if (!_translater.NeedFormatInto(temp))
                            {
                                needFormat = !needFormat;
                                result.Append(cmd);
                            }
                            else
                                result.Append(needFormat ? cmd : temp);
                            temp = "";
                            break;
                        }

                    case CommandTranslater.Answer.YesButCanBeLonger:
                        {
                            //todo выпилить костыль
                            if (i + 1 == str.Length ||
                                _translater.CanDefineCommand(temp + str[i + 1]) == CommandTranslater.Answer.No)
                            {
                                var cmd = TranslateCommandToNeedForm(temp, openedTags);
                                result.Append(needFormat ? cmd : temp);
                                temp = "";
                            }
                            break;
                        }
                    case CommandTranslater.Answer.Maybe:
                        {
                            continue;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (openedTags.Count!=0)
                throw new ArgumentException("Ваш текст отстой:)");
            return result.ToString();
        }

        private string TranslateCommandToNeedForm(string command, Stack<string> openedTags)
        {
            if (!_translater.IsCloseableCommand(command))
                return _translater.GetOpenedForm(command);

            if (openedTags.Count > 0 && openedTags.Peek() == command)
                return _translater.GetClosedForm(openedTags.Pop());
            openedTags.Push(command);
            return _translater.GetOpenedForm(command);
        }

        private class CommandTranslater
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
                Maybe
            }

            public Answer CanDefineCommand(string str)
            {
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
                return _commands[str].IsCloseable;
            }

            public string GetOpenedForm(string command)
            {
                return _commands[command].OpenedForm;
            }
        }
    }

    public static class DictionaryExtension
    {
        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return dict.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
