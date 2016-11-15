namespace KitchenResponsible.Controllers
{
    public class Employee
    {
        public Employee(ushort weekReponsible, string name)
        {
            WeekResponsible = weekReponsible;
            Name = name;
        }

        public ushort WeekResponsible { get; set; }
        public string Name { get; }
    }
}