using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkDownProcessor
    {
        private readonly CommandTranslater _translater;
        private readonly MarkDownStringProcessor _markdownStringProcessor;

        public MarkDownProcessor(Dictionary<string, Command> commands)
        {
            _markdownStringProcessor = new MarkDownStringProcessor(commands);
        }

        public string HanldeBlock(string str)
        {
            throw new NotImplementedException();
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
    }
}
