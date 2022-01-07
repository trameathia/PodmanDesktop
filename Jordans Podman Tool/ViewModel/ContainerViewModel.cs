using Jordans_Podman_Tool.Model;
using Jordans_Podman_Tool.Podman;
using Jordans_Podman_Tool.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Container = Jordans_Podman_Tool.Model.Container;

namespace Jordans_Podman_Tool.ViewModel
{
    public class ContainerViewModel : ViewModelBase
    {
        #region Private Properties
        private ObservableCollection<Container> containers;
        private bool showAll;
        private ICommand startContainerCommand;
        private ICommand stopContainerCommand;
        private ICommand restartContainerCommand;
        private ICommand rmContainerCommand;
        private ICommand showAllCommand;
        private IPodman podman;
        private BackgroundWorker backgroundWorker;
        #endregion
        #region Public Properties
        public ObservableCollection<Container> Containers
        {
            get => containers;
            set => containers = value;
        }
        public bool ShowAll
        {
            get => showAll;
            set => showAll = value;
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
        public ICommand ShowAllCommand
        {
            get => showAllCommand;
            set => showAllCommand = value;
        }
        public IPodman Podman
        {
            get => podman;
            set => podman = value;
        }
        #endregion
        #region Public Methods
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ContainerViewModel(IPodman podman)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Podman = podman;
            containers = new ObservableCollection<Container>();
            StartContainerCommand = new RelayCommand(new Action<object>(StartContainer));
            StopContainerCommand = new RelayCommand(new Action<object>(StopContainer));
            RestartContainerCommand = new RelayCommand(new Action<object>(RestartContainer));
            RMContainerCommand = new RelayCommand(new Action<object>(RMContainer));
            ShowAllCommand = new RelayCommand(new Action<object>(UpdateShowAll));

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(bw_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.RunWorkerAsync();
        }
        #endregion
        #region Private Methods
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (!worker.CancellationPending)
            {
                List<Container> results = new();
                string command = String.Format("podman ps{0}", ShowAll ? " -a" : "");
                if (Podman.Run(command, out string output) && output.Contains("PORTS"))
                {
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
                                results.Add(new Container(split[0], split[1], split[2], split[3], split[4], split[5], split[6]));
                            }
                            else if (split.Length == 6)
                            {
                                if (split[4].Contains("->"))
                                {
                                    results.Add(new Container(split[0], split[1], "", split[2], split[3], split[4], split[5]));
                                }
                                else
                                {
                                    results.Add(new Container(split[0], split[1], split[2], split[3], split[4], "", split[5]));
                                }
                            }
                            else if (split.Length == 5)
                            {
                                results.Add(new Container(split[0], split[1], "", split[2], split[3], "", split[4]));
                            }
                        }
                        worker.ReportProgress(1, results);
                    }
                }
                //worker.ReportProgress(0);
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                containers.Clear();
                ((List<Container>)e.UserState).ForEach(containers.Add);
            }
        }

        private void UpdateShowAll(object obj)
        {
            ShowAll = (bool)obj;
        }

        private void StartContainer(object obj)
        {
            _ = Podman.Run(string.Format("podman start {0}", (string)obj), out _);
        }

        private void StopContainer(object obj)
        {
            _ = Podman.Run(string.Format("podman stop {0}", (string)obj), out _);
        }

        private void RestartContainer(object obj)
        {
            _ = Podman.Run(string.Format("podman restart {0}", (string)obj), out _);
        }

        private void RMContainer(object obj)
        {// TODO add confirmation check
            _ = Podman.Run(string.Format("podman rm {0}", (string)obj), out _);
        }
        #endregion
    }
}
