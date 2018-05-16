using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ProjectsTool.Models
{
    public class ActiveResourceModel
    {
        public int ProjectResource { get; set; }
        public ActiveProject ActiveProject { get; set; }
        public int IDProject { get; set; }
        public int IDPerson { get; set; }
      


    }
}