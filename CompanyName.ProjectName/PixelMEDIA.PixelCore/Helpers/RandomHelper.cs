using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PixelMEDIA.PixelCore.Helpers
{
	/// <summary>
	/// Helper library for generating random values.
	/// </summary>
	public static class RandomHelper
	{
		private static readonly Random _random = new Random();
		private const string _alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

		/// <summary>
		/// Returns a random string of the given size composed of numbers and upper and lower case letters.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static string GetRandomAlphanumericString(int size)
		{
			char[] buffer = new char[size];

			for (int i = 0; i < size; i++)
			{
				buffer[i] = _alphanumeric[_random.Next(_alphanumeric.Length)];
			}

			return new string(buffer);
		}

	}
}
