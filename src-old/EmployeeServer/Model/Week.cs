namespace KitchenResponsible.Model
{
    public struct Week
    {
        public Week(ushort week, string responsible)
        {
            WeekNumber = week;
            Responsible = responsible;
        }

        public ushort WeekNumber { get; }
        public string Responsible { get; }
        
        public override string ToString() => $"{WeekNumber} {Responsible}";
    }
}