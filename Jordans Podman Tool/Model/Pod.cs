namespace Jordans_Podman_Tool.Model
{
    public class Pod : ModelBase
    {
        #region Private Properties
        private string podID;
        private string name;
        private string status;
        private string created;
        private string infraID;
        private string containers;
        #endregion
        #region Public Properties
        public string PodID
        {
            get => podID;
            set => SetProperty(ref podID, value);
        }
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }
        public string Created
        {
            get => created;
            set => SetProperty(ref created, value);
        }
        public string InfraID
        {
            get => infraID;
            set => SetProperty(ref infraID, value);
        }
        public string Containers
        {
            get => containers;
            set => SetProperty(ref containers, value);
        }
        #endregion
        public bool CanStart => !Status.Contains("Running");
        public bool CanStop => Status.Contains("Running");
        public bool CanRestart => CanStop;
        public bool CanRM => CanStart;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Pod(string podID, string name, string status, string created, string infraID, string containers)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            PodID = podID;
            Name = name;
            Status = status;
            Created = created;
            InfraID = infraID;
            Containers = containers;
        }
    }
}
