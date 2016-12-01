namespace KitchenResponsible.Model
{
    public struct Week
    {
        public Week(ushort week, string responsible, ushort n)
        {
            WeekNumber = week;
            Responsible = responsible;
            N = n;
        }

        public ushort WeekNumber { get; }
        public string Responsible { get; }
        public ushort N { get; }

        public override string ToString() => $"{WeekNumber} {Responsible}";
    }
}