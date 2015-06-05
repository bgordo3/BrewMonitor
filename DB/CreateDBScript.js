var db = connect("127.0.0.1:27017/Brewing");

if (db.system.namespace.find( { name: 'Brewing.' + "Recipe" } ))
{
	db.Recipe.drop();
}
db.createCollection("Recipe");
db.Recipe.save( {
	_id:ObjectId("507f1f77bcf86cd799439011"),
	Name : "TestRecipe",
	BeerStyle:0,
	BeerType:0
});

if (db.system.namespace.find( { name: 'Brewing.' + "Batch" } ))
{
	db.Batch.drop();
}
db.createCollection("Batch");

db.Batch.save( {
	Name : "TestBatch",
	IsDone:true,
	BeerRecipe:ObjectId("507f1f77bcf86cd799439011")
});

if (db.system.users.find({user:'pikaille'}).count() == 0)
{
	db.createUser(
	{ 
		user: "pikaille",
		pwd: "Miowmix01",
		customData: { },
		roles: [{ role: "readWrite", db: "Brewing" }  ]
	});
}

