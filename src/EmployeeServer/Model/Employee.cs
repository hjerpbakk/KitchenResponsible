namespace KitchenResponsible.Model
{
    public class Employee
    {
        public Employee(ushort weekReponsible, string firstName, string lastName, string nick = null)
        {
            WeekResponsible = weekReponsible;
            FirstName = firstName;
            LastName = lastName;
            Nick = nick ?? firstName; 

            JobTitle = "UX Utvikler";
            Company = "Health Angels";
            Email = "roh@dips.no";
            Phone = "98817218";

            // TODO: Use id
            Id = 1;
        }

        public ushort WeekResponsible { get; set; }
        public string Nick { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Company { get; }
        public string JobTitle { get; }
        public string Email { get; }
        public string Phone { get; }
        public int Id { get; }
    }
}