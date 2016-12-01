namespace KitchenResponsible.Model
{
    public struct ResponsibleForWeek
    {
        public ResponsibleForWeek(ushort week, string responsible, string onDeck)
        {
            Week = week;
            Responsible = responsible;
            OnDeck = onDeck;
        }

        public ushort Week { get; }
        public string Responsible { get; }
        public string OnDeck { get; }

        public override string ToString() => $"{Week} {Responsible} {OnDeck}";
    }
}