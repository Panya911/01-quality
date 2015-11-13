using Markdown;
using NUnit.Framework;

namespace MarkDownTests
{
    [TestFixture]
    class MarkDownParagraphDivider_should
    {
        [TestCase("paragraph1\r\n \r\t\r\nparagraph2",
            Result = new[] { "paragraph1", "paragraph2" },
            TestName = "divideString_withDifferentWhitespaceSymbols_between\\n")]

        [TestCase("paragraph1\r\n\r\nparagraph2",
            Result = new[] { "paragraph1", "paragraph2" },
            TestName = "divideString_withDouble\\n ")]
        [TestCase("paragraph1\t \r\n\r\n   \r paragraph2",
            Result = new[] { "paragraph1", "paragraph2" }
            , TestName = "divideString_withInitialAndFinalWhitespaces"
            )]
        [TestCase("paragraph1\r\nparagraph2\r\n",
            Result = new[] { "paragraph1\r\nparagraph2\r\n" },
            TestName = "notDivideString_withTextBetween\\n")]
        public string[] divideStringOnParagraph(string str)
        {
            return MarkDownParagraphDivider.DivideStringOnParagraph(str);
        }
    }
}
