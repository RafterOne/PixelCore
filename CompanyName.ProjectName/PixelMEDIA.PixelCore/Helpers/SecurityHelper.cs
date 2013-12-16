using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace PixelMEDIA.PixelCore.Helpers
{
	/// <summary>
	/// Helper library for common security-related functions.
	/// </summary>
	public static class SecurityHelper
	{
		/// <summary>
		/// Hash a password via SHA-256 with the given salt.
		/// </summary>
		/// <param name="password"></param>
		/// <param name="salt"></param>
		/// <returns></returns>
		public static string HashPassword(string password, string salt)
		{
			var hash = HashPassword(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt));
			return Convert.ToBase64String(hash);
		}

		/// <summary>
		/// Hash a password via SHA-256 with the given salt.
		/// </summary>
		/// <param name="password"></param>
		/// <param name="salt"></param>
		/// <returns></returns>
		private static byte[] HashPassword(byte[] password, byte[] salt)
		{
			byte[] saltedValue = password.Concat(salt).ToArray();
			return new SHA256Managed().ComputeHash(saltedValue);
		}

		/// <summary>
		/// Hash a set of strings via SHA-256.
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public static string HashStrings(params string[] values)
		{
			var concat = String.Concat(values);
			var concatBytes = Encoding.UTF8.GetBytes(concat);
			var hash = new SHA256Managed().ComputeHash(concatBytes);
			return Convert.ToBase64String(hash);
		}

	}
}
