using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectsTool.Models
{
    public class ClientList : ProjectToolsEntities
    {
        public int IDClient { get; set; }
        public string Name { get; set; }
        public string Society { get; set; }
        public string NumberPhone { get; set; }
        public string Email { get; set; }
        public string PartitaIva { get; set; }

    }
}