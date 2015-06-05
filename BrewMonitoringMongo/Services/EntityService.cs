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

		public async Task<IEnumerable<EntityType>> GetAll()
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

		public async Task<EntityType> Get(ObjectId _id)
		{
			try
			{
				Status = ServiceStatus.Ok;
				return (await EntityCollection.Find(Item => Item._id == _id).ToListAsync()).FirstOrDefault();
			}
			catch (MongoConnectionException Ex)
			{
				Status = ServiceStatus.Error;
				string ErrorMessage = Ex.Message;
				return default(EntityType);
			}
		}

		// Creates an entity and inserts it into the collection in MongoDB.
		public async Task<EntityType> Create(EntityType InEntity)
		{
			try
			{
				Status = ServiceStatus.Ok;
				await EntityCollection.InsertOneAsync(InEntity);
				return await Get(InEntity._id);
			}
			catch (MongoCommandException Ex)
			{
				Status = ServiceStatus.Error;
				string ErrorMessage = Ex.Message;
				return default(EntityType);
			}
		}

		public async Task<EntityType> Replace(EntityType InEntity)
		{
			try
			{
				Status = ServiceStatus.Ok;
				BsonDocument Filter = new BsonDocument("_id", InEntity._id);
				await EntityCollection.FindOneAndReplaceAsync<EntityType>(Filter, InEntity);
				return await Get(InEntity._id);
			}
			catch (MongoCommandException Ex)
			{
				Status = ServiceStatus.Error;
				string ErrorMessage = Ex.Message;
				return default(EntityType);
			}
		}

		public async Task<EntityType> Delete(ObjectId _id)
		{
			try
			{
				Status = ServiceStatus.Ok;
				BsonDocument Filter = new BsonDocument("_id", _id);
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