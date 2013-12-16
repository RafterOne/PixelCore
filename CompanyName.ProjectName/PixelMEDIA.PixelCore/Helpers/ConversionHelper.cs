using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JsonDict = System.Collections.Generic.IDictionary<string, object>;
using JsonDictImpl = System.Collections.Generic.Dictionary<string, object>;

namespace PixelMEDIA.PixelCore.Helpers
{
	/// <summary>
	/// Helper library for converting things into other things.
	/// </summary>
	public static class ConversionHelper
	{
		/// <summary>
		/// Makes a JsonDict (string, object) from a dictionary of (string, someOtherType) pairs.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dict"></param>
		/// <returns></returns>
		public static JsonDict MakeJsonDict<T>(IDictionary<string, T> dict)
		{
			var jsonDict = new JsonDictImpl();

			foreach (var kvp in dict)
			{
				jsonDict.Add(kvp.Key, kvp.Value);
			}

			return jsonDict;
		}

		#region guid conversion

		/// <summary>
		/// Converts an array of strings to an enumerable of guids. Excludes improper guids.
		/// </summary>
		/// <param name="strings"></param>
		/// <returns></returns>
		public static IEnumerable<Guid> SafeConvertGuidArray(string[] strings)
		{
			return (from s in strings
					select ConversionHelper.SafeConvertGuid(s)).Where(g => g != Guid.Empty);
		}

		/// <summary>
		/// Converts a string to a guid, or returns Guid.Empty.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static Guid SafeConvertGuid(string s)
		{
			return SafeConvertGuid(s, Guid.Empty);
		}

		/// <summary>
		/// Converts a string to a guid, or returns the default value.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static Guid SafeConvertGuid(string s, Guid defaultValue)
		{
			Guid returnGuid;

			if (Guid.TryParse(s, out returnGuid))
			{
				return returnGuid;
			}
			else
			{
				return defaultValue;
			}
		}

		#endregion


		#region decimal conversion

		/// <summary>
		/// Converts an integer into a string. If it can't, returns 0.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static decimal SafeConvertDecimal(string s)
		{
			return SafeConvertDecimal(s, 0);
		}

		/// <summary>
		/// Converts a decimal into a string. If it can't, returns defaultValue.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static decimal SafeConvertDecimal(string s, decimal defaultValue)
		{
			decimal result;

			if (Decimal.TryParse(s, out result))
			{
				return result;
			}
			else
			{
				return defaultValue;
			}
		}

		#endregion


		#region int conversion

		/// <summary>
		/// Converts an integer into a string. If it can't, returns 0.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static int SafeConvertInt(string s)
		{
			return SafeConvertInt(s, 0);
		}

		/// <summary>
		/// Converts an integer into a string. If it can't, returns defaultValue.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int SafeConvertInt(string s, int defaultValue)
		{
			int result;

			if (Int32.TryParse(s, out result))
			{
				return result;
			}
			else
			{
				return defaultValue;
			}
		}
        
        /// <summary>
		/// Return the integer parsed from the object, or the defaultValue if it can't parse it.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int SafeConvertInt(object o, int defaultValue)
		{
			return SafeConvertInt(Convert.ToString(o), defaultValue);
		}

		/// <summary>
		/// Return the integer parsed from the object, or 0 if it can't parse it.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static int SafeConvertInt(object o)
		{
			return SafeConvertInt(Convert.ToString(o), 0);
		}

		#endregion


		#region Enum Conversion

		/// <summary>
		/// Returns the string version of an enum value based on the provided integer.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumTextValueFromInt<TEnum>(int value)
		{
			return GetEnumFromInt<TEnum>(value).ToString();
		}

		/// <summary>
		/// Returns an enum from the provided integer.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TEnum GetEnumFromInt<TEnum>(int value)
		{
			return (TEnum)Enum.ToObject(typeof(TEnum), value);
		}

		/// <summary>
		/// Returns the human-friendly version of the provided enum.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetLabelFromEnum<TEnum>(TEnum value)
		{
			return GetLabelFromPascalCase(value.ToString());
		}

		/// <summary>
		/// Returns the text version of the provided enum.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumTextValueFromEnum<TEnum>(TEnum value)
		{
			return value.ToString();
		}

		#endregion


		#region String Conversion

		/// <summary>
		/// Splits a pascal-case string into its component words. Inserts a space wherever an upper case letter follows a lower case letter.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string GetLabelFromPascalCase(string str)
		{
			return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + m.Value[1]);
		}

		/// <summary>
		/// Returns a non-null string based on the provided argument.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string SafeConvertString(string str)
		{
			return String.IsNullOrEmpty(str) ? String.Empty : str;
		}

		/// <summary>
		/// Returns a non-null string based on the provided argument.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string SafeConvertString(object obj)
		{
			return Convert.ToString(obj);
		}

		#endregion


		 /// <summary>
        /// Converts a string into a DateTime. If it can't, returns defaultValue.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime SafeConvertDate(string s, DateTime defaultValue)
        {
            DateTime result;
            try{
                result = DateTime.Parse(s);
            }
            catch (FormatException){
           
                result = defaultValue;
            }
            return result;
        }
	}
}
