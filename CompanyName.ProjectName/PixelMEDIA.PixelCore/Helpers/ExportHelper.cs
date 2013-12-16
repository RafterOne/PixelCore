using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PixelMEDIA.PixelCore.Helpers
{
    /// <summary>
    /// CSV utilities.
    /// </summary>
	public static class ExportHelper
	{
		//Credit: Harpo http://stackoverflow.com/a/4685745

		private const string QUOTE = "\"";
		private const string ESCAPED_QUOTE = "\"\"";
		private static char[] CHARACTERS_THAT_MUST_BE_QUOTED = { ',', '"', '\n' };

		/// <summary>
		/// Escape a field for inclusion in a CSV.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		private static string EscapeCsv(string s)
		{
			if (s == null)
			{
				return String.Empty;
			}

			if (s.Contains(QUOTE))
				s = s.Replace(QUOTE, ESCAPED_QUOTE);

			if (s.IndexOfAny(CHARACTERS_THAT_MUST_BE_QUOTED) > -1)
				s = QUOTE + s + QUOTE;

			return s;
		}


		private static string GetCsvLine(IEnumerable<string> values)
		{
			return String.Join(",", from o in values select EscapeCsv(o));
		}

		private static string GetCsvLine(IEnumerable<object> values)
		{
			return String.Join(",", from o in values select EscapeCsv(Convert.ToString(o)));
		}

		/// <summary>
		/// Generates a CSV from the publicly gettable properties of a collection. If no field names are specified, all field names are included.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="fieldNames"></param>
		/// <returns></returns>
		public static string GetCsv<T>(IEnumerable<T> collection, params string[] fieldNames)
		{
			Type ttype = typeof(T);
			var props = ReflectionHelper.GetGetableProperties(ttype, fieldNames);
			var sb = new StringBuilder();

			sb.Append(GetCsvLine(from prop in props select prop.Name));

			foreach (var item in collection)
			{
				sb.Append("\n");
				var line = GetCsvLine(from prop in props select prop.GetValue(item, null));
				sb.Append(line);
			}

			return sb.ToString();
		}


	}
}
