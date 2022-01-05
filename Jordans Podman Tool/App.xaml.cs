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
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            var shutdown = false;

            // Process exception
            if (e.Exception is DivideByZeroException)
            {
                // Recoverable - continue processing
                shutdown = false;
            }
            else if (e.Exception is ArgumentNullException)
            {
                // Unrecoverable - end processing
                shutdown = true;
            }

            if (shutdown)
            {
                // Add entry to event log
                EventLog.WriteEntry("JPT", "Unrecoverable Exception: " + e.Exception.Message, EventLogEntryType.Error);

                // Return exit code
                Shutdown(-1);
            }

            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}
