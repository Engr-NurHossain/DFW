using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class FollowUpCommission 
	{
        public string CustomerName { get; set; }
        public string Technician { get; set; }
        public int TicketIdValue { get; set; }
        public int CustomerIdValue { get; set; }
        public double BalanceDue { set; get; }
    }
}
