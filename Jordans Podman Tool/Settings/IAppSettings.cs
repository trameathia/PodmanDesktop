namespace Jordans_Podman_Tool.Settings
{
    public interface IAppSettings
    {
        bool UseSudo { get; }
        double WindowHeight { get; set; }
        double WindowWidth { get; set; }
    }
}
