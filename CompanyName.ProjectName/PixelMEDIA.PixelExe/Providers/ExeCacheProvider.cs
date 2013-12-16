using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PixelMEDIA.PixelCore.Interfaces;

namespace PixelExe.Providers
{
    /// <summary>
    /// Provides a dictionary-based cached for non-web enviroments (such as Windows/Console/Service applications).
    /// </summary>
	public class ExeCacheProvider : ICacheProvider
	{
		private Dictionary<string, object> SessionCache { get; set; }
		private Dictionary<string, object> ApplicationCache { get; set; }

        /// <summary>
        /// Create a new ExeCacheProvider.
        /// </summary>
		public ExeCacheProvider()
		{
			this.SessionCache = new Dictionary<string, object>();
			this.ApplicationCache = new Dictionary<string, object>();
		}

		/// <summary>
		/// Returns a user session object for the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object GetSessionCacheItem(string key)
		{
			object value;
			if (this.SessionCache.TryGetValue(key, out value))
			{
				return value;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Returns a typed user session object for the given key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T GetSessionCacheItem<T>(string key)
		{
			return (T)GetSessionCacheItem(key);
		}

		/// <summary>
		/// Sets a typed user session object for the given key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public T SetSessionCacheItem<T>(string key, T value)
		{
			this.SessionCache[key] = value;
			return value;
		}

		/// <summary>
		/// Returns true if the user session cache has a non-null value for the key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool HasSessionCacheItem(string key)
		{
			return this.GetSessionCacheItem(key) != null;
		}

		/// <summary>
		/// Returns a global application object for the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object GetApplicationCacheItem(string key)
		{
			object value;
			if (this.ApplicationCache.TryGetValue(key, out value))
			{
				return value;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Returns a typed global application object for the given key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T GetApplicationCacheItem<T>(string key)
		{
			return (T)GetApplicationCacheItem(key);
		}

		/// <summary>
		/// Sets a typed global application object for the given key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public T SetApplicationCacheItem<T>(string key, T value)
		{
			this.ApplicationCache[key] = value;
			return value;
		}

		/// <summary>
		/// Returns true if the application cache has a non-null value for the key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool HasApplicationCacheItem(string key)
		{
			return this.GetApplicationCacheItem(key) != null;
		}
	}
}
