using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PixelMEDIA.PixelWeb.Attributes
{
	/// <summary>
	/// Mark an action so that posts won't check for an anti-forgery token.
	/// </summary>
	public class AntiForgeryExemptAttribute : Attribute
	{
	}
}
