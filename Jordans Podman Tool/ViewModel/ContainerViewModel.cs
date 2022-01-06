using Jordans_Podman_Tool.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace Jordans_Podman_Tool.ViewModel
{
    public class ContainerViewModel
    {
        #region Private Properties
        private ObservableCollection<Container> containers;
        private ICommand startContainerCommand;
        private ICommand stopContainerCommand;
        private ICommand restartContainerCommand;
        private ICommand rmContainerCommand;
        private DispatcherTimer ContainerTimer;
        #endregion
        #region Public Properties
        public ObservableCollection<Container> Containers
        {
            get => containers;
            set => containers = value;
        }
        public ICommand StartContainerCommand
        {
            get => startContainerCommand;
            set => startContainerCommand = value;
        }
        public ICommand StopContainerCommand
        {
            get => stopContainerCommand;
            set => stopContainerCommand = value;
        }
        public ICommand RestartContainerCommand
        {
            get => restartContainerCommand;
            set => restartContainerCommand = value;
        }
        public ICommand RMContainerCommand
        {
            get => rmContainerCommand;
            set => rmContainerCommand = value;
        }
        #endregion
        #region Public Methods
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ContainerViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            containers = new ObservableCollection<Container>();
            StartContainerCommand = new RelayCommand(new Action<object>(StartContainer));
            StopContainerCommand = new RelayCommand(new Action<object>(StopContainer));
            RestartContainerCommand = new RelayCommand(new Action<object>(RestartContainer));
            RMContainerCommand = new RelayCommand(new Action<object>(RMContainer));

            PopulateContainers();
            ContainerTimer = new DispatcherTimer();
            ContainerTimer.Interval = TimeSpan.FromSeconds(10);
            ContainerTimer.Tick += OnContainerEvent;
            ContainerTimer.Start();
        }
        #endregion
        #region Private Methods
        private void OnContainerEvent(object? send, EventArgs e)
        {
            PopulateContainers();
        }
        private void PopulateContainers()
        {
            string command = "podman ps";
            if (WSLCommand.Run(command, false, out string output))
            {
                Containers.Clear();
                output = output.Substring(output.IndexOf(command) + command.Length + 2);
                output = output.Substring(output.IndexOf("\n") + 1);
                if (output.IndexOf("\n\r\n") > -1)
                {
                    output = output.Substring(0, output.IndexOf("\n\r\n"));
                    string[] lines = output.Split("\n");
                    foreach (string line in lines)
                    {
                        string[] split = System.Text.RegularExpressions.Regex.Split(line, @"\s{2,}");
                        if (split.Length == 7)
                        {
                            Containers.Add(new Container(split[0], split[1], split[2], split[3], split[4], split[5], split[6]));
                        }
                        else if (split.Length == 6)
                        {
                            Containers.Add(new Container(split[0], split[1], split[2], split[3], split[4], "", split[5]));
                        }
                        else if (split.Length == 5)
                        {
                            Containers.Add(new Container(split[0], split[1], "", split[2], split[3], "", split[4]));
                        }
                    }
                }
            }
        }

        private void StartContainer(object obj)
        {
            _ = WSLCommand.Run(string.Format("podman start {0}", (string)obj), false, out _);
            PopulateContainers();
        }

        private void StopContainer(object obj)
        {
            _ = WSLCommand.Run(string.Format("podman stop {0}", (string)obj), false, out _);
            PopulateContainers();
        }

        private void RestartContainer(object obj)
        {
            _ = WSLCommand.Run(string.Format("podman restart {0}", (string)obj), false, out _);
            PopulateContainers();
        }

        private void RMContainer(object obj)
        {
            _ = WSLCommand.Run(string.Format("podman rm {0}", (string)obj), false, out _);
            PopulateContainers();
        }
        #endregion
    }
}
