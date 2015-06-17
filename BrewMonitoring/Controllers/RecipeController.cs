using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace BrewMonitoring.Controllers
{
	public class RecipeController : Controller
    {

		protected Services.RecipeService RecipeService = new Services.RecipeService();
        
		public async Task<ActionResult> Index()
        {
			return Json(await RecipeService.GetAll(), JsonRequestBehavior.AllowGet);
        }

		public async Task<ActionResult> Get(String id)
		{
			if (id != null) 
			{
				ObjectId InID = new ObjectId (id);
				return Json(await RecipeService.Get(InID), JsonRequestBehavior.AllowGet);
			}

			return new JsonResult ();
		}
    }
}
