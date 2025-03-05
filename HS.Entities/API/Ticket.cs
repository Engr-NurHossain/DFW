using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities.API
{
    public class TicketAPI
    {
        public class TicketAPIModels
        {
            public int id { set; get; }
            public Guid customerguid { set; get; }
            public int customerId { set; get; }
            public string appoinmentType { set; get; }
            public string customerName { set; get; }
            public string customerBusinessName { set; get; }
            public DateTime appoinmentDate { set; get; }
            public string startTime { set; get; }
            public string endTime { set; get; }
            public string message { set; get; }
            public string status { set; get; }
            public Guid createdByGuid { set; get; }
            public string createdByName { set; get; }
            public DateTime createdDate { set; get; }
            public Guid assignedToGuid { set; get; }
            public string assignedToName { set; get; }
            
        }
    }
}
