using System;
using System.Collections.Generic;
using Markdown;
using NUnit.Framework;

namespace MarkDownTests
{
    [TestFixture]
    public class MarkDownStringProcessor_should
    {
        private readonly Dictionary<string, Command> _commands;

        public MarkDownStringProcessor_should()
        {
            _commands = new Dictionary<string, Command>
            {
                ["_"] = new Command("em", needFormatInto: true, paired: true),
                ["__"] = new Command("strong", needFormatInto: true, paired: true),
                ["`"] = new Command("code", needFormatInto: false, paired: true),
                ["$"] = new Command("tag", needFormatInto: false, paired: false)
            };
        }

        [Test]
        public void notChangeInputStringWithoutCommand()
        {
            var input = "Просто текст";
            var markDownStringProcessor = new MarkDownStringProcessor(input, _commands);
            var result = markDownStringProcessor.Process();
            var expected = input.Clone();
            Assert.AreEqual(expected, result);
        }

        [TestCase("Текст _окруженный с двух сторон_  одинарными символами подчерка",
                    Result = "Текст <em>окруженный с двух сторон</em>  одинарными символами подчерка",
                    TestName = "ReplaceSingleSnakes")]

        [TestCase("Текст _окруженный с двух сторон_  одинарными _символами подчерка_",
                    Result = "Текст <em>окруженный с двух сторон</em>  одинарными <em>символами подчерка</em>",
                    TestName = "ReplaceMultipleSnakes")]

        [TestCase(@"Текст \_окруженный с двух сторон\_ одинарными символами подчерка",
                    Result = "Текст _окруженный с двух сторон_ одинарными символами подчерка",
                    TestName = "NotReplaceScreenedSnake")]

        [TestCase(@"Текст \_окруженный с двух сторон\_ одинарными \_символами подчерка\_",
                    Result = "Текст _окруженный с двух сторон_ одинарными _символами подчерка_",
                    TestName = "NotReplaceMultipleScreenedSnake")]

        [TestCase("_Текст окруженный с двух сторон_ одинарными символами подчерка",
                    Result = "<em>Текст окруженный с двух сторон</em> одинарными символами подчерка",
                    TestName = "ReplaceSnakesAtFirstIndex")]

        public string replaceSnakeToEm(string str)
        {
            var markDownStringProcessor = new MarkDownStringProcessor(str, _commands);
            return markDownStringProcessor.Process();
        }

        [TestCase("Двумя __символами__ - должен становиться жирным",
            Result = "Двумя <strong>символами</strong> - должен становиться жирным",
            TestName = "ReplaceSingleDoubleSnakes")]

        [TestCase("Двумя __символами__ - должен __становиться__ жирным",
            Result = "Двумя <strong>символами</strong> - должен <strong>становиться</strong> жирным",
            TestName = "ReplaceMultipleDoubleSnakes")]

        [TestCase(@"Двумя \_\_символами\_\_ - должен становиться жирным",
            Result = "Двумя __символами__ - должен становиться жирным",
            TestName = "NotReplaceSimpleScreenedDoubleSnakes")]

        [TestCase(@"Двумя \_\_символами\_\_ - должен \_\_становиться\_\_ жирным",
            Result = "Двумя __символами__ - должен __становиться__ жирным",
            TestName = "NotReplaceSimpleScreenedDoubleSnakes")]

        [TestCase("__Двумя символами__ - должен становиться жирным",
            Result = "<strong>Двумя символами</strong> - должен становиться жирным",
            TestName = "ReplaceDoubleSnakesAtFirstIndex")]

        public string replaceDoubleSnakesToStrong(string str)
        {
            var markDownStringProcessor = new MarkDownStringProcessor(str, _commands);
            return markDownStringProcessor.Process();
        }

        [Test]
        public void notReplaceTags_IntoCodeTag()
        {
            var input = @"Текст окруженный `одинарными _обратными_ кавычками` (backticks)";
            var expected = @"Текст окруженный <code>одинарными _обратными_ кавычками</code> (backticks)";
            var markDownStringProcessor = new MarkDownStringProcessor(input, _commands);
            var result = markDownStringProcessor.Process();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void notFormateTag_whichNeedntFormat_()
        {
            var input = @"Текст окруженный `одинарными $обратными$ кавычками` (backticks)";
            var expected = @"Текст окруженный <code>одинарными $обратными$ кавычками</code> (backticks)";
            var markDownStringProcessor = new MarkDownStringProcessor(input, _commands);
            var result = markDownStringProcessor.Process();
            Assert.AreEqual(expected, result);
        }

        [TestCase(@"number is 15_74",
            Result = @"number is 15_74",
            TestName = "notReplaceUnderscore_intoNumber")]
        [TestCase(@"number is 15__74",
            Result = @"number is 15__74",
            TestName = "notReplaceDoubleUnderscore_intoNumber")]
        [TestCase(@"внутри текста c цифрами_12_3",
            Result = @"внутри текста c цифрами_12_3",
            TestName = "notReplaceUnderscore_beforeDigit")]
        public string notReplaceCommandIntoNumber(string str)
        {
            var markDownStringProcessor = new MarkDownStringProcessor(str, _commands);
            return markDownStringProcessor.Process();
        }

        [Test]
        public void notReplaceUnpairedCommand()
        {
            var input = @"__непарные _символы не считаются `выделением";
            var expected = @"__непарные _символы не считаются `выделением";
            var markDownStringProcessor = new MarkDownStringProcessor(input, _commands);
            var result = markDownStringProcessor.Process();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void replaceAnglebrackets_correctly()
        {
            var input = @"Каждый абзац должен быть заключен в теги \<p\>...\</p\>";
            var expected = @"Каждый абзац должен быть заключен в теги &lt;p&gt;...&lt;/p&gt;";
            var markDownStringProcessor = new MarkDownStringProcessor(input, _commands);
            var result = markDownStringProcessor.Process();
            Assert.AreEqual(expected, result);
        }
    }
}
