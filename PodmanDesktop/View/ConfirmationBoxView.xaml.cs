using System.Windows;

namespace PodmanDesktop.View
{
    /// <summary>
    /// Interaction logic for ConfirmationBox.xaml
    /// </summary>
    public partial class ConfirmationBoxView : Window
    {
        public ConfirmationBoxView()
        {
            InitializeComponent();
        }

        private void YesClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
