using System;
using System.Linq;
using System.Collections.Generic;

namespace KitchenResponsibleService.Model
{
    public struct Employee : IEquatable<Employee>
    {
        public Employee(string slackUserId, string name) {
            SlackUserId = slackUserId;
            Name = name;
        }
        
        public string SlackUserId { get; set; }
        public string Name { get; set; }

        bool IEquatable<Employee>.Equals(Employee other)
            => SlackUserId == other.SlackUserId && Name == other.Name; 
    }
}