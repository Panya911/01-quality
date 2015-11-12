using Markdown;
using NUnit.Framework;

namespace MarkDownTests
{
    [TestFixture]
    class MarkDownParagraphDivider_should
    {
        [TestCase("paragraph1\n \r\t\nparagraph2",
            Result = new[] { "paragraph1", "paragraph2" },
            TestName = "divideString_withDifferentWhitespaceSymbols_between\\n")]

        [TestCase("paragraph1\n\nparagraph2",
            Result = new[] { "paragraph1", "paragraph2" },
            TestName = "divideString_withDouble\\n ")]
        [TestCase("paragraph1\t \n\n   \r paragraph2",
            Result = new[] { "paragraph1", "paragraph2" }
            , TestName = "divideString_withInitialAndFinalWhitespaces"
            )]
        [TestCase("paragraph1\nparagraph2\n",
            Result = new[] { "paragraph1\nparagraph2\n" },
            TestName = "notDivideString_withTextBetween\\n")]
        public string[] divideStringOnParagraph(string str)
        {
            return MarkDownParagraphDivider.DivideStringOnParagraph(str);
        }
    }
}
