namespace Jordans_Podman_Tool.Settings
{
    public interface IAppSettings
    {
        bool UseSudo { get; set; }
        double WindowHeight { get; set; }
        double WindowWidth { get; set; }
        bool UseDefaultWSLDistro { get; set; }
        string WSLDistro { get; set; }
    }
}
