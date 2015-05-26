using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrewMonitoring;
using MongoDB.Driver;
using MongoDB;
using System.Configuration;
using MongoDB.Bson.Serialization;
using System.Threading.Tasks;
using BrewMonitoring.Entities;
using MongoDB.Bson;

namespace BrewMonitoring.Services
{
	public abstract class EntityService<EntityType> where EntityType : BrewMonitoring.Entities.IEntity
	{
		public enum ServiceStatus
		{
			Error,
			Warning,
			Ok
		}
		public ServiceStatus Status = ServiceStatus.Ok;
		public string ErrorMessage = "";

		protected IMongoCollection<EntityType> EntityCollection = null;

		// Default constructor.        
		public EntityService()
		{
			//Registering model classes.
			if (!BsonClassMap.IsClassMapRegistered(typeof(EntityType)))
				BsonClassMap.RegisterClassMap<EntityType>();

			if (MongoConnection.GetInstance().Database != null)
				EntityCollection = MongoConnection.GetInstance().Database.GetCollection<EntityType>(GetCollectionName());
		}

		public abstract string GetCollectionName ();

		public async Task<IEnumerable<EntityType>> GetAll(string CollectionName)
		{
			try
			{
				Status = ServiceStatus.Ok;
				return await EntityCollection.Find(Item => true).ToListAsync();
			}
			catch (MongoConnectionException Ex)
			{
				Status = ServiceStatus.Error;
				string ErrorMessage = Ex.Message;
				return null;
			}
		}

		// Creates an entity and inserts it into the collection in MongoDB.
		public async void Create(EntityType InEntity)
		{
			try
			{
				Status = ServiceStatus.Ok;
				await EntityCollection.InsertOneAsync(InEntity);
			}
			catch (MongoCommandException Ex)
			{
				Status = ServiceStatus.Error;
				string ErrorMessage = Ex.Message;
			}
		}

		public async Task<EntityType> Replace(EntityType Entity)
		{
			try
			{
				Status = ServiceStatus.Ok;
				BsonDocument Filter = new BsonDocument("Name", Entity.Name);
				return await EntityCollection.FindOneAndReplaceAsync<EntityType>(Filter, Entity);
			}
			catch (MongoCommandException Ex)
			{
				Status = ServiceStatus.Error;
				string ErrorMessage = Ex.Message;
				return default(EntityType);
			}
		}

		public async Task<EntityType> Update(string Name)
		{
			try
			{
				Status = ServiceStatus.Ok;
				BsonDocument Filter = new BsonDocument("Name", Name);
				return await EntityCollection.FindOneAndDeleteAsync<EntityType>(Filter);
			}
			catch (MongoCommandException Ex)
			{
				Status = ServiceStatus.Error;
				string ErrorMessage = Ex.Message;
				return default(EntityType);
			}
		}
	}
}