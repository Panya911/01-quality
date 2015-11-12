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
                    case CommandTranslater.CanDefineCommandAnswer.No:
                        {
                            _index += ProcessAsSimpleText(temp);
                            temp = "";
                            break;
                        }
                    case CommandTranslater.CanDefineCommandAnswer.EscapeSymbol:
                        {
                            _index += ProcessAsEscapeSimbol();
                            temp = "";
                            break;
                        }
                    case CommandTranslater.CanDefineCommandAnswer.Yes:
                        {
                            if (IsNearDigit(_index, temp))
                                _index += ProcessAsSimpleText(temp);
                            else
                                _index += ProcessAsCommand(temp, _index);
                            temp = "";
                            break;
                        }
                    case CommandTranslater.CanDefineCommandAnswer.YesButCanBeLonger:
                        {
                            var cmd = GetMaxCommand(temp, _index);
                            if (IsNearDigit(_index, cmd))
                                _index += ProcessAsSimpleText(cmd);
                            else
                                _index += ProcessAsCommand(cmd, _index);
                            temp = "";
                            break;
                        }
                    case CommandTranslater.CanDefineCommandAnswer.Maybe:
                        {
                            _index++;
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return _result.ToString();
        }

        private int ProcessAsSimpleText(string str)
        {
            _result.Append(str);
            return str.Length;
        }

        private int ProcessAsEscapeSimbol()
        {
            var nextSymbol = _str[_index + 1];
            switch (nextSymbol)
            {
                case '<':
                    _result.Append("&lt;");
                    break;
                case '>':
                    _result.Append("&gt;");
                    break;
                default:
                    _result.Append(nextSymbol);
                    break;
            }
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
                return _translater.GetOpenedForm(str);
            if (PeekItemIsTheSame(str))
                return _translater.GetClosedForm(_openedCommands.Pop().Command);
            _openedCommands.Push(new CommandIntoText(str, index));//todo
            return _translater.GetOpenedForm(str);
        }

        private bool PeekItemIsTheSame(string command)
        {
            return _openedCommands.Count > 0 && _openedCommands.Peek().Command == command;
        }

        private bool IsNearDigit(int index, string command)
        {
            return IsInsideBounds(index - 1) && IsInsideBounds(index + command.Length) &&
                (char.IsDigit(_str[index - 1]) || char.IsDigit(_str[index + command.Length]));
        }

        private bool IsInsideBounds(int index)
        {
            return index >= 0 && index < _str.Length;
        }

        private string GetMaxCommand(string temp, int index)
        {
            var i = _index;
            while (!(i + 1 == _str.Length ||
                _translater.CanDefineCommand(temp + _str[i + 1]) == CommandTranslater.CanDefineCommandAnswer.No))
            {
                i++;
                temp += _str[i];
            }
            return temp;
        }

        class CommandIntoText
        {
            public readonly string Command;
            public readonly int Position; //пока не используется

            public CommandIntoText(string command, int position)
            {
                Command = command;
                Position = position;
            }
        }
    }
}