using Jordans_Podman_Tool.Model;
using Jordans_Podman_Tool.Podman;
using Jordans_Podman_Tool.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace Jordans_Podman_Tool.ViewModel
{
    public class PodViewModel : ViewModelBase
    {
        #region Private Properties
        private ObservableCollection<Pod> pods;
        private ICommand startPodCommand;
        private ICommand stopPodCommand;
        private ICommand restartPodCommand;
        private ICommand rmPodCommand;
        private IPodman podman;
        private BackgroundWorker backgroundWorker;
        #endregion
        #region Public Properties
        public ObservableCollection<Pod> Pods
        {
            get => pods;
            set => pods = value;
        }
        public ICommand StartPodCommand
        {
            get => startPodCommand;
            set => startPodCommand = value;
        }
        public ICommand StopPodCommand
        {
            get => stopPodCommand;
            set => stopPodCommand = value;
        }
        public ICommand RestartPodCommand
        {
            get => restartPodCommand;
            set => restartPodCommand = value;
        }
        public ICommand RMPodCommand
        {
            get => rmPodCommand;
            set => rmPodCommand = value;
        }
        public IPodman Podman
        {
            get => podman;
            set => podman = value;
        }
        #endregion
        #region Public Methods
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PodViewModel(IPodman podman)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Podman = podman;
            pods = new ObservableCollection<Pod>();
            StartPodCommand = new RelayCommand(new Action<object>(StartPod));
            StopPodCommand = new RelayCommand(new Action<object>(StopPod));
            RestartPodCommand = new RelayCommand(new Action<object>(RestartPod));
            RMPodCommand = new RelayCommand(new Action<object>(RMPod));

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
                List<Pod> results = new();
                string command = "podman pod ps";
                if (Podman.Run(command, out string output) && output.Contains("CREATED"))
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
                            results.Add(new Pod(split[0], split[1], split[2], split[3], split[4], split[5]));
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
                pods.Clear();
                ((List<Pod>)e.UserState).ForEach(pods.Add);
            }
        }

        private void StartPod(object obj)
        {
            _ = Podman.Run(string.Format("podman pod start {0}", (string)obj), out _);
        }

        private void StopPod(object obj)
        {
            _ = Podman.Run(string.Format("podman pod stop {0}", (string)obj), out _);
        }

        private void RestartPod(object obj)
        {
            _ = Podman.Run(string.Format("podman pod restart {0}", (string)obj), out _);
        }

        private void RMPod(object obj)
        {
            _ = Podman.Run(string.Format("podman pod rm {0}", (string)obj), out _);
        }
        #endregion
    }
}
