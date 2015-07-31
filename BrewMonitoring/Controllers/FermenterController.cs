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
	public class FermenterController : Controller
    {
		protected Services.BatchService BatchService = new Services.BatchService();
		public ActionResult Index()
        {
			return Json(HardwareManager.GetInstance().GetFermenters(), JsonRequestBehavior.AllowGet);
        }

		public async Task<ActionResult> SetID(Fermenter InFermenter)
		{
			for (int i = 0; i < HardwareManager.GetInstance().GetFermenterCount(); ++i )
			{
				Fermenter CurrFermenter = HardwareManager.GetInstance().GetFermenter(i);
				if (CurrFermenter.TransientID == InFermenter.TransientID)
				{
					CurrFermenter.SetID (InFermenter.Id);
					if (CurrFermenter.CurrBatch != null) 
					{
						CurrFermenter.CurrBatch.FermentationMesures.HardwareID =  CurrFermenter.Id;
						await BatchService.Replace(CurrFermenter.CurrBatch);
					}

				}
			}
			return Json(HardwareManager.GetInstance().GetFermenters(), JsonRequestBehavior.AllowGet);
		}
    }
}
