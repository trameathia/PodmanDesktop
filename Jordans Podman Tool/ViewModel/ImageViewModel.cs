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
    public class ImageViewModel : ViewModelBase
    {
        #region Private Properties
        private ObservableCollection<Image> images;
        private DispatcherTimer ImageTimer;
        private IPodman podman;
        private BackgroundWorker backgroundWorker;
        #endregion
        #region Public Properties
        public ObservableCollection<Image> Images
        {
            get => images;
            set => images = value;
        }
        public IPodman Podman
        {
            get => podman;
            set => podman = value;
        }
        #endregion
        #region Public Methods
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ImageViewModel(IPodman podman)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Podman = podman;
            images = new ObservableCollection<Image>();

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
                List<Image> results = new();
                string command = "podman image list";
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
                            results.Add(new Image(split[0], split[1], split[2], split[3], split[4]));
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
                images.Clear();
                ((List<Image>)e.UserState).ForEach(images.Add);
            }
        }
        #endregion
    }
}
