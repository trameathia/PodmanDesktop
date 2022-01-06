using System;
using System.Configuration;

namespace Jordans_Podman_Tool.Settings
{
    class AppSettings : IAppSettings
    {
        public bool UseSudo
        {
            get => Convert.ToBoolean(ConfigurationManager.AppSettings["UseSudo"]);
        }
    }
}
