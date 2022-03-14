using PodmanDesktop.Podman;
using PodmanDesktop.Settings;
using PodmanDesktop.View;
using PodmanDesktop.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace PodmanDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IAppSettings, AppSettings>();
            services.AddSingleton<IPodman, WSLCommand>();
            services.AddSingleton<OptionsViewModel>();
            services.AddSingleton<PodViewModel>();
            services.AddSingleton<ContainerViewModel>();
            services.AddSingleton<ImageViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainView>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = serviceProvider.GetService<MainView>();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            //window.DataContext = serviceProvider.GetService<MainViewModel>();
            window.Height = serviceProvider.GetService<IAppSettings>().WindowHeight;
            window.Width = serviceProvider.GetService<IAppSettings>().WindowWidth;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            window.Show();
        }
    }
}
