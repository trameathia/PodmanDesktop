using Jordans_Podman_Tool.ViewModel;
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
        public MainWindow()
        {
            InitializeComponent();
            PodViewModel PodVM = new();
            podView.DataContext = PodVM;
            ContainerViewModel ContainerVM = new();
            containerView.DataContext = ContainerVM;
            ImageViewModel ImageVM = new();
            imageView.DataContext = ImageVM;
        }
    }
}
