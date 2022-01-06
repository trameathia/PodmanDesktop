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
        public double WindowHeight
        {
            get => Convert.ToDouble(ConfigurationManager.AppSettings["WindowHeight"]);
            set {
                Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                oConfig.AppSettings.Settings["WindowHeight"].Value = value.ToString();
                oConfig.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        public double WindowWidth
        {
            get => Convert.ToDouble(ConfigurationManager.AppSettings["WindowWidth"]);
            set
            {
                Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                oConfig.AppSettings.Settings["WindowWidth"].Value = value.ToString();
                oConfig.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
}
