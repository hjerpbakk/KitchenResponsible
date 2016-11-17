using System;
using Newtonsoft.Json;

namespace Acquaint.Models {
	public class TeamMember : TeamMemberLite {
		[JsonConstructor]
		public TeamMember(int id, string firstName, string lastName, string company, string jobTitle, string email, string phone) 
			: base(id, firstName, lastName, company, jobTitle) {
			Email = email;
			Phone = phone;

			State = "State";
			City = "City";
			Street = "Street";
			PostalCode = "PostalCode";
		}

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
