using System.Collections.Generic;

namespace backEnd.Models
{
    public class ResponseErr
    {
        public string msg { get; set; }
        public object data { get; set; }

        public List<DataFile> listdata { get; set; }
    }
}