using Jordans_Podman_Tool.Settings;
using Jordans_Podman_Tool.ViewModel;
using System.Windows;

namespace Jordans_Podman_Tool.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private IAppSettings _appSettings;
        public MainView(IAppSettings appSettings, PodViewModel PodVM, ContainerViewModel ContainerVM, ImageViewModel ImageVM)
        {
            _appSettings = appSettings;
            InitializeComponent();
            podView.DataContext = PodVM;
            containerView.DataContext = ContainerVM;
            imageView.DataContext = ImageVM;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _appSettings.WindowHeight = ((MainView)sender).Height;
            _appSettings.WindowWidth = ((MainView)sender).Width;
        }
    }
}
