using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Service.Com
{
    public class Result<T> where T : new()
    {
        public T Data { get; set; }

        public int Code { get; set; }

        public string Msg { get; set; }
    }
}