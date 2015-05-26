using System;

namespace BrewMonitoring
{
	public class Fermenter
	{
		string PortName;
		string Id;
		public Fermenter (string InPortName)
		{
			PortName = InPortName;
			//Check if Arduino has an ID. if not, assign him one. 
		}

		/// <summary>
		/// Returns the current temperature inside refrigerator
		/// </summary>
		/// <returns>The reading.</returns>
		public float GetReadingInRefrigerator()
		{
			return 21.0f;
		}

		/// <summary>
		/// Returns the current temperature of the beer
		/// </summary>
		/// <returns>The reading.</returns>
		public float GetReadingBeer()
		{
			return 21.0f;
		}

		/// <summary>
		/// Sets the beer temperature target.
		/// </summary>
		/// <param name="">.</param>
		public void SetTarget(float Target)
		{
		}
	}
}

