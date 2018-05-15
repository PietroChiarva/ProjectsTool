using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectsTool.Models
{
    public class ProjectResourceView : ProjectToolsEntities
    {
        public Person Resources { get; set; }
        public Project progetti { get; set; }
        public List<Project> projects { get; set; }
        public int? IDPerson { get; set; }
        //public List<ProjectResource> ProjectResources { get; set; }
        public int IDProject { get; set; }
        //public string ProjectName { get; set; }
        //[DisplayFormat(DataFormatString = "{0:d}")]
        //public DateTime? StartDate { get; set; }
        //[DisplayFormat(DataFormatString = "{0:d}")]
        //public DateTime? EndDate { get; set; }
    }
}