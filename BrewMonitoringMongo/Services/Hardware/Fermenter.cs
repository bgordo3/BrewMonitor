using System;
using System.IO.Ports;
using BrewMonitoring.Entities;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BrewMonitoring
{
	public class Fermenter
	{
		[ScriptIgnore]
		String CONNECT = "<c";
		[ScriptIgnore]
		String ACCEPT = "<a";
		//String KEEP_ALIVE = "<k";
		[ScriptIgnore]
		String SET_TARGET = "<t";
		[ScriptIgnore]
		String SET_ID = "<i";
		[ScriptIgnore]
		String GET_TEMP = "<g";
		[ScriptIgnore]
		String END_OF_LINE = "\n";

		public string PortName;
		[ScriptIgnore]
		public SerialPort Port;
		public string Id { get; set; }
		[ScriptIgnore]
		public Batch CurrBatch = null;
		//Use to make sure you set the string id on the right fermenter.
		public int TransientID { get;  set; }

		/// <summary>
		/// The data.
		/// </summary>
		/// 
		[ScriptIgnore]
		DataCurve Data = new DataCurve();

		public Fermenter ()
		{
		}

		public Fermenter (string InPortName)
		{
			PortName = InPortName;
			TransientID = (int)DateTime.Now.Ticks;
		}

		public bool Init()
		{
			//Ping the port to find out if we have a Arduino Fermenter.
			Port = new SerialPort(PortName);
			if (Port.IsOpen) 
			{
				return false;
			}

			try
			{
				//open serial port
				Port.Open();
				Port.BaudRate = 9600;
				Port.WriteTimeout = 1000;
				Port.ReadTimeout = 1000;
				Port.Write(CONNECT+ END_OF_LINE);
				String ReceivedString = Port.ReadTo(END_OF_LINE);
				if (ReceivedString == ACCEPT)
				{
					String ID = Port.ReadTo(END_OF_LINE);
					Id = ID;
					//Start reading job.
					Task TempReading = new Task(TempReadingJob,  TaskCreationOptions.LongRunning);
					TempReading.Start();
					return true;
				}
				else
				{
					Port.Close();
				}

				return false;
			}
			catch (TimeoutException)
			{
				Port.Close();
				return false;
			}
			catch (Exception)
			{
				Port.Close ();
				return false;
				//MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public async void TempReadingJob()
		{
			while (IsAlive())
			{
				lock (Port)
				{
					GetReadingInRefrigerator();
				}
				await Task.Delay(TimeSpan.FromSeconds(2.0));
			}
		}
		
		/// <summary>
		/// Returns the current temperature inside refrigerator
		/// </summary>
		/// <returns>The reading.</returns>
		public void GetReadingInRefrigerator()
		{
			if (!IsAlive())
				return;
			try
			{
				if (CurrBatch == null)
					return;
				
				Port.Write(GET_TEMP+ END_OF_LINE);
				string ReceivedString = Port.ReadTo(END_OF_LINE);
				float Temp;
				if (float.TryParse(ReceivedString, out Temp))
				{
					CurrBatch.FermentationMesures.Curve.SetValue(DateTime.Now.Ticks, Temp);
				}
				else
				{
					Port.Close();
				}
			}
			catch (Exception)
			{
				Port.Close ();
				//MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public DataCurve FlushData()
		{
			DataCurve Result = Data;
			Data = new DataCurve ();
			return Result;
		}

		/// <summary>
		/// Sets the beer temperature target.
		/// </summary>
		/// <param name="">.</param>
		public void SetTarget(float Target)
		{
			
			try
			{
				lock (Port)
				{
					if (!IsAlive())
						return;
					Port.Write(SET_TARGET+ END_OF_LINE);
					Port.Write(Target + END_OF_LINE);
				}
			}
			catch (Exception)
			{
				Port.Close ();
			}
		}
			
		public void SetID(string ID)
		{
			try
			{
				lock (Port)
				{
					if (!IsAlive())
						return;
					Port.Write(SET_ID+ END_OF_LINE);
					Port.Write(ID + END_OF_LINE);
				}
				Id = ID;
			}
			catch (Exception)
			{
				Port.Close ();
			}
		}

		public bool IsAlive()
		{
			return Port != null && Port.IsOpen;
		}
	}
}

