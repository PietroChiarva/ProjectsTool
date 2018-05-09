using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectsTool.Models
{
    public class ProjectModel : ProjectToolsEntities
    {
       public List<SingleProjectModel> Projects { get; set; }
       public List<ProjectResource> ProjectResources { get; set; }
       public List<Person> Resources { get; set; }
       public List<Project> projects { get; set; }
       public List<Client> Clients { get; set; }

    }
}