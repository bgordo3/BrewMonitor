using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrewMonitoring.Controllers
{
    public class BatchController : Controller
    {
		private Services.BatchService DAL = new Services.BatchService();
        public ActionResult Index()
        {
            return View ();
        }
    }
}
