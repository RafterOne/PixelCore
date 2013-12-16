using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using PixelMEDIA.PixelCore.Interfaces;
using PixelMEDIA.PixelWeb.Components;

namespace PixelMEDIA.PixelWeb.Providers
{
	/// <summary>
	/// Uses the asp.net MVC ViewEngine to render an email message to a string.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EmailTemplateProvider<T> : IEmailTemplateProvider<T>
	{
		private String ViewName { get; set; }
		private PixelController Controller { get; set; }

        /// <summary>
        /// Creates a new Email Template Provider.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
		public EmailTemplateProvider(PixelController controller, string viewName)
		{
			this.Controller = controller;
			this.ViewName = viewName;
		}

		/// <summary>
		/// Renders the email message using the given model.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public string Render(T model)
		{
			return this.Controller.RenderViewToString(this.ViewName, model);
		}


        /// <summary>
        /// URL encode a string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
		public string UrlEncode(string s)
		{
			return this.Controller.Server.UrlEncode(s);
		}
	}
}