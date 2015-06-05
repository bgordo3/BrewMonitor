using System;
using BrewMonitoring.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Driver;

namespace BrewMonitoring.Services
{
	public class RecipeService : EntityService<Recipe>
	{
		public const string COLLECTION_NAME = "Recipe";
		public RecipeService ()
		{
		}

		#region implemented abstract members of EntityService

		public override string GetCollectionName ()
		{
			return COLLECTION_NAME;
		}

		#endregion
	}
}

