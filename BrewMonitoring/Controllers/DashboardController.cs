﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using BrewMonitoring.ActionFilters;

namespace BrewMonitoring.Controllers
{
	public class DashboardController : Controller
	{
		public ActionResult Index()
		{
			return View ();
		}
	}
}
