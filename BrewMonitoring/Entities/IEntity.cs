using System;
using MongoDB.Bson;

namespace BrewMonitoring.Entities
{
	public interface IEntity
	{
		string Name { get; set; }
		ObjectId Id { get; set; }
	}
}

