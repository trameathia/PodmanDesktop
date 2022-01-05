using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jordans_Podman_Tool
{
    public class Container
    {
        public string ContainerID { get; set; }
        public string Image { get; set; }
        public string Command { get; set; }
        public string Created { get; set; }
        public string Status { get; set; }
        public string Ports { get; set; }
        public string Names { get; set; }
        public bool CanStart
        {
            get { return !this.Status.Contains("Up"); }
        }
        public bool CanStop
        {
            get { return this.Status.Contains("Up"); }
        }
        public bool CanRestart { get { return CanStop; } }
        public bool CanRM { get { return CanStart; } }

        public Container(string containerID, string image, string command, string created, string status, string ports, string names)
        {
            this.ContainerID = containerID;
            this.Image = image;
            this.Command = command;
            this.Created = created;
            this.Status = status;
            this.Ports = ports;
            this.Names = names;
        }
    }
}
