using System;
using System.IO.Ports;
using System.Collections.Generic;


namespace BrewMonitoring
{
	//Faire singleton
	public class HardwareManager
	{
		bool Stop = false;
		List<string> UsedPorts;
		Dictionary<String, Fermenter> Fermenters;

		private HardwareManager ()
		{
			
		}

		public void Discover()
		{
			UsedPorts = new List<string> ();
			foreach (string PortName in SerialPort.GetPortNames()) 
			{
				if (!UsedPorts.Contains (PortName)) 
				{
					continue;
				}
				Fermenter NewFermenter = new Fermenter (PortName);

				if (NewFermenter.Init()) 
				{
					UsedPorts.Add (PortName);
					Fermenters.Add (PortName, NewFermenter);
				}
			}

			//sleep 5000
			if (Stop)
				return;
			Discover();
		}

		public void UpdateBatchWithThermometer()
		{
			//flush data of all thermother associated with bath in batch
		}

		public void UpdateFermenter()
		{
			//For each existing fermenter, check if dead, if so, removes it.
		}
	}
}

