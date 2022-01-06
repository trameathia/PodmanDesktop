using Jordans_Podman_Tool.View;
using Jordans_Podman_Tool.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace Jordans_Podman_Tool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainViewModel mainViewModel = new();
            PodViewModel podViewModel = new(mainViewModel);
            ContainerViewModel containerViewModel = new(mainViewModel);
            ImageViewModel imageViewModel = new(mainViewModel);
            MainView window = new(podViewModel, containerViewModel, imageViewModel);
            window.DataContext = mainViewModel;
            window.Show();
        }
    }
}
