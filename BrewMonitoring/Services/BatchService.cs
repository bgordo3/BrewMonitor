using System;
using BrewMonitoring.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Driver;

namespace BrewMonitoring.Services
{
	public class BatchService : EntityService<Batch>
	{
		public const string COLLECTION_NAME = "Batches";
		public BatchService ()
		{
		}

		#region implemented abstract members of EntityService

		public override string GetCollectionName ()
		{
			return COLLECTION_NAME;
		}

		#endregion

		/// <summary>
		/// Gets the batches currently in production (if any).
		/// </summary>
		/// <returns>The current batch.</returns>
		public async Task<List<Batch>> GetCurrentBatches()
		{
			try
			{
				return await EntityCollection.Find(item => item.IsDone).ToListAsync<Batch>();
			}
			catch (MongoConnectionException)
			{
				return new List<Batch>();
			}
		}
	}
}

