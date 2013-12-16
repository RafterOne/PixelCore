using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PixelMEDIA.PixelCore.Interfaces
{
	/// <summary>
	/// Provides an interface for an email template renderer.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IEmailTemplateProvider<T>
	{
        /// <summary>
        /// Renders an email message using the provided model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
		string Render(T model);
	}
}
