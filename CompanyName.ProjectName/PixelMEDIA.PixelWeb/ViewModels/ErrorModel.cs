using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using PixelMEDIA.PixelWeb.Providers;
using PixelMEDIA.PixelCore.Helpers;

namespace PixelMEDIA.PixelWeb.ViewModels
{
	/// <summary>
	/// Model for custom error views.
	/// </summary>
	public class ErrorModel
	{
        /// <summary>
        /// Return the exception for the model.
        /// </summary>
		public Exception Ex { get; private set; }

        /// <summary>
        /// Returns the cleaned-up HTML version of the exception.
        /// </summary>
		public IHtmlString ExceptionHtml { get; set; }

        /// <summary>
        /// Return the method path of the error's origin.
        /// </summary>
		public String MethodPath { get; set; }

        /// <summary>
        /// Get the client properties for the error.
        /// </summary>
		public WebClientPropertyProvider ClientProperties { get; private set; }

        /// <summary>
        /// Create a new Error model.
        /// </summary>
        /// <param name="ex"></param>
		public ErrorModel(Exception ex)
		{
			this.Ex = ex;
			var rawExceptionText = ConversionHelper.SafeConvertString(Logger.GetFullExceptionText(ex));
			rawExceptionText = HttpContext.Current.Server.HtmlEncode(rawExceptionText);
			this.ExceptionHtml = new HtmlString(rawExceptionText.Replace("\n", "<br/>\n"));
			this.MethodPath = Logger.GetMethodPath(ex);
			this.ClientProperties = new WebClientPropertyProvider();
		}
	}
}
