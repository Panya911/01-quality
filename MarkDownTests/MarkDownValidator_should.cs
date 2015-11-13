using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown;
using NUnit.Framework;
using Text = Markdown.Text;

namespace MarkDownTests
{
    [TestFixture]
    public class MarkDownValidator_should
    {
        [Test]
        public void validateTextCorrectly()
        {
            var inputSequance = new List<IMarkUpElement> { new Text("<text1>") };
            MarkDownValidator.Validate(inputSequance);
            CollectionAssert.AreEqual(
                new List<IMarkUpElement> { new Text("&lt;text1&gt;") },
                inputSequance);
        }

        [Test]
        public void replaceUnpairedTags_toText()
        {
            var inputSequance = new List<IMarkUpElement> { MarkupTags.Em, new Text("text1"), new Text("text2") };
            MarkDownValidator.Validate(inputSequance);
            CollectionAssert.AreEqual(
                new List<IMarkUpElement> { new Text("_"), new Text("text1"), new Text("text2") },
                inputSequance);
        }

        [Test]
        public void replaceMultipleUnpairedTags_toText()
        {
            var inputSequance = new List<IMarkUpElement> { MarkupTags.Em, new Text("text1"), MarkupTags.Strong, new Text("text2"), MarkupTags.Code, MarkupTags.Em };
            MarkDownValidator.Validate(inputSequance);
            CollectionAssert.AreEqual(
                new List<IMarkUpElement> { MarkupTags.Em, new Text("text1"), new Text("__"), new Text("text2"), new Text("`"), MarkupTags.Em },
                inputSequance);
        }



        [Test]
        public void replaceTagsToText_ifItIsIntoCodeTag()
        {
            var inputSequance = new List<IMarkUpElement>
            {
                new Tag("", "`"),
                    new Text("text1"),
                    new Tag("", "__"),
                    new Tag("", "__"),
                    new Text("text2"),
                new Tag("", "`")
            };
            MarkDownValidator.Validate(inputSequance);
            CollectionAssert.AreEqual(
                new List<IMarkUpElement>
                {
                    new Tag(" ", "`"),
                        new Text("text1"),
                        new Text("__"),
                        new Text("__"),
                        new Text("text2"),
                    new Tag("","`")
                },
                inputSequance);
        }
    }
}