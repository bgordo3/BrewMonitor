using System;

namespace BrewMonitoring.Entities
{
	public class Measurable
	{
		/// <summary>
		/// Usually the ID the arduino was assigned with.
		/// </summary>
		public string HardwareID { get; set; }
		public DataCurve Values { get; set; }
	}
}

