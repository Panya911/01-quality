using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class MarkDownStringProcessor
    {
        private readonly CommandTranslater _translater;
        private int _index;
        private readonly Stack<CommandIntoText> _openedCommands;
        private bool _needFormat;
        private readonly StringBuilder _result;
        private readonly string _str;

        public MarkDownStringProcessor(string str, Dictionary<string, Command> commands)
        {
            _str = str;
            _translater = new CommandTranslater(commands);
            _openedCommands = new Stack<CommandIntoText>();
            _result = new StringBuilder();
            _needFormat = true;
        }

        public string Process()
        {
            var temp = "";
            _index = 0;
            while (_index < _str.Length)
            {
                temp += _str[_index];
                var answer = _translater.CanDefineCommand(temp);
                switch (answer)
                {
                    case CommandTranslater.Answer.No:
                        {
                            _index += ProcessAsSimpleText(temp);
                            temp = "";
                            break;
                        }
                    case CommandTranslater.Answer.EscapeSymbol:
                        {
                            _index += ProcessAsEscapeSimbol();
                            temp = "";
                            break;
                        }
                    case CommandTranslater.Answer.Yes:
                        {
                            if (IsBetweenDigits(_index, temp))
                                _index += ProcessAsSimpleText(temp);
                            else
                                _index += ProcessAsCommand(temp, _index);
                            temp = "";
                            break;
                        }
                    case CommandTranslater.Answer.YesButCanBeLonger:
                        {
                            var cmd = GetMaxCommand(temp, _index);
                            if (IsBetweenDigits(_index, cmd))
                                _index += ProcessAsSimpleText(cmd);
                            else
                                _index += ProcessAsCommand(cmd, _index);
                            temp = "";
                            break;
                        }
                    case CommandTranslater.Answer.Maybe:
                        {
                            _index++;
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            //if (_openedCommands.Count != 0)
            //    foreach (var command in _openedCommands)
            //    {
            //        _result.Insert(command.Position, command.Command);
            //    }
            return _result.ToString();
        }

        private int ProcessAsSimpleText(string str)
        {
            _result.Append(str);
            return str.Length;
        }

        private int ProcessAsEscapeSimbol()
        {
            _result.Append(_str[_index + 1]);
            return 2;
        }

        private int ProcessAsCommand(string str, int i)
        {
            var needFormatChangedOnFalse = false;
            if (!_translater.NeedFormatInto(str))
                needFormatChangedOnFalse = ChangeNeedFormatFlag(str);

            if (_needFormat ^ needFormatChangedOnFalse)
                _result.Append(GetNeededTag(str, i));
            else
            {
                var cmd = str;
                _result.Append(cmd);
            }
            return str.Length;
        }

        private bool ChangeNeedFormatFlag(string str)
        {
            var needFormatChangedOnFalse = false;
            if (_needFormat)
            {
                _needFormat = false;
                needFormatChangedOnFalse = true;
            }
            if (PeekItemIsTheSame(str))
                _needFormat = true;
            return needFormatChangedOnFalse;
        }

        private string GetNeededTag(string str, int index)
        {
            if (!_translater.IsCloseableCommand(str))
            {
                return _translater.GetOpenedForm(str);

            }
            if (PeekItemIsTheSame(str))
            {
                return _translater.GetClosedForm(_openedCommands.Pop().Command);
            }
            _openedCommands.Push(new CommandIntoText(str, index));//todo
            return _translater.GetOpenedForm(str);
        }

        private bool PeekItemIsTheSame(string command)
        {
            return _openedCommands.Count > 0 && _openedCommands.Peek().Command == command;
        }

        private bool IsBetweenDigits(int index, string command)
        {
            return IsInsideBounds(index - 1) && IsInsideBounds(index + command.Length + 1) &&
                char.IsDigit(_str[index - 1]) && char.IsDigit(_str[index + command.Length + 1]);
        }

        private bool IsInsideBounds(int index)
        {
            return index >= 0 && index < _str.Length;
        }

        private string GetMaxCommand(string temp, int index)
        {
            var i = _index;
            //todo может быть maybe. исправить
            while (!(i + 1 == _str.Length ||
                _translater.CanDefineCommand(temp + _str[i + 1]) == CommandTranslater.Answer.No))
            {
                i++;
                temp += _str[i];
            }
            return temp;
        }

        class CommandIntoText
        {
            public readonly string Command;
            public readonly int Position;

            public CommandIntoText(string command, int position)
            {
                Command = command;
                Position = position;
            }
        }
    }
}