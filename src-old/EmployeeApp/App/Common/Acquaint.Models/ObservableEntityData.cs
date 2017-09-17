using System;
using Acquaint.Abstractions;
using MvvmHelpers;
using Newtonsoft.Json;

namespace Acquaint.Models
{
	/// <summary>
	/// A type that mirrors the properties of Microsoft.Azure.Mobile.Server.EntityData, and is also observable.
	/// </summary>
	public class ObservableEntityData : ObservableObject, IObservableEntityData
	{
		public ObservableEntityData()
		{
			Id = Guid.NewGuid().ToString().ToUpper();
		}

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset UpdatedAt { get; set; }

		public byte[] Version { get; set; }
	}
}

