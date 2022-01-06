using Jordans_Podman_Tool.Model;
using System;
using System.Collections.ObjectModel;
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
        private DispatcherTimer PodTimer;
        private MainViewModel parentVM;
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
        public MainViewModel ParentVM
        {
            get => parentVM;
            set => parentVM = value;
        }
        #endregion
        #region Public Methods
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PodViewModel(MainViewModel parentVM)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            ParentVM = parentVM;
            pods = new ObservableCollection<Pod>();
            StartPodCommand = new RelayCommand(new Action<object>(StartPod));
            StopPodCommand = new RelayCommand(new Action<object>(StopPod));
            RestartPodCommand = new RelayCommand(new Action<object>(RestartPod));
            RMPodCommand = new RelayCommand(new Action<object>(RMPod));

            PopulatePods();
            PodTimer = new DispatcherTimer();
            PodTimer.Interval = TimeSpan.FromSeconds(10);
            PodTimer.Tick += OnPodEvent;
            PodTimer.Start();
        }
        #endregion
        #region Private Methods
        private void OnPodEvent(object? send, EventArgs e)
        {
            PopulatePods();
        }
        private void PopulatePods()
        {
            string command = "podman pod ps";
            if (WSLCommand.Run(command, ParentVM.UseSudo, out string output))
            {
                Pods.Clear();
                output = output.Substring(output.IndexOf(command) + command.Length + 2);
                output = output.Substring(output.IndexOf("\n") + 1);
                if (output.IndexOf("\n\r\n") > -1)
                {
                    output = output.Substring(0, output.IndexOf("\n\r\n"));
                    string[] lines = output.Split("\n");
                    foreach (string line in lines)
                    {
                        string[] split = System.Text.RegularExpressions.Regex.Split(line, @"\s{2,}");
                        Pods.Add(new Pod(split[0], split[1], split[2], split[3], split[4], split[5]));
                    }
                }
            }
        }

        private void StartPod(object obj)
        {
            _ = WSLCommand.Run(string.Format("podman pod start {0}", (string)obj), ParentVM.UseSudo, out _);
            PopulatePods();
        }

        private void StopPod(object obj)
        {
            _ = WSLCommand.Run(string.Format("podman pod stop {0}", (string)obj), ParentVM.UseSudo, out _);
            PopulatePods();
        }

        private void RestartPod(object obj)
        {
            _ = WSLCommand.Run(string.Format("podman pod restart {0}", (string)obj), ParentVM.UseSudo, out _);
            PopulatePods();
        }

        private void RMPod(object obj)
        {
            _ = WSLCommand.Run(string.Format("podman pod rm {0}", (string)obj), ParentVM.UseSudo, out _);
            PopulatePods();
        }
        #endregion
    }
}
