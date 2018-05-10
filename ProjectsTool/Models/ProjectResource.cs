using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectsTool.Models
{
    public class ProjectResource : ProjectToolsEntities
    {
      
        public Person Resources { get; set; }
        public Project projects { get; set; }
        public int Percentage { get; set; }
        public int IDProject { get; set; }
        public int? IDPerson { get; set; }
        public List<ProjectResource> ProjectResources { get; set; }

   

    }
}