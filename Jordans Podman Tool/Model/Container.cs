namespace Jordans_Podman_Tool.Model
{
    public class Container : ModelBase
    {
        #region Private Properties
        private string containerID;
        private string image;
        private string command;
        private string created;
        private string status;
        private string ports;
        private string names;
        #endregion
        #region Public Properties
        public string ContainerID
        {
            get => containerID;
            set => SetProperty(ref containerID, value);
        }
        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }
        public string Command
        {
            get => command;
            set => SetProperty(ref command, value);
        }
        public string Created
        {
            get => created;
            set => SetProperty(ref created, value);
        }
        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }
        public string Ports
        {
            get => ports;
            set => SetProperty(ref ports, value);
        }
        public string Names
        {
            get => names;
            set => SetProperty(ref names, value);
        }
        #endregion
        public bool CanStart => !Status.Contains("Up");
        public bool CanStop => Status.Contains("Up");
        public bool CanRestart => CanStop;
        public bool CanRM => CanStart;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Container(string containerID, string image, string command, string created, string status, string ports, string names)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            ContainerID = containerID;
            Image = image;
            Command = command;
            Created = created;
            Status = status;
            Ports = ports;
            Names = names;
        }
    }
}
