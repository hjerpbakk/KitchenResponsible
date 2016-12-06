using System;
using Newtonsoft.Json;

namespace Acquaint.Models
{
	public struct Week
	{
		[JsonConstructor]
		public Week(ushort weekNumber, string responsible)
		{
			WeekNumber = weekNumber;
			Responsible = responsible;
		}

		public ushort WeekNumber { get; }
		public string Responsible { get; }

		public override string ToString() => $"{WeekNumber} {Responsible}";
	}
}
