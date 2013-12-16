using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PixelMEDIA.PixelWeb.Components;
using CompanyName.ProjectName.Core;

namespace CompanyName.ProjectName.Components.BaseClasses
{
    /// <summary>
    /// A base class for untyped view pages. Adds the "Pixel" helper. Views must be called from PixelControllers.
    /// </summary>
    public abstract class ProjectNameWebViewPage : PixelWebViewPage
    {
        public ProjectNameHtmlHelper Pixel { get; private set; }
        public ApplicationSettings AppSettings { get { return ApplicationSettings.Instance; } }

        public override void InitHelpers()
        {
            this.Pixel = new ProjectNameHtmlHelper(base.ViewContext, this);
            base.InitHelpers();
        }
    }

    /// <summary>
    /// A base class for typed view pages. Adds the "Pixel" helper. Views must be called from PixelControllers.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class ProjectNameWebViewPage<TModel> : PixelWebViewPage<TModel>
    {
        public ProjectNameHtmlHelper Pixel { get; private set; }
        private ApplicationSettings AppSettings { get { return ApplicationSettings.Instance; } }

        public override void InitHelpers()
        {
            this.Pixel = new ProjectNameHtmlHelper(base.ViewContext, this);
            base.InitHelpers();
        }
    }
}
