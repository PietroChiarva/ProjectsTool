﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectsTool.Models
{
    public class ClientList : Client
    {
        public ClientList()
        {
            ResultList = new List<Client>();
        }
        public List<Client> ResultList { get; set; }
    }
}