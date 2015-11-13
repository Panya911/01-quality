namespace Markdown
{
    public class Tag : IMarkUpElement
    {



        public bool Opened { get; set; }

        public string Name { get; }
        public string HtmlRepresentation => Opened ? '<' + Name + '>' : "</" + Name + '>';
        public string MarkUpRepresentation { get; }

        public Tag(string name, string markUpRepresentation)
        {
            Name = name;
            MarkUpRepresentation = markUpRepresentation;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Tag;
            return other != null && MarkUpRepresentation == other.MarkUpRepresentation;
        }

        public override int GetHashCode()
        {
            return MarkUpRepresentation.GetHashCode() * 31 + Name.GetHashCode();
        }
    }
}