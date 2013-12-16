using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PixelMEDIA.PixelCore.Helpers
{
	/// <summary>
	/// Helper library for logging.
	/// </summary>
	public static class Logger
	{
        /// <summary>
        /// Client properties for the logger.
        /// </summary>
		public static IClientPropertyProvider ClientProperties { get; set; }

		/// <summary>
		/// Call this from your global.asax or other application initializer to get useful logging details.
		/// </summary>
		/// <param name="clientProperties"></param>
		public static void SetClientPropertyProvider(IClientPropertyProvider clientProperties)
		{
			ClientProperties = clientProperties;
		}

        /// <summary>
        /// Log an Info-level event.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
		public static void Info(string format, params object[] args) { Log(LogEntryType.Information, format, args); }
        /// <summary>
        /// Log an Info-level event.
        /// </summary>
        /// <param name="message"></param>
		public static void Info(string message) { Log(LogEntryType.Information, message); }
        /// <summary>
        /// Log an Info-level event.
        /// </summary>
        /// <param name="ex"></param>
		public static void Info(Exception ex) { Log(LogEntryType.Information, ex); }

        /// <summary>
        /// Log a Warn-level event.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
		public static void Warn(string format, params object[] args) { Log(LogEntryType.Warning, format, args); }
        /// <summary>
        /// Log a Warn-level event.
        /// </summary>
        /// <param name="message"></param>
		public static void Warn(string message) { Log(LogEntryType.Warning, message); }
        /// <summary>
        /// Log a Warn-level event.
        /// </summary>
        /// <param name="ex"></param>
		public static void Warn(Exception ex) { Log(LogEntryType.Warning, ex); }

        /// <summary>
        /// Log an Error-level event.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
		public static void Error(string format, params object[] args) { Log(LogEntryType.Error, format, args); }
        /// <summary>
        /// Log an Error-level event.
        /// </summary>
        /// <param name="message"></param>
		public static void Error(string message) { Log(LogEntryType.Error, message); }
        /// <summary>
        /// Log an Error-level event.
        /// </summary>
        /// <param name="ex"></param>
		public static void Error(Exception ex) { Log(LogEntryType.Error, ex); }

        /// <summary>
        /// Log a Fatal-level event.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
		public static void Fatal(string format, params object[] args) { Log(LogEntryType.Fatal, format, args); }
        /// <summary>
        /// Log a Fatal-level event.
        /// </summary>
        /// <param name="message"></param>
		public static void Fatal(string message) { Log(LogEntryType.Fatal, message); }
        /// <summary>
        /// Log a Fatal-level event.
        /// </summary>
        /// <param name="ex"></param>
		public static void Fatal(Exception ex) { Log(LogEntryType.Fatal, ex); }

        /// <summary>
        /// Log an event.
        /// </summary>
        /// <param name="entryType"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
		public static void Log(LogEntryType entryType, string format, params object[] args)
		{
			LogEntry(String.Format(format, args), entryType);
		}

        /// <summary>
        /// Log an event.
        /// </summary>
        /// <param name="entryType"></param>
        /// <param name="ex"></param>
		public static void Log(LogEntryType entryType, Exception ex)
		{
			LogEntry(GetFullExceptionText(ex), entryType);
		}

        /// <summary>
        /// Log an event.
        /// </summary>
        /// <param name="entryType"></param>
        /// <param name="message"></param>
		public static void Log(LogEntryType entryType, string message)
		{
			LogEntry(message, entryType);
		}

		private static void LogEntry(string message, LogEntryType entryType)
		{
			LogEntry(message, entryType, false);
		}

		/// <summary>
		/// Returns a slash-delimited list of the methods called on the stack for an exception, excluding this logger and "noisy" things from System and WebHost.
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static string GetMethodPath(Exception ex)
		{
			return GetMethodPath(new StackTrace(ex));
		}

		/// <summary>
		/// Returns a slash-delimited list of the methods called on the stack for the calling method, excluding this logger and "noisy" things from System and WebHost.
		/// </summary>
		/// <returns></returns>
		public static string GetMethodPath()
		{
			return GetMethodPath(new StackTrace());
		}

		/// <summary>
		/// Returns a slash-delimited list of the methods called on a stack, excluding this logger and "noisy" things from System and WebHost.
		/// </summary>
		/// <param name="stack"></param>
		/// <returns></returns>
		private static string GetMethodPath(StackTrace stack)
		{
			var frames = stack.GetFrames();
			var stackPath = new List<string>();

			for (int i = frames.Length - 1; i >= 0; i--)
			{
				var frame = frames[i];
				var method = frame.GetMethod();
				var declaringType = method.DeclaringType;

				// if the stack frame corresponds to still being inside the log4net assembly, skip it.
				if ((declaringType == null) || declaringType.Assembly == typeof(log4net.LogManager).Assembly)
				{
					continue;
				}

				//Exclude noise items from the stack.
				if (declaringType.FullName.Contains("System.") || declaringType.FullName.Contains("Logger") || declaringType.FullName.Contains("WebHost"))
				{
					continue;
				}

				stackPath.Add(String.Format("{0}.{1}",
					declaringType.Name,
					method.Name));
			}
			return String.Join("/", stackPath);
		}

		/// <summary>
		/// Wrapper method for log4net logging.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="entryType"></param>
		/// <param name="forceLog"></param>
		private static void LogEntry(string message, LogEntryType entryType, bool forceLog)
		{
			var logger = log4net.LogManager.GetLogger("Logger");
			var trace = GetMethodPath();
			var fullMessage = String.Format("[{2}({3})] {4} {1} ({0})", trace, message, ClientProperties.IpAddress, ClientProperties.Identifier, ClientProperties.ApplicationLocation);

			switch (entryType)
			{
				case LogEntryType.Debug:
					if (!forceLog && !logger.IsDebugEnabled) return;
					logger.Debug(fullMessage);
					break;
				case LogEntryType.Information:
					if (!forceLog && !logger.IsInfoEnabled) return;
					logger.Info(fullMessage);
					break;
				case LogEntryType.Warning:
					if (!forceLog && !logger.IsWarnEnabled) return;
					logger.Warn(fullMessage);
					break;
				case LogEntryType.Error: // always log errors
					logger.Error(fullMessage);
					break;
				case LogEntryType.Fatal: // always log fatal
					logger.Fatal(fullMessage);
					break;
				default:
					if (!forceLog && !logger.IsInfoEnabled) return;
					logger.Info(fullMessage);
					break;
			}
		}


		/// <summary>
		/// Recursively builds a string from the exception and its inner exceptions.
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static string GetFullExceptionText(Exception ex)
		{
			var sb = new StringBuilder();
			GetException(ex, sb);
			return sb.ToString();
		}

		private static void GetException(Exception ex, StringBuilder sb)
		{
			sb.AppendLine(String.Format("Source: '{0}', Message: '{1}'", ex.Source, ex.Message));
			sb.AppendLine(String.Format("Stack Trace: {0}", ex.StackTrace));
			if (ex.InnerException == null) return;
			sb.AppendLine("### INNER EXCEPTION ###");
			GetException(ex.InnerException, sb);
		}
	}

	/// <summary>
	/// Log entry types.
	/// </summary>
	public enum LogEntryType
	{
        /// <summary>
        /// A debug-level event.
        /// </summary>
		Debug,
        /// <summary>
        /// An info-level event.
        /// </summary>
		Information,
        /// <summary>
        /// A warning-level event.
        /// </summary>
		Warning,
        /// <summary>
        /// An error-level event.
        /// </summary>
		Error,
        /// <summary>
        /// A fatal-level event.
        /// </summary>
		Fatal
	}

	/// <summary>
	/// Client-application-agnostic provider of client properties.
	/// </summary>
	public interface IClientPropertyProvider
	{
		/// <summary>
		/// An identifier for the current user (username).
		/// </summary>
		string Identifier { get; }

		/// <summary>
		/// The IP address of the current user.
		/// </summary>
		string IpAddress { get; }

		/// <summary>
		/// The location within the application.  Could be a URL or a window or component identifier.
		/// </summary>
		string ApplicationLocation { get; }
	}

}
