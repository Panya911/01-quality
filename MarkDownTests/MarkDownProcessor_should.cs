using System;
using System.Collections.Generic;
using System.IO;
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
        [Test]
        public void ProcessMarkDownFile_corretly()
        {
            var input = File.ReadAllText("testedText.md");
            var result = MarkDownProcessor.ProcessText(input);
            var answer = File.ReadAllText("answer.html");
            Assert.AreEqual(answer,result);
        }
    }
}
