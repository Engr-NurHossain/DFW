using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities.API
{
    public class CustomerAPI
    {
        public class CustomerAPIModels
        {
            public int id { set; get; }
            public Guid guid { set; get; }
            public string firstName { set; get; }
            public string lastName { set; get; }
            public string businessName { set; get; }
            public string type { set; get; }
            public string primaryPhone { set; get; }
            public string secondaryPhone { set; get; }
            public string emailAddress { set; get; }
            public string street { set; get; }
            public string city { set; get; }
            public string state { set; get; }
            public string zipCode { set; get; }
            public string street2 { set; get; }
            public string city2 { set; get; }
            public string state2 { set; get; }
            public string zipCode2 { set; get; }
        }
    }
}
