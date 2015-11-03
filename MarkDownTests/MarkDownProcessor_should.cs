using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown;
using NUnit.Framework;

namespace MarkDownTests
{
    [TestFixture]
    public class MarkDownProcessor_should
    {
        private readonly MarkDownProcessor _markDownProcessor;

        public MarkDownProcessor_should()
        {
            var commands = new Dictionary<string, Command>
            {
                ["_"] = new Command("<em>", "</em>", true),
                ["__"] = new Command("<strong>", "</strong>", true),
                ["`"] = new Command("<code>", "</code>", false)
            };
            _markDownProcessor = new MarkDownProcessor(commands);
        }

        [Test]
        public void NotChangeInputStringWithoutCommand()
        {
            var input = "Просто текст";
            var result = _markDownProcessor.ProcessString(input);
            var expected = input.Clone();
            Assert.AreEqual(expected, result);
        }

        [TestCase("Текст _окруженный с двух сторон_  одинарными символами подчерка",
                    Result = "Текст <em>окруженный с двух сторон</em>  одинарными символами подчерка",
                    TestName = "ReplaceSingleSnakes")]

        [TestCase("Текст _окруженный с двух сторон_  одинарными _символами подчерка_",
                    Result = "Текст <em>окруженный с двух сторон</em>  одинарными <em>символами подчерка</em>",
                    TestName = "ReplaceMultipleSnakes")]

        [TestCase("Текст \\_окруженный с двух сторон\\_ одинарными символами подчерка",
                    Result = "Текст _окруженный с двух сторон_ одинарными символами подчерка",
                    TestName = "NotReplaceScreenedSnake")]

        [TestCase("Текст \\_окруженный с двух сторон\\_ одинарными \\_символами подчерка\\_",
                    Result = "Текст _окруженный с двух сторон_ одинарными _символами подчерка_",
                    TestName = "NotReplaceMultipleScreenedSnake")]

        [TestCase("_Текст окруженный с двух сторон_ одинарными символами подчерка",
                    Result = "<em>Текст окруженный с двух сторон</em> одинарными символами подчерка",
                    TestName = "ReplaceSnakesAtFirstIndex")]

        public string ReplaceSnakeToEm(string str)
        {
            return _markDownProcessor.ProcessString(str);
        }

        [TestCase("Двумя __символами__ - должен становиться жирным",
            Result = "Двумя <strong>символами</strong> - должен становиться жирным",
            TestName = "ReplaceSingleDoubleSnakes")]

        [TestCase("Двумя __символами__ - должен __становиться__ жирным",
            Result = "Двумя <strong>символами</strong> - должен <strong>становиться</strong> жирным",
            TestName = "ReplaceMultipleDoubleSnakes")]

        [TestCase("Двумя \\_\\_символами\\_\\_ - должен становиться жирным",
            Result = "Двумя __символами__ - должен становиться жирным",
            TestName = "NotReplaceSimpleScreenedDoubleSnakes")]

        [TestCase("Двумя \\_\\_символами\\_\\_ - должен \\_\\_становиться\\_\\_ жирным",
            Result = "Двумя __символами__ - должен __становиться__ жирным",
            TestName = "NotReplaceSimpleScreenedDoubleSnakes")]

        [TestCase("__Двумя символами__ - должен становиться жирным",
            Result = "<strong>Двумя символами</strong> - должен становиться жирным",
            TestName = "ReplaceDoubleSnakesAtFirstIndex")]

        public string ReplaceDoubleSnakesToStrong(string str)
        {
            return _markDownProcessor.ProcessString(str);
        }

        [Test]
        public void NotReplaceTags_IntoCodeTag()
        {
            var input = "Текст окруженный `одинарными _обратными_ кавычками` (backticks)";
            var expected = "Текст окруженный <code>одинарными _обратными_ кавычками</code> (backticks)";
            var result = _markDownProcessor.ProcessString(input);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ThrowException_OnBadInput()
        {
            var input = "Плохой _текст";
            Assert.Throws<ArgumentException>(() => _markDownProcessor.ProcessString(input));
        }
    }
}
