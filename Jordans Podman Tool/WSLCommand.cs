using System.Diagnostics;
using System.Threading;

namespace Jordans_Podman_Tool
{
    public class WSLCommand
    {
        public static bool Run(string command, bool useSudo, out string output)
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
                string input = string.Format("wsl {0}{1}", useSudo ? "sudo " : "", command);
                proc.StandardInput.WriteLine(input);
                Thread.Sleep(500); // give some time for command to execute
                proc.StandardInput.Flush();
                proc.StandardInput.Close();
                proc.WaitForExit(5000); // wait up to 5 seconds for command to execute
                bool returnEarly = false;
                if (useSudo)
                {
                    if (!proc.HasExited && proc.Threads != null && proc.Threads.Count > 0)
                    {
                        foreach (ProcessThread thread in proc.Threads)
                        {
                            if (thread.ThreadState == System.Diagnostics.ThreadState.Wait)
                            {
                                proc.Kill();
                                //proc.StandardInput.Close();
                                useSudo = false;
                                returnEarly = true;
                            }
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
