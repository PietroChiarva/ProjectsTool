using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectsTool.Models
{
    public class ProjectResourceView : ProjectToolsEntities
    {
        public Person Resources { get; set; }
        public Project progetti { get; set; }
        public List<Project> projects { get; set; }
        public int? IDPerson { get; set; }
        public List<ProjectResource> ProjectResources { get; set; }
        public int IDProject { get; set; }
    }
}