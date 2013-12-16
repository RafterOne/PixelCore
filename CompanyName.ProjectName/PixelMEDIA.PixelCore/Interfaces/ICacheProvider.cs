using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PixelMEDIA.PixelCore.Interfaces
{
	/// <summary>
	/// Provides an interface for a cross-environment caching mechanism.
	/// </summary>
	public interface ICacheProvider
	{
        /// <summary>
        /// Get a session item of type T at this key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
		T GetSessionCacheItem<T>(string key);

        /// <summary>
        /// Get a session object at this key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		object GetSessionCacheItem(string key);

        /// <summary>
        /// Set a session item of type T a this key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
		T SetSessionCacheItem<T>(string key, T value);

        /// <summary>
        /// Check a session object at this key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		bool HasSessionCacheItem(string key);

        /// <summary>
        /// Get an application item of type T at this key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
		T GetApplicationCacheItem<T>(string key);

        /// <summary>
        /// Get an application object at this key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		object GetApplicationCacheItem(string key);

        /// <summary>
        /// Set an application item of type T a this key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
		T SetApplicationCacheItem<T>(string key, T value);

        /// <summary>
        /// Check an application object at this key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		bool HasApplicationCacheItem(string key);
	}
}
