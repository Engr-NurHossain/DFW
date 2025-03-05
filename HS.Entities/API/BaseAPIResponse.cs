using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities.API
{
    public class BaseAPIResponse
    {
        public bool success { set; get; }
        public string message { set; get; }
        public object data { set; get; }
    }
}
