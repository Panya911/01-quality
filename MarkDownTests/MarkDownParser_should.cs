using System.Collections.Generic;
using Markdown;
using NUnit.Framework;
using Text = Markdown.Text;

namespace MarkDownTests
{
    [TestFixture]
    public class MarkDownParser_should
    {

        [Test]
        public void parseTextOnLexem_correctly()
        {
            var result = MarkDownParser.Parse("Текст _окруженный с двух сторон_ одинарными символами подчерка должен помещаться в тег \\< em\\>");
            CollectionAssert.AreEqual(
                new List<IMarkUpElement>
                {
                    new Text("Текст "),
                    MarkupTags.Em,
                    new Text("окруженный с двух сторон"),
                    MarkupTags.Em,
                    new Text(" одинарными символами подчерка должен помещаться в тег \\< em\\>")
                },
                result
                );
        }

        [Test]
        public void parseStringWithNestedTags_correctly()
        {
            var result = MarkDownParser.Parse(@"Внутри _выделения \<em\> может быть __\<strong\>__ выделение_");
            CollectionAssert.AreEqual(
                new List<IMarkUpElement>
                {
                    new Text("Внутри "),
                    MarkupTags.Em,
                        new Text("выделения \\<em\\> может быть "),
                        MarkupTags.Strong,
                            new Text("\\<strong\\>"),
                        MarkupTags.Strong,
                        new Text(" выделение"),
                    MarkupTags.Em
                },
                result
                );
        }
    }

}
