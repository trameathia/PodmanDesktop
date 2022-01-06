namespace Jordans_Podman_Tool.Model
{
    public class Image : ModelBase
    {
        #region Private Properties
        private string repository;
        private string tag;
        private string imageID;
        private string created;
        private string size;
        #endregion
        #region Public Properties
        public string Repository
        {
            get => repository;
            set => SetProperty(ref repository, value);
        }
        public string Tag
        {
            get => tag;
            set => SetProperty(ref tag, value);
        }
        public string ImageID
        {
            get => imageID;
            set => SetProperty(ref imageID, value);
        }
        public string Created
        {
            get => created;
            set => SetProperty(ref created, value);
        }
        public string Size
        {
            get => size;
            set => SetProperty(ref size, value);
        }
        #endregion

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Image(string repository, string tag, string imageID, string created, string size)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Repository = repository;
            Tag = tag;
            ImageID = imageID;
            Created = created;
            Size = size;
        }
    }
}
