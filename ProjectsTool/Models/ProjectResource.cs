using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectsTool.Models
{
    public class ProjectResource : ProjectToolsEntities
    {
        public Person Resources { get; set; }
        public int Percentage { get; set; }
    }
}