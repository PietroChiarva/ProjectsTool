using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectsTool.Models
{
    public class ActiveProjectModel : ProjectToolsEntities
    {
        public int ProjectResource { get; set; }
        public ActiveProject ActiveProject { get; set; }
        public int IDProject { get; set; }
        public int IDPerson { get; set; }
        public int Percentage { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? StartActiveDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? EndActiveDate { get; set; }
    }
}