using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Jordans_Podman_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static DispatcherTimer _podTimer = new DispatcherTimer();
        private static DispatcherTimer _containerTimer = new DispatcherTimer();
        private static DispatcherTimer _imageTimer = new DispatcherTimer();

        private ObservableCollection<Pod> Pods = new ObservableCollection<Pod>();
        private ObservableCollection<Container> Containers = new ObservableCollection<Container>();
        private ObservableCollection<Image> Images = new ObservableCollection<Image>();

        public MainWindow()
        {
            InitializeComponent();
            SetupPodDG();
            SetupContainerDG();
            SetupImageDG();
        }

        private void SetupPodDG()
        {
            PodDG.ItemsSource = Pods;
            PopulatePodDG();
            _podTimer.Tick += OnPodEvent;
            _podTimer.Interval = TimeSpan.FromSeconds(10);
            _podTimer.Start();
        }
        private void OnPodEvent(object? send, EventArgs e)
        {
            PopulatePodDG();
        }
        private void PopulatePodDG()
        {
            Pods.Clear();
            string command = "podman pod ps";
            string? output = RunWSLCommand(command);
            if (output != null)
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
                        Pods.Add(new Pod(split[0], split[1], split[2], split[3], split[4], split[5]));
                    }
                }
            }
        }

        private void SetupContainerDG()
        {
            ContainerDG.ItemsSource = Containers;
            PopulateContainerDG();
            _containerTimer.Tick += OnContainerEvent;
            _containerTimer.Interval = TimeSpan.FromSeconds(10);
            _containerTimer.Start();
        }
        private void OnContainerEvent(object? send, EventArgs e)
        {
            PopulateContainerDG();
        }
        private void PopulateContainerDG()
        {
            Containers.Clear();
            string command = String.Format("podman ps{0}", (ShowAllCB.IsChecked ?? false) ? " -a" : "");
            string? output = RunWSLCommand(command);
            if (output != null)
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

        private void SetupImageDG()
        {
            ImageDG.ItemsSource = Images;
            PopulateImageDG();
            _imageTimer.Tick += OnImageEvent;
            _imageTimer.Interval = TimeSpan.FromSeconds(10);
            _imageTimer.Start();
        }
        private void OnImageEvent(object? send, EventArgs e)
        {
            PopulateImageDG();
        }
        private void PopulateImageDG()
        {
            Images.Clear();
            string command = "podman image list";
            string? output = RunWSLCommand(command);
            if (output != null)
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
                        Images.Add(new Image(split[0], split[1], split[2], split[3], split[4]));
                    }
                }
            }
        }

        private string? RunWSLCommand(string command)
        {
            string output = "";
            // Execute wsl command:
            using (var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true,
                }
            })
            {
                proc.Start();
                string input = string.Format("wsl {0}{1}", (UseSudoCB.IsChecked ?? false) ? "sudo " : "", command);
                proc.StandardInput.WriteLine(input);
                Thread.Sleep(500); // give some time for command to execute
                proc.StandardInput.Flush();
                proc.StandardInput.Close();
                proc.WaitForExit(5000); // wait up to 5 seconds for command to execute
                bool returnEarly = false;
                if (UseSudoCB.IsChecked ?? false)
                {
                    if (!proc.HasExited && proc.Threads != null && proc.Threads.Count > 0)
                    {
                        foreach (ProcessThread thread in proc.Threads)
                        {
                            if (thread.ThreadState == System.Diagnostics.ThreadState.Wait)
                            {
                                proc.Kill();
                                //proc.StandardInput.Close();
                                UseSudoCB.IsChecked = false;
                                returnEarly = true;
                            }
                        }
                    }
                }
                if (returnEarly)
                    return null;
                output = proc.StandardOutput.ReadToEnd();
            }
            //EventLog.WriteEntry("JPT", "Output: " + output, EventLogEntryType.Information);
            return output;
        }

        private void Containers_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                switch (button.Name)
                {
                    case "Start":
                        RunWSLCommand(string.Format("podman start {0}", button.Tag));
                        PopulateContainerDG();
                        break;
                    case "Stop":
                        RunWSLCommand(string.Format("podman stop {0}", button.Tag));
                        PopulateContainerDG();
                        break;
                    case "Restart":
                        RunWSLCommand(string.Format("podman restart {0}", button.Tag));
                        PopulateContainerDG();
                        break;
                    case "RM":
                        RunWSLCommand(string.Format("podman rm {0}", button.Tag));
                        PopulateContainerDG();
                        break;
                }
            }
        }

        private void Pods_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                switch (button.Name)
                {
                    case "Start":
                        RunWSLCommand(string.Format("podman pod start {0}", button.Tag));
                        PopulatePodDG();
                        break;
                    case "Stop":
                        RunWSLCommand(string.Format("podman pod stop {0}", button.Tag));
                        PopulatePodDG();
                        break;
                    case "Restart":
                        RunWSLCommand(string.Format("podman pod restart {0}", button.Tag));
                        PopulatePodDG();
                        break;
                    case "RM":
                        RunWSLCommand(string.Format("podman pod rm {0}", button.Tag));
                        PopulatePodDG();
                        break;
                }
            }
        }
    }
}
