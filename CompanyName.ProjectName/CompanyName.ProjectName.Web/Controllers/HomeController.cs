using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompanyName.ProjectName.Components.BaseClasses;

namespace CompanyName.ProjectName.Web.Controllers
{
    public class HomeController : ProjectNameController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
