using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectsTool.Models
{
    public class SingleProjectModel : ProjectToolsEntities
    {
        public string ProjectName { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? EndDate { get; set; }
        public bool IsFinish { get; set; }
        public string ClientName { get; set; }
        public string ManagerName { get; set; }
        public int IDProject { get; set; }
        public int Percentage { get; set; }
    }
}