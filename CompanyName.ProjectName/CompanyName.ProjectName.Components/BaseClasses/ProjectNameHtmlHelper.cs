using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PixelMEDIA.PixelWeb.Components;
using CompanyName.ProjectName.Core;
using System.Web.Mvc;
using System.Web.Routing;

namespace CompanyName.ProjectName.Components.BaseClasses
{
    public class ProjectNameHtmlHelper : PixelHelper
    {
        /// <summary>
        /// Settings for this application.
        /// </summary>
        public ApplicationSettings AppSettings { get { return ApplicationSettings.Instance; } }

        /// <summary>
        /// The current controller as a project-specific controller.
        /// </summary>
        public ProjectNameController Controller { get { return (this.ViewContext.Controller as ProjectNameController); } }

        /// <summary>
        /// Project-specific helpers could go here.
        /// </summary>
        public ProjectNameSpecificHelper ProjectName { get { return new ProjectNameSpecificHelper(); } }


        /*
		/// <summary>
		/// The current user of the application.
		/// </summary>
		public CachedUser CurrentUser { get { return this.Controller.CurrentUser; } }
		*/

        public ProjectNameHtmlHelper(ViewContext viewContext, WebViewPage viewPage)
            : this(viewContext, viewPage, RouteTable.Routes)
        {
        }

        public ProjectNameHtmlHelper(ViewContext viewContext, WebViewPage viewPage, RouteCollection routeCollection)
            : base(viewContext, viewPage, routeCollection)
        {
        }
    }
}
