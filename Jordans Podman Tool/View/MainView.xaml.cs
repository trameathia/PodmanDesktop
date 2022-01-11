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
        private OptionsViewModel _optionsViewModel;
        public MainView(IAppSettings appSettings, PodViewModel podVM, ContainerViewModel containerVM, ImageViewModel imageVM, OptionsViewModel optionsVM)
        {
            InitializeComponent();
            _appSettings = appSettings;
            podView.DataContext = podVM;
            containerView.DataContext = containerVM;
            imageView.DataContext = imageVM;
            _optionsViewModel = optionsVM;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _appSettings.WindowHeight = ((MainView)sender).Height;
            _appSettings.WindowWidth = ((MainView)sender).Width;
        }
        private void OpenOptions(object sender, RoutedEventArgs e)
        {
            OptionsView optionsVew = new();
            optionsVew.DataContext = _optionsViewModel;
            optionsVew.Show();
        }
    }
}
