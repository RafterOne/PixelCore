using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PixelMEDIA.PixelCore.Helpers;

namespace CompanyName.ProjectName.Core
{
    public class ApplicationSettings
    {
        private static ApplicationSettings _settings;

        public string SomeWebConfigProperty { get; private set; }
        public int SomeOtherWebConfigProperty { get; private set; }

        private ApplicationSettings()
        {
            this.SomeWebConfigProperty = SettingsHelper.Get("SomeWebConfigProperty");
            this.SomeOtherWebConfigProperty = SettingsHelper.GetInt("SomeOtherWebConfigProperty");
        }

        static ApplicationSettings()
        {
            _settings = new ApplicationSettings();
        }

        public static ApplicationSettings Instance { get { return _settings; } }

    }
}
