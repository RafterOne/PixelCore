using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PixelMEDIA.PixelCore.Helpers;

namespace PixelExe.Providers
{
    /// <summary>
    /// Provides faked web client properties in non-web enviroments (such as Windows/Console/Service applications).
    /// </summary>
	public class ExeClientPropertyProvider : IClientPropertyProvider
	{
		/// <summary>
		/// The current user name.
		/// </summary>
		public string Identifier
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// The IP Address of the current user.
		/// </summary>
		public string IpAddress
		{
			get
			{
				return "0.0.0.0";
			}
		}

		/// <summary>
		/// The URL of the current page.
		/// </summary>
		public string ApplicationLocation
		{
			get
			{
				return string.Empty;
			}
		}
	}
}
