using System;
using System.Web.Mvc;
using MongoDB.Bson;

namespace BrewMonitoring
{
	public class ObjectIdBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			return new ObjectId(result.AttemptedValue);
		}
	}
}

