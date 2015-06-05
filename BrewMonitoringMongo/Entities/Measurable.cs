using System;

namespace BrewMonitoring.Entities
{
	public class Measurable
	{
		/// <summary>
		/// Usually the ID the arduino was assigned with.
		/// </summary>
		public string HardwareID { get; set; }
		public DataCurve Curve { get; set; }

		public Measurable()
		{
			Curve = new DataCurve ();
		}
	}
}

