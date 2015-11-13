using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkDownValidator
    {
        public static void Validate(List<IMarkUpElement> elementsSequance)
        {

            var sequanceControllers = GetSequanceControllersForTags(elementsSequance);
            ReplaceUnpairedTagsToText(elementsSequance, sequanceControllers);
            HandleCodeTags(elementsSequance);
            ValidateTextElements(elementsSequance);
        }

        private static Dictionary<Tag, TagSequanceController> GetSequanceControllersForTags(List<IMarkUpElement> elementsSequance)
        {
            var sequanceControllers = new Dictionary<Tag, TagSequanceController>();
            for (var i = 0; i < elementsSequance.Count; i++)
            {
                if (elementsSequance[i] is Tag)
                {
                    var tag = (Tag)elementsSequance[i];
                    if (sequanceControllers.ContainsKey(tag))
                    {
                        if (sequanceControllers[tag].LastTagIsOpened)
                        {
                            tag.Opened = false;
                            sequanceControllers[tag].LastTagIsOpened = false;
                        }
                        else
                        {
                            sequanceControllers[tag].LastTagIsOpened = true;
                            tag.Opened = true;
                            sequanceControllers[tag].LastOpenedTagIndex = i;
                        }
                    }
                    else
                    {
                        tag.Opened = true;
                        sequanceControllers[tag] = new TagSequanceController
                        {
                            LastTagIsOpened = true,
                            LastOpenedTagIndex = i
                        };
                    }
                }
            }
            return sequanceControllers;
        }

        private static void ValidateTextElements(List<IMarkUpElement> elementsSequance)
        {
            for (var i = 0; i < elementsSequance.Count; i++)
                if (elementsSequance[i] is Text)
                    elementsSequance[i] = ValidateText(elementsSequance[i].MarkUpRepresentation);
        }

        private static void HandleCodeTags(List<IMarkUpElement> elementsSequance)
        {
            var needValidate = true;
            for (var i = 0; i < elementsSequance.Count; i++)
            {
                if (elementsSequance[i].Equals(MarkupTags.Code))
                {
                    needValidate = !needValidate;
                    continue;
                }
                if (!needValidate)
                    elementsSequance[i] = new Text(elementsSequance[i].MarkUpRepresentation);
            }
        }

        private static void ReplaceUnpairedTagsToText(List<IMarkUpElement> elementsSequance, Dictionary<Tag, TagSequanceController> sequanceControllers)
        {
            foreach (var tag in sequanceControllers.Keys.Where(tag => sequanceControllers[tag].LastTagIsOpened))
                elementsSequance[sequanceControllers[tag].LastOpenedTagIndex] = new Text(tag.MarkUpRepresentation);
        }

        private static Text ValidateText(string str)
        {
            return new Text(str.Replace("\\", "")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;"));
        }

        class TagSequanceController
        {
            public int LastOpenedTagIndex;
            public bool LastTagIsOpened;
        }
    }
}
