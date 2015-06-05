using System;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BrewMonitoring.Services
{
	public class MongoConnection
	{
		//Connexion info
		const string DB_NAME = "Brewing";
		const string PASSWORD = "Miowmix01";
		const string USERNAME = "pikaille";
		const string HOST = "127.0.0.1" ;

		private MongoClient Client = null;
		public IMongoDatabase Database = null;

		private MongoConnection ()
		{
			//Connect to DB
			MongoClientSettings ClientSettings = new MongoClientSettings 
			{ 
				Credentials = new List<MongoCredential>() {MongoCredential.CreateCredential(DB_NAME, USERNAME, PASSWORD)}, 
				Server = new MongoServerAddress(HOST)
			};  
			Client = new MongoClient (ClientSettings);

			//Fetch DB
			Database = Client.GetDatabase (DB_NAME);
		}

		private static MongoConnection Instance = null;
		private static Object Lock = new object ();
		public static MongoConnection GetInstance()
		{
			if (Instance == null) 
			{
				lock(Lock)
				{
					if (Instance == null) 
					{
						Instance = new MongoConnection ();
					}
				}
			}
			return Instance;
		}
	}
}

