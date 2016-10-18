using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Manage
{
    public class MUser
    {
        public int code { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public bool? recvOrder { get; set; }

        public bool? recvSetup { get; set; }

        public bool? sendSetup { get; set; }
    }
}