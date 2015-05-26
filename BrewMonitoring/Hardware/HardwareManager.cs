using System;
using System.IO.Ports;
using System.Collections.Generic;


namespace BrewMonitoring
{
	public class HardwareManager
	{
		List<string> UsedPorts;
		List<Fermenter> Fermenters;

		public HardwareManager ()
		{
			UsedPorts = new List<string> ();
			foreach (string PortName in SerialPort.GetPortNames()) 
			{
				if (IsUnregisteredFermenter (PortName)) 
				{
					UsedPorts.Add (PortName);
					Fermenters.Add (new Fermenter (PortName));
				}
			}
		}

		private bool IsUnregisteredFermenter(string PortName)
		{
			//Ping the port to find out if we have a Arduino Fermenter.
			return false;
		}
	}
}

