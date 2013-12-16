using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;

namespace PixelMEDIA.PixelWeb.Components
{

	/// <summary>
	/// A base class for untyped view pages. The inheriting, project specific class adds the "Pixel" helper. Views must be called from PixelControllers.
	/// </summary>
	public abstract class PixelWebViewPage : System.Web.Mvc.WebViewPage
	{
        /// <summary>
        /// Placeholder override for InitHelpers()
        /// </summary>
		public override void InitHelpers()
		{
			base.InitHelpers();
		}
	}

	/// <summary>
	/// A base class for typed view pages. The inheriting, project specific class adds the "Pixel" helper. Views must be called from PixelControllers.
	/// </summary>
	/// <typeparam name="TModel"></typeparam>
	public abstract class PixelWebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
	{
        /// <summary>
        /// Placeholder override for InitHelpers()
        /// </summary>
		public override void InitHelpers()
		{
			base.InitHelpers();
		}
	}

}
