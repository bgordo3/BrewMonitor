using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BrewMonitoring.Entities
{
	public interface IEntity
	{
		
		string Name { get; set; }
		ObjectId _id { get; set; }
	}
}

