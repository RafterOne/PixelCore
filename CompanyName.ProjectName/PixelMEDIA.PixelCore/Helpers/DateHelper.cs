using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace PixelMEDIA.PixelCore.Helpers
{
    /// <summary>
    /// Date utilities.
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Convert a date to a string. If date is null, return a null string.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string DateToString(DateTime? date)
        {
            if (date == null) return null;
            return Convert.ToString(date);
        }

        /// <summary>
        /// Get the short date string from a date as an object.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetShortDate(object date)
        {
           if (date == null) return null;
           return Convert.ToDateTime(date).ToShortDateString();
        }

        /// <summary>
        /// Get the formatted date string from a date as an object.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetDateFormatted(object date, string format)
        {
            if (date == null) return null;
            var tmp = Convert.ToDateTime(date);
            return tmp.ToString(format);
        }

        /// <summary>
        /// Get yesterday.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetYesterday()
        {
            return DateTime.Today.AddDays(-1);
        }

        /// <summary>
        /// Get tomorrow.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetTomorrow()
        {
            return DateTime.Today.AddDays(1);
        }

        /// <summary>
        /// Gets the first day of the current year.
        /// </summary>
        public static DateTime FirstDayOfYear()
        {
            return FirstDayOfYear(DateTime.Today);
        }

        /// <summary>
        /// Finds the first day of year of the specified day.
        /// </summary>
        public static DateTime FirstDayOfYear(DateTime y)
        {
            return new DateTime(y.Year, 1, 1);
        }

        /// <summary>
        /// Finds the last day of the year for today.
        /// </summary>
        public static DateTime LastDayOfYear()
        {
            return LastDayOfYear(DateTime.Today);
        }

        /// <summary>
        /// Finds the last day of the year for the selected day's year.
        /// </summary>
        public static DateTime LastDayOfYear(DateTime d)
        {
            // 1
            // Get first of next year
            DateTime n = new DateTime(d.Year + 1, 1, 1);
            // 2
            // Subtract 1 from it
            return n.AddDays(-1);
        }

        /// <summary>
        /// Returns the last day of the month.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
		public static DateTime EndOfMonth(DateTime d)
		{
			DateTime n = new DateTime(d.Year, d.Month + 1, 1, 23, 59, 59);
			return n.AddDays(-1);
		}


        /// <summary>
        /// Returns the last day of the current month.
        /// </summary>
        /// <returns></returns>
		public static DateTime EndOfMonth()
		{
			return EndOfMonth(DateTime.UtcNow);
		}

        /// <summary>
        /// Returns the first day of the month.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
		public static DateTime StartOfMonth(DateTime d)
		{
			return new DateTime(d.Year, d.Month, 1);
		}

        /// <summary>
        /// Returns the first day of the current month.
        /// </summary>
        /// <returns></returns>
		public static DateTime StartOfMonth()
		{
			return StartOfMonth(DateTime.UtcNow);
		}

        /// <summary>
        /// Returns an indication whether the year passed as method parameter is a leap year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        /// <summary>
        /// This method will return an integer representing the number of days between two
        /// particular dates passed to a method
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetDaysDifference(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDifference = endDate - startDate;

            try
            {
                return Convert.ToInt32(Math.Round(timeDifference.TotalDays));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
		public static List<DateTime> GetDaysInRangeInclusive(DateTime startDate, DateTime endDate)
		{
			List<DateTime> days = new List<DateTime>();

			var dayCount = GetDaysDifference(startDate, endDate);

			for (var i = 0; i <= dayCount; i++ )
			{
				days.Add(startDate.AddDays(i));
			}

			return days;
		}

		/// <summary>
		/// Returns the full, localized name for the given month
		/// </summary>
		/// <param name="month">Number between 1 and 12</param>
		/// <returns></returns>
		public static string GetMonthName(int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

        }
    }
}
