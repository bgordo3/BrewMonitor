using System.Linq;
using System;
using System.Reflection;

namespace BrewMonitoring.Helpers
{
	public static class  AttributeHelper
	{
		/// <summary>
		///     A generic extension method that aids in reflecting 
		///     and retrieving any attribute that is applied to an `Enum`.
		/// </summary>
		public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) 
			where TAttribute : Attribute
		{
			return enumValue.GetType()
				.GetMember(enumValue.ToString())
				.First()
				.GetCustomAttribute<TAttribute>();
		}
	}
}

