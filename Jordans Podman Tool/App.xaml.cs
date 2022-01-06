using Jordans_Podman_Tool.Podman;
using Jordans_Podman_Tool.Settings;
using Jordans_Podman_Tool.View;
using Jordans_Podman_Tool.ViewModel;
using System.Windows;

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
            AppSettings settings = new();
            WSLCommand wslCommand = new(settings);
            MainViewModel mainViewModel = new();
            PodViewModel podViewModel = new(settings, wslCommand);
            ContainerViewModel containerViewModel = new(settings, wslCommand);
            ImageViewModel imageViewModel = new(settings, wslCommand);
            MainView window = new(settings, podViewModel, containerViewModel, imageViewModel);
            window.DataContext = mainViewModel;
            window.Height = settings.WindowHeight;
            window.Width = settings.WindowWidth;
            window.Show();
        }
    }
}
