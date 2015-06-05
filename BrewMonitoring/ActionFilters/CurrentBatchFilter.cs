using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Threading;

namespace BrewMonitoring.ActionFilters
{
	public class CurrentBatchFilter : System.Web.Mvc.ActionFilterAttribute, System.Web.Mvc.IActionFilter
    {
		protected Services.BatchService BatchService = new Services.BatchService();

		void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)

		{
			//Might have performence impact. fuck off.
			var BatchTask = Task.Run(() => BatchService.GetAll ());
			BatchTask.Wait();
			filterContext.Controller.ViewBag.CurrentBatches = BatchTask.Result;

			this.OnActionExecuting(filterContext);
		}
    }
}
