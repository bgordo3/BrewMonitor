using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using MongoDB.Bson;

namespace BrewMonitoring.Entities
{
	/// <summary>
	/// Beer Recipe.
	/// </summary>
	public class Recipe : IEntity
	{
		public string Name { get; set; }
		public ObjectId Id { get; set; }

		public enum Type
		{
			Ale,
			Lager,
			Stout,
			Malt
		}

		public enum Style
		{
			Light,
			Pale,
			Blonde,
			Red,
			Brown,
			Amber,
			Dark,
			Cream,
			Fruit,
			Golden,
			Honey,
			IndiaPaleAle,
			Lime,
			Pilsner,
			Strong,
			Wheat
		}

		public Style BeerStyle { get; set; }
		public Type BeerType { get; set; }
		/// <summary>
		/// The fermentation ideal temperature curve.
		/// </summary>
		public DataCurve FermentationTemperatureCurve { get; set; } 


		public Recipe ()
		{
		}


	}
}

