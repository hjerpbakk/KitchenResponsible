using System;
using Newtonsoft.Json;

namespace Acquaint.Models
{
	public struct Week
	{
		[JsonConstructor]
		public Week(ushort weekNumber, string responsible, string smallPhotoUrl)
		{
			WeekNumber = weekNumber;
			Responsible = responsible;
			SmallPhotoUrl = smallPhotoUrl;
		}

		public ushort WeekNumber { get; }
		public string Responsible { get; }
		public string SmallPhotoUrl { get; }

		public override string ToString() => $"{WeekNumber} {Responsible}";
	}
}
