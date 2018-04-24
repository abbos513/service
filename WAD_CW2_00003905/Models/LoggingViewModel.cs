using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WAD_CW2_00003905.Models
{
    public class LoggingViewModel
    {               
        public string date { get; set; }
        public string username { get; set; }
        public string ip { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public string httpRequestType { get; set; }
        public string httpParameters { get; set; }
    }

}