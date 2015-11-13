using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MarkupTags
    {
        public static Tag Em => new Tag("em", "_");
        public static Tag Strong => new Tag("strong", "__");
        public static Tag Code => new Tag("code", "`");


        public static IEnumerable<Tag> GetAllTags()
        {
            yield return Strong;
            yield return Em;
            yield return Code;
        }
    }
}
