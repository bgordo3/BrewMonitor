using System;
using System.IO.Ports;
using System.Collections.Generic;
using BrewMonitoring.Services;
using BrewMonitoring.Entities;
using System.Threading.Tasks;


namespace BrewMonitoring
{
	//Faire singleton
	public class HardwareManager
	{
		private Services.BatchService BatchService = new Services.BatchService();

		bool Stop = false;
		List<string> UsedPorts = new List<string> ();
		public List<Fermenter> Fermenters {get; private set;}

		private HardwareManager ()
		{
			Fermenters = new List<Fermenter> ();
			Task HardwareLoopTask = new Task(() => HardwareLoop(),  TaskCreationOptions.LongRunning);
			HardwareLoopTask.Start();
		}

		private static Object Lock = new Object();
		private static HardwareManager Instance = null;
		public static HardwareManager GetInstance()
		{
			if (Instance == null) 
			{
				lock (Lock) 
				{
					if (Instance == null) 
					{
						Instance = new HardwareManager ();
					}
				}
			}

			return Instance;
		}

		public async void HardwareLoop()
		{
			while (!Stop) 
			{
				await SaveBatches ();
				UpdateFermenters ();
				Discover();
				DissociateAllFermenter ();
				await UpdateBatchesWithThermometer ();
				await Task.Delay(TimeSpan.FromSeconds(5.0));
			}
		}

		public void Discover()
		{
			foreach (string PortName in SerialPort.GetPortNames()) 
			{
				if (UsedPorts.Contains (PortName)) 
				{
					continue;
				}
				Fermenter NewFermenter = new Fermenter (PortName);

				if (NewFermenter.Init()) 
				{
					UsedPorts.Add (PortName);
					Fermenters.Add (NewFermenter);
				}
			}
		}

		public async Task UpdateBatchesWithThermometer()
		{
			IEnumerable<Batch> Batches = await BatchService.GetAll ();
			foreach (Batch CurrBatch in Batches)
			{
				foreach (Fermenter CurrFermenter in Fermenters) 
				{
					if (CurrFermenter.Id == CurrBatch.FermentationMesures.HardwareID)
						CurrFermenter.CurrBatch = CurrBatch;
				}
			}
		}

		public async Task SaveBatches()
		{
			foreach (Fermenter CurrFermenter in Fermenters) 
			{
				if (CurrFermenter.CurrBatch != null) 
				{
					await BatchService.Replace (CurrFermenter.CurrBatch);
				}
			}
		}

		public void DissociateAllFermenter()
		{
			foreach (Fermenter CurrFermenter in Fermenters) 
			{
				CurrFermenter.CurrBatch = null;
			}
		}

		public void UpdateFermenters()
		{
			for (int i = Fermenters.Count - 1; i >= 0; --i) 
			{
				if (!Fermenters [i].IsAlive ()) 
				{
					UsedPorts.Remove (Fermenters [i].PortName);
					Fermenters.Remove (Fermenters [i]);
				}
			}
		}
	}
}

