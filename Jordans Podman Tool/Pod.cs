using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jordans_Podman_Tool
{
    public class Pod
    {
        public string PodID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Created { get; set; }
        public string InfraID { get; set; }
        public string Containers { get; set; }
        public bool CanStart
        {
            get { return !this.Status.Contains("Running"); }
        }
        public bool CanStop
        {
            get { return this.Status.Contains("Running"); }
        }
        public bool CanRestart { get { return CanStop; } }
        public bool CanRM { get { return CanStart; } }

        public Pod(string podID, string name, string status, string created, string infraID, string containers)
        {
            this.PodID = podID;
            this.Name = name;
            this.Status = status;
            this.Created = created;
            this.InfraID = infraID;
            this.Containers = containers;
        }
    }
}
