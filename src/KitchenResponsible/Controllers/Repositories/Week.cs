namespace KitchenResponsible.Controllers.Repositories
{
    public struct Week
    {
        public Week(ushort weekNumber, string nameOfResponsible)
        {
            WeekNumber = weekNumber;
            NameOfResponsible = nameOfResponsible;
        }

        public ushort WeekNumber { get; }
        public string NameOfResponsible { get; }
    }
}