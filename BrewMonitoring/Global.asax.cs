
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MongoDB.Bson;

namespace BrewMonitoring
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

			routes.MapRoute (
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Dashboard", action = "Index", id = "" }
			);

		}

		public static void RegisterGlobalFilters (GlobalFilterCollection filters)
		{
			filters.Add (new HandleErrorAttribute ());
		}

		protected void Application_Start ()
		{
			AreaRegistration.RegisterAllAreas ();
			RegisterGlobalFilters (GlobalFilters.Filters);
			RegisterRoutes (RouteTable.Routes);
			ModelBinders.Binders.Add(typeof(ObjectId), new ObjectIdBinder());
		}
	}
}
