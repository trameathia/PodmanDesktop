using PodmanDesktop.Settings;
using System.Diagnostics;

namespace PodmanDesktop.Podman
{
    public class WSLCommand : IPodman
    {
        private readonly IAppSettings _appSettings;
        public WSLCommand(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }
        public bool Run(string command, out string output)
        {
            output = string.Empty;
            string result;
            string input = $"{(_appSettings.UseDefaultWSLDistro ? "" : _appSettings.WSLDistro)} {(_appSettings.UseSudo ? "sudo " : "")}{command}";
            if (RunRaw(input, out result))
            {
                output = result;
                return true;
            }
            return false;
            
                
        }

        public bool RunRaw(string command, out string output)
        {
            output = string.Empty;
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
                string input = $"wsl {command}";
                proc.StandardInput.WriteLine(input);
                proc.StandardInput.Flush();
                proc.StandardInput.Close();
                proc.WaitForExit(5000);
                bool returnEarly = false;
                if (!proc.HasExited && proc.Threads != null && proc.Threads.Count > 0)
                {
                    foreach (ProcessThread thread in proc.Threads)
                    {
                        if (thread.ThreadState == ThreadState.Wait)
                        {
                            proc.Kill();
                            returnEarly = true;
                        }
                    }
                }
                if (returnEarly)
                    return false;
                output = proc.StandardOutput.ReadToEnd();
            }
            return true;
        }
    }
}
