﻿namespace Jordans_Podman_Tool.Podman
{
    public interface IPodman
    {
        bool Run(string command, out string output);
        bool RunRaw(string command, out string output);
    }
}
