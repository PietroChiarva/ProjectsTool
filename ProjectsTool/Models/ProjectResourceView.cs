using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectsTool.Models
{
    public class ProjectResourceView : ProjectToolsEntities
    {
        public List<Project> projects { get; set; }
        public int? IDPerson { get; set; }
    }
}