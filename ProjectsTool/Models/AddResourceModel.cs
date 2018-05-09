using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectsTool.Models
{
    public class AddResourceModel
    {
        public List<ProjectResource> ProjectResources { get; set; }
        public int IDProject { get; set; }
    }
}