using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities.API
{
    public class OpportunityAPI
    {
        public class OpportunityAPIModels
        {
            public int id { set; get; }
            public string title { set; get; }
            public string description { set; get; }
            public string status { set; get; }
            public int accountId { set; get; }
            public Guid accountGuid { set; get; }
        }
    }
}
