namespace Markdown
{
    public class Command
    {
        private readonly string _name;

        public bool NeedFormateInto { get; }
        public bool IsPaired { get; }

        public string OpenedForm => '<' + _name + '>';
        public string ClosedForm => IsPaired ? "</" + _name + '>' : null;


        public Command(string name, bool needFormatInto, bool paired)
        {
            _name = name;
            IsPaired = paired;
            NeedFormateInto = needFormatInto;
        }
    }
}