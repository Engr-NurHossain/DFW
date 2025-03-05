using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class TicketUser 
	{
		public string FullName { set; get; }
        public string StartDate { get; set; }
        public string Enddate { get; set; }
        public string CustomerName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool IsAllDay { get; set; }
        public string EMPNAME { get; set; }
        public int TicketIntId { get; set; }
        public string BookingId { get; set; }
        public int MyTicketCount { get; set; }
    }
    public partial class AdditionalMember
    {
        public string FullName { set; get; }
        public Guid UserId { get; set; }
        public bool IsReschedulePay { get; set; }

    }

}
