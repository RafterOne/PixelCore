using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using PixelMEDIA.PixelCore.Interfaces;

namespace PixelMEDIA.PixelWeb.Providers
{
	/// <summary>
	/// Wraps the ASP.NET Session and Application collections.
	/// </summary>
	public class WebCacheProvider : ICacheProvider
	{
		private HttpContext Context
		{
			get { return HttpContext.Current; }
		}

		/// <summary>
		/// Returns a user session object for the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object GetSessionCacheItem(string key)
		{
			return this.Context.Session[key];
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
			this.Context.Session[key] = value;
			return value;
		}

		/// <summary>
		/// Returns true if the user session cache has a non-null value for the key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool HasSessionCacheItem(string key)
		{
			return this.Context.Session[key] != null;
		}

		/// <summary>
		/// Returns a global application object for the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object GetApplicationCacheItem(string key)
		{
			return this.Context.Application[key];
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
			this.Context.Application[key] = value;
			return value;
		}

		/// <summary>
		/// Returns true if the application cache has a non-null value for the key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool HasApplicationCacheItem(string key)
		{
			return this.Context.Application[key] != null;
		}

	}
}