using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace BrewMonitoring.Entities
{
	public class Batch : IEntity
	{
		public Recipe BeerRecipe { get; set; }
		public DateTime ProductionStart { get; set; }
		public DateTime ProductionEnd { get; set; }
		public bool IsDone { get; set; }
		public Measurable FermentationMesures { get; set; }
		public string Name { get; set; }
		public ObjectId Id { get; set; }

		public Batch () : base()
		{
			IsDone = false;
			ProductionEnd = DateTime.MaxValue;
			ProductionStart = DateTime.MinValue;
			FermentationMesures = new Measurable ();
		}
	}
}

