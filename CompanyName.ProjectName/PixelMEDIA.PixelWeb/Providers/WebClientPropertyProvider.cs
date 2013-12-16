using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using PixelMEDIA.PixelCore.Helpers;

namespace PixelMEDIA.PixelWeb.Providers
{
	/// <summary>
	/// Provides client details for a web application.
	/// </summary>
	public class WebClientPropertyProvider : IClientPropertyProvider
	{
		/// <summary>
		/// The current user name.
		/// </summary>
		public string Identifier
		{
			get
			{
				try
				{
					return HttpContext.Current.User.Identity.Name;
				}
				catch (Exception)
				{
					return string.Empty;
				}
			}
		}

		/// <summary>
		/// The IP Address of the current user.
		/// </summary>
		public string IpAddress
		{
			get
			{
				try
				{
					return HttpContext.Current.Request.UserHostAddress;
				}
				catch (Exception)
				{
					return string.Empty;
				}
			}
		}

		/// <summary>
		/// The URL of the current page.
		/// </summary>
		public string ApplicationLocation
		{
			get
			{
				try
				{
					return HttpContext.Current.Request.Url.AbsoluteUri;
				}
				catch (Exception)
				{
					return string.Empty;
				}
			}
		}
	}
}