using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using MongoDB.Bson;
using BrewMonitoring.Entities;

namespace BrewMonitoring.Controllers
{
	public class BatchController : Controller
    {
		protected Services.BatchService BatchService = new Services.BatchService();
        public async Task<ActionResult> Index()
        {
			return Json(await BatchService.GetAll(), JsonRequestBehavior.AllowGet);
        }

		public async Task<ActionResult> CurrentBatches()
		{
			return Json(await BatchService.GetAll(), JsonRequestBehavior.AllowGet);
		}

		public async Task<ActionResult> Get(String id)
		{
			if (id != null) 
			{
				ObjectId InID = new ObjectId (id);
				return Json(await BatchService.Get(InID), JsonRequestBehavior.AllowGet);
			}

			return new JsonResult ();
		}

		[HttpPost] 
		public async Task<ActionResult> Update(Batch InBatch)
		{
			//Batch InBatch = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Batch>(InJson);
			if (InBatch != null) 
			{
				return Json(await BatchService.Replace(InBatch), JsonRequestBehavior.DenyGet);
			}

			return new JsonResult ();
		}

		[HttpPost] 
		public async Task<ActionResult> Delete(String id)
		{
			//Batch InBatch = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Batch>(InJson);
			ObjectId InID = new ObjectId (id);
			if (InID != null) 
			{
				
				return Json(await BatchService.Delete(InID), JsonRequestBehavior.DenyGet);
			}

			return new JsonResult ();
		}

		public async Task<ActionResult> Create()
		{
			Batch NewBatch = new Batch ();
			NewBatch.Name = "Une nouvelle batch";
			return Json(await BatchService.Create(NewBatch), JsonRequestBehavior.AllowGet);
		}
    }
}
