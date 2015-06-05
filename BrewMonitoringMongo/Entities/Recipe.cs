using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BrewMonitoring.Entities
{
	/// <summary>
	/// Beer Recipe.
	/// </summary>
	public class Recipe : IEntity
	{
		public string Name { get; set; }
		public ObjectId _id { get; set; }

		public enum Type
		{
			//[Display(Name = "Ale")]
			Ale,
			//[Display(Name = "Lager")]
			Lager,
			//[Display(Name = "Stout")]
			Stout,
			//[Display(Name = "Malt")]
			Malt,
			Max
		}

		public enum Style
		{
			///[Display(Name = "Light")]
			Light,
			//[Display(Name = "Pale")]
			Pale,
			//[Display(Name = "Blonde")]
			Blonde,
			//[Display(Name = "Red")]
			Red,
			//[Display(Name = "Brown")]
			Brown,
			//[Display(Name = "Amber")]
			Amber,
			///[Display(Name = "Dark")]
			Dark,
			//[Display(Name = "Cream")]
			Cream,
			//[Display(Name = "Fruit")]
			Fruit,
			//[Display(Name = "Golden")]
			Golden,
			//[Display(Name = "Honey")]
			Honey,
			//[Display(Name = "India Pale Ale")]
			IndiaPaleAle,
			//[Display(Name = "Lime")]
			Lime,
			//[Display(Name = "Pilsner")]
			Pilsner,
			//[Display(Name = "Strong")]
			Strong,
			//[Display(Name = "Wheat")]
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
			_id = new ObjectId();
			FermentationTemperatureCurve = new DataCurve ();
		}


	}
}

