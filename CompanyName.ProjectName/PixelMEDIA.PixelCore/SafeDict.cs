using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsonDict = System.Collections.Generic.IDictionary<string, object>;
using JsonDictImpl = System.Collections.Generic.Dictionary<string, object>;
using PixelMEDIA.PixelCore.Helpers;

namespace PixelMEDIA.PixelCore
{

	/// <summary>
	/// A resilient wrapper for JSON returned from external sources.
	/// </summary>
	public class SafeDict
	{
		private JsonDict WrappedDictionary { get; set; }

        /// <summary>
        /// Creates a new SafeDict.
        /// </summary>
        /// <param name="dict"></param>
		public SafeDict(JsonDict dict)
		{
			if (dict != null)
			{
				this.WrappedDictionary = dict;
			}
			else
			{
				this.WrappedDictionary = new JsonDictImpl();
			}
		}


        /// <summary>
        /// Create a SafeDict from a string-keyed dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
		public static SafeDict FromDict<T>(IDictionary<string, T> dict)
		{
			return new SafeDict(ConversionHelper.MakeJsonDict(dict));
		}

		/// <summary>
		/// Returns a SafeDict at the given key. If there is no underlying JSON dictionary, then just returns an empty SafeDict.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public SafeDict GetDict(string key)
		{
		    object value;

		    return this.WrappedDictionary.TryGetValue(key, out value) ? new SafeDict(value as JsonDict) : new SafeDict(new JsonDictImpl());
		}

	    /// <summary>
		/// Gets a string for the given key, or returns String.Empty if it is missing.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetString(string key) { return this.GetValue(key, String.Empty); }

		/// <summary>
		/// Gets a string for the given key, or returns defaultValue if it is missing.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public string GetString(string key, string defaultValue) { return this.GetValue(key, defaultValue); }

		/// <summary>
		/// Gets a long for the given key, or returns 0.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public long GetLong(string key) { return this.GetValue<long>(key); }

		/// <summary>
		/// Gets a long for the given key, or returns defaultValue if it is missing.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public long GetLong(string key, long defaultValue) { return this.GetValue(key, defaultValue); }

		/// <summary>
		/// Gets an int for the given key, or returns 0.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public long GetInt(string key) { return this.GetValue<int>(key); }

		/// <summary>
		/// Gets an int for the given key, or returns defaultValue if it is missing.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public long GetInt(string key, int defaultValue) { return this.GetValue(key, defaultValue); }

		/// <summary>
		/// Gets a bool for the given key, or returns false if it is missing.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool GetBool(string key) { return this.GetValue(key, false); }

		/// <summary>
		/// Gets a bool for the given key, or returns defaultValue if it is missing.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public bool GetBool(string key, bool defaultValue) { return this.GetValue(key, defaultValue); }

		/// <summary>
		/// Gets a value for the given key, or returns the default of T if it is missing.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T GetValue<T>(string key) { return this.GetValue(key, default(T)); }

		/// <summary>
		/// Gets a value for the given key, or defaultValue if it is missing. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public T GetValue<T>(string key, T defaultValue)
		{
			object value;

			if (this.WrappedDictionary.TryGetValue(key, out value))
			{
				if (value is T)
				{
					return (T)value;
				}
				else
				{
					return defaultValue;
				}
			}
			else
			{
				return defaultValue;
			}
		}

	}
}