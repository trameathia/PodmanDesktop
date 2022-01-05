using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jordans_Podman_Tool
{
    public class Image
    {
        public string Repository { get; set; }
        public string Tag { get; set; }
        public string ImageID { get; set; }
        public string Created { get; set; }
        public string Size { get; set; }

        public Image(string repository, string tag, string imageID, string created, string size)
        {
            this.Repository = repository;
            this.Tag = tag;
            this.ImageID = imageID;
            this.Created = created;
            this.Size = size;
        }
    }
}
