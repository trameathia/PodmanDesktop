using Jordans_Podman_Tool.Podman;
using Jordans_Podman_Tool.Settings;
using Jordans_Podman_Tool.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jordans_Podman_Tool.ViewModel
{
    public class OptionsViewModel
    {
        #region Private Properties
        private ObservableCollection<string> wslDistros;
        private IAppSettings appSettings;
        private IPodman podman;
        #endregion
        #region Public Properties
        public ObservableCollection<string> WSLDistros
        {
            get => wslDistros;
            set => wslDistros = value;
        }
        public IAppSettings AppSettings
        {
            get => appSettings;
            set => appSettings = value;
        }
        public IPodman Podman
        {
            get => podman;
            set => podman = value;
        }
        #endregion
        #region Public Methods
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public OptionsViewModel(IAppSettings appSettings, IPodman podman)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            AppSettings = appSettings;
            Podman = podman;
            WSLDistros = new();

            PopulateDistroList();
        }
        #endregion
        #region Private Methods
        private void PopulateDistroList()
        {
            string command = "-l -q";
            if (Podman.RunRaw(command, out string output))
            {
                output = output.Replace("\0", "");
                output = output.Substring(output.IndexOf(command) + command.Length + 2);
                if (output.IndexOf("\n\r\n") > -1)
                {
                    output = output.Substring(0, output.IndexOf("\n\r\n"));
                    output = output.Replace("\r\n", "\r");
                    string[] lines = output.Trim().Split("\r");
                    foreach (string line in lines)
                    {
                        string[] split = line.Split(' ');
                        WSLDistros.Add(split[0]);
                    }
                }
            }
        }
        #endregion
    }
}
