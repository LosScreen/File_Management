using System.Collections.Generic;
using System;

namespace backEnd.Models
{
    public class ResponseErr
    {
        public string msg { get; set; }
        public object data { get; set; }
        public string test { get; set; }

        public List<DataFile> listdata { get; set; }
    }
}