using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectsTool.Models
{
    public class ResourceList : Person
    {
        public ResourceList()
        {
            ResultList = new List<Person>();
        }
        public List<Person> ResultList { get; set; }

    }
}