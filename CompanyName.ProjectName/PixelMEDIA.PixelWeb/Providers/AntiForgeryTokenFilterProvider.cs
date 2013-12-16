using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using PixelMEDIA.PixelCore.Helpers;
using PixelMEDIA.PixelWeb.Attributes;

namespace PixelMEDIA.PixelWeb.Providers
{
	/// <summary>
	/// Applies the ValidateAntiForgeryTokenAttribute to all non-exempt post requests.
	/// </summary>
	public class AntiForgeryTokenFilterProvider : IFilterProvider
	{
		/// <summary>
		/// Applies the ValidateAntiForgeryTokenAttribute to all non-exempt post requests.
		/// </summary>
		/// <param name="controllerContext"></param>
		/// <param name="actionDescriptor"></param>
		/// <returns></returns>
		public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
		{
			List<Filter> result = new List<Filter>();
			string incomingVerb = controllerContext.HttpContext.Request.HttpMethod;

			if (String.Equals(incomingVerb, "POST", StringComparison.OrdinalIgnoreCase))
			{
				if (!actionDescriptor.HasAttribute<AntiForgeryExemptAttribute>())
				{
					result.Add(new Filter(new ValidateAntiForgeryTokenAttribute(), FilterScope.Global, null));
				}
			}

			return result;
		}
	}
}