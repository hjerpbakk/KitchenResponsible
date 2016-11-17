using System;
using Newtonsoft.Json;

namespace Acquaint.Models {
	public class TeamMember {
		[JsonConstructor]
		public TeamMember(string email, string phone) {
			Email = email;
			Phone = phone;

			State = "State";
			City = "City";
			Street = "Street";
			PostalCode = "PostalCode";
		}

		public TeamMemberLite TeamMemberLite { private get; set; }

		public int Id { get { return TeamMemberLite.Id; } }
		public string FirstName { get { return TeamMemberLite.FirstName; } }
		public string LastName { get { return TeamMemberLite.LastName; } }
		public string Company { get { return TeamMemberLite.Company; } }
		public string JobTitle { get { return TeamMemberLite.JobTitle; } }
		public string SmallPhotoUrl { get { return TeamMemberLite.SmallPhotoUrl; } }

		public string Email { get; }
		public string Phone { get; }
		public string State { get; }
		public string City { get; }
		public string Street { get; }
		public string PostalCode { get; }

		public string PhotoUrl => SmallPhotoUrl;

		[JsonIgnore]
		public string AddressString => string.Format(
			"{0} {1} {2} {3}",
			Street,
			!string.IsNullOrWhiteSpace(City) ? City + "," : "",
			State,
			PostalCode);

		[JsonIgnore]
		public string StatePostal => State + " " + PostalCode;
	}
}
