using System;
using Newtonsoft.Json;

namespace Acquaint.Models {
	public class TeamMemberLite {
		[JsonConstructor]
		public TeamMemberLite(int id, string firstName, string lastName, string company, string jobTitle) {
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			Company = company;
			JobTitle = jobTitle;
		}

		public int Id { get; }
		public string FirstName { get; }
		public string LastName { get; }
		public string Company { get; }
		public string JobTitle { get; }
		public string SmallPhotoUrl { get; }

		[JsonIgnore]
		public string DisplayLastNameFirst => $"{LastName}, {FirstName}";

		[JsonIgnore]
		public string DisplayName => ToString();

		public override string ToString() => $"{FirstName} {LastName}";
	}
}
