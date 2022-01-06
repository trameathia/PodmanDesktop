using Jordans_Podman_Tool.ViewModel;
using System.Windows;

namespace Jordans_Podman_Tool.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(PodViewModel PodVM, ContainerViewModel ContainerVM, ImageViewModel ImageVM)
        {
            InitializeComponent();
            podView.DataContext = PodVM;
            containerView.DataContext = ContainerVM;
            imageView.DataContext = ImageVM;
        }
    }
}
