using System;
using System.IO.Ports;
using BrewMonitoring.Entities;

namespace BrewMonitoring
{
	public class Fermenter
	{

		string CONNECT = "<c";
		string ACCEPT = "<a";
		string KEEP_ALIVE = "<k";
		char END_OF_LINE = '\n';
		String SET_ID = "<i";

		string PortName;
		SerialPort Port;
		string Id;

		/// <summary>
		/// The data.
		/// </summary>
		DataCurve Data = new DataCurve();

		public Fermenter (string InPortName)
		{
			PortName = InPortName;
			//Check if Arduino has an ID. if not, assign him one. 
		}

		public bool Init()
		{
			//Ping the port to find out if we have a Arduino Fermenter.
			SerialPort Port = new SerialPort(PortName);
			if (Port.IsOpen) 
			{
				return false;
			}

			try
			{
				//open serial port
				Port.Open();
				Port.BaudRate = 9600;
				Port.WriteTimeout = 10;
				Port.ReadTimeout = 10;
				Port.Write(CONNECT+ END_OF_LINE);
				String ReceivedString = Port.ReadTo(END_OF_LINE);
				if (ReceivedString == ACCEPT)
				{
					String ID = Port.ReadTo(END_OF_LINE);
					if (ID.StartsWith("͡° ͜ʖ ͡°"))
					{
						Id = "͡° ͜ʖ ͡°";
					}
					else
					{
						Id = ID;
					}
					//Start reading job.
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
			catch (Exception ex)
			{
				Port.Close ();
				return false;
				//MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		/// <summary>
		/// Returns the current temperature inside refrigerator
		/// </summary>
		/// <returns>The reading.</returns>
		public float GetReadingInRefrigerator()
		{
			try
			{
				//open serial port
				String ReceivedString = Port.ReadTo(END_OF_LINE);
				float Temp;
				if (float.TryParse(ReceivedString, Temp))
				{
					Data.SetValue(new DateTime().Ticks, Temp);
				}
				else
				{
					Port.Close();
				}
			}
			catch (TimeoutException)
			{
				Port.Close();
			}
			catch (Exception ex)
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
		}
	}
}

