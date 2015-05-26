using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace BrewMonitoring.Entities
{
	public class DataCurve
	{
		/// <summary>
		/// The values.
		/// key is always time in millis. Starts at 0.
		/// </summary>
		public List<Tuple<long, float>> Values { get; protected set; }

		public DataCurve ()
		{
			Values = new List<Tuple<long, float>> ();
		}

		/// <summary>
		/// Compute the distance between the two curves at the last point of the "Other"
		/// </summary>
		/// <param name="Other">Other.</param>
		public float Dist(DataCurve Other)
		{
			Tuple<long, float> OtherLast = Other.Values.Last ();
			Tuple<long, float> Previous = Values.First();

			foreach (Tuple<long, float> Curr in Values) 
			{
				if (Curr.Item1 > OtherLast.Item1) 
				{
					float Lerp = (Curr.Item2 - Previous.Item2) / (Curr.Item1 - Previous.Item1) * (OtherLast.Item1 - Previous.Item1);
					return Previous.Item2 + Lerp;
				}

				Previous = Curr;
			}

			return 0.0f;
		}

		public void SetValue(long Time, float Value)
		{
			Values.Add (new Tuple<long, float>(Time, Value));
		}
	}
}

