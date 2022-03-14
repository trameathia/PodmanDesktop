namespace PodmanDesktop.Podman
{
    public interface IPodman
    {
        bool Run(string command, out string output);
        bool RunRaw(string command, out string output);
    }
}
