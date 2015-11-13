namespace Markdown
{
    public interface IMarkUpElement
    {
        string HtmlRepresentation { get; }
        string MarkUpRepresentation { get; }
    }
}