using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.CSM.Models
{
    public class GetLeadsRequest
    {
        public int siteId { set; get; }
        public DateTime startDate { set; get; }
        public DateTime endDate { set; get; }
        public int qty { set; get; }
        public int page { set; get; }
        public string token { set; get; }
    }
}
