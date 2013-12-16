using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PixelMEDIA.PixelCore.Helpers
{
	/// <summary>
	/// Helper library for string manipulation.
	/// </summary>
	public static class StringHelper
	{
		/// <summary>
		/// Returns the content within the text parameter between the start and end parameters.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string ExtractBetween(string start, string end, string text)
		{
			Regex reg = new Regex(String.Format("{0}(.+?){1}", Regex.Escape(start), Regex.Escape(end)));
			var match = reg.Match(text);
			return (match.Groups.Count > 1) ? match.Groups[1].Value : String.Empty;
		}


		/// <summary>
		/// Returns the number of words in a string.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static int GetWordCount(string text)
		{
			if (String.IsNullOrEmpty(text))
			{
				return 0;
			}
			else
			{
				return text.Split(new char[] { ' ', '.', ',', '?', '!', '(', ')', '/', '\'', '"', '-' }, StringSplitOptions.RemoveEmptyEntries).Length;
			}
		}

        /// <summary>
        /// Builds a dictionary from a delimited list of keys and values.
        /// </summary>
        /// <param name="text">The raw text to extract key-value pairs from.</param>
        /// <param name="keyValueSeparator">The delimiter between keys and values.</param>
        /// <returns>A dictionary of string keys and values.</returns>
		public static Dictionary<string, string> ExtractKeyValuePairs(string text, string keyValueSeparator)
		{
			var dict = new Dictionary<string, string>();

			var lines = text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var line in lines)
			{
				var kvp = line.Split(new string[] { keyValueSeparator }, 2, StringSplitOptions.None);

				string key;

				if (kvp.Length > 0)
				{
					key = kvp[0];
					var val = (kvp.Length > 1) ? SafeTrim(kvp[1]) : null;

					dict.Add(key, val);
				}
			}

			return dict;
		}
        
        /// <summary>
        /// Returns a trimmed string, even if it is null.
        /// </summary>
        /// <param name="s">The string to trim.</param>
        /// <returns>A trimmed string, or String.Empty if it is null.</returns>
		public static string SafeTrim(this string s)
		{
			return String.IsNullOrEmpty(s) ? String.Empty : s.Trim();
		}


		/// <summary>
		/// Truncate a string greater than the given length and add ellipsis (...). Otherwise, return the string.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string Ellipsize(string s, int length)
		{
			if (String.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			else if (s.Length < length)
			{
				return s;
			}
			else
			{
				return s.Substring(0, length) + "...";
			}
		}

		/// <summary>
		/// Truncate a string greater than the given length. Otherwise, return the string.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string Left(string s, int length)
		{
			if (String.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			else if (s.Length < length)
			{
				return s;
			}
			else
			{
				return s.Substring(0, length);
			}
		}

	}
}
