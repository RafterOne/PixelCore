using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace PixelMEDIA.PixelCore.Helpers
{
	/// <summary>
	/// Helper library for getting web.config/app.config settings.
	/// </summary>
	public static class SettingsHelper
	{
		/// <summary>
		/// Gets a value from appSettings as a string, or returns null if it can't match the key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string Get(string key)
		{
			return ConfigurationManager.AppSettings.Get(key);
		}

		/// <summary>
		/// Gets a value from appSettings as an int, or returns 0 if it can't match the key or convert the value to an int.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static int GetInt(string key)
		{
			return GetInt(key, 0);
		}

		/// <summary>
		/// Gets a value from appSettings as an int, or returns defaultValue if it can't match the key or convert the value to an int.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int GetInt(string key, int defaultValue)
		{
			return ConversionHelper.SafeConvertInt(Get(key), defaultValue);
		}

		/// <summary>
		/// Gets a value from appSettings as a string, or returns defaultValue if it can't match the key or the value is null or empty.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string Get(string key, string defaultValue)
		{
			var match = ConfigurationManager.AppSettings.Get(key);

			return String.IsNullOrEmpty(match) ? defaultValue : match;
		}
	}
}
