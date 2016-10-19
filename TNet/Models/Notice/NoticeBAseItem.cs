using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Notice
{
    public class NoticeBaseItem
    {
      
        public string idnotice { get; set; }

       
        public string title { get; set; }

        public DateTime? publish_time { get; set; }
    }
}