using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CompanyName.ProjectName.Components.BaseClasses
{
    public class ProjectNameSpecificHelper
    {
        /// <summary>
        /// This is just an example of a project-specific function. Feel free to remove.
        /// </summary>
        /// <returns></returns>
        public MvcHtmlString CustomProjectSpecificFunction()
        {
            return new MvcHtmlString("Success!");
        }
    }
}
