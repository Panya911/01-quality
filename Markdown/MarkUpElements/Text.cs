namespace Markdown
{
    public class Text : IMarkUpElement
    {
        public Text(string text)
        {
            HtmlRepresentation = text;
            MarkUpRepresentation = text;
        }

        public string HtmlRepresentation { get; }
        public string MarkUpRepresentation { get; }

        public override bool Equals(object obj)
        {
            var other = obj as Text;
            return other != null && MarkUpRepresentation == other.MarkUpRepresentation;
        }
    }
}