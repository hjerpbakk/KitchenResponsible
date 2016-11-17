﻿namespace KitchenResponsible.Model
{
    public class Employee
    {
        public Employee(ushort weekReponsible, string firstName, string lastName, string nick = null)
        {
            WeekResponsible = weekReponsible;
            FirstName = firstName;
            LastName = lastName;
            Nick = nick ?? firstName; 
        }

        public ushort WeekResponsible { get; set; }
        public string Nick { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}