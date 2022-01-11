using System;
using System.Configuration;

namespace Jordans_Podman_Tool.Settings
{
    public class AppSettings : IAppSettings
    {
        public bool UseSudo
        {
            get => Convert.ToBoolean(ConfigurationManager.AppSettings["UseSudo"]);
            set
            {
                Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                oConfig.AppSettings.Settings["UseSudo"].Value = value.ToString();
                oConfig.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
            }
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
        public bool UseDefaultWSLDistro
        {
            get => Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultWSLDistro"]);
            set
            {
                Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                oConfig.AppSettings.Settings["UseDefaultWSLDistro"].Value = value.ToString();
                oConfig.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        public string WSLDistro
        {
            get => ConfigurationManager.AppSettings["WSLDistro"].ToString();
            set
            {
                Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                oConfig.AppSettings.Settings["WSLDistro"].Value = value.ToString();
                oConfig.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
}
