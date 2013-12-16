using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PixelMEDIA.PixelWeb.Components;
using CompanyName.ProjectName.Core;

namespace CompanyName.ProjectName.Components.BaseClasses
{
    public abstract class ProjectNameController : PixelController
    {

        public ApplicationSettings AppSettings { get { return ApplicationSettings.Instance; } }


    }
}
