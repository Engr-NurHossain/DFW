using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class TicketReply 
	{
		public string CreatedByVal { set; get; }
        public string TypeReply { set; get; }
        public string RepliedDateVal { set; get; }
        public string ProfilePicture { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
    public partial class AssignTicket
    {

        public int CustomerId { get; set; }
        public int TicketId { get; set; }
        public string TicketStatus { get; set; }
        public Guid CustomerGuid { get; set; }
        public string TicketType { set; get; }
        public string CreatedBy { set; get; }
        public string CustomerName { set; get; }
        public DateTime CreatedDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string AppointmentStartTime { set; get; }
        public string AppointmentEndTime { set; get; }
        public bool IsPrimary { get; set; }
        public bool NotificationOnly { get; set; }
        public string AssignedUserName { get; set; }







    }
    public partial class AssignAllTicket
    {
        public List<AssignAllTicket> AssignTicketList { get; set; }

        public int CustomerId { get; set; }
        public int TicketId { get; set; }
        public string TicketStatus { get; set; }
        public Guid CustomerGuid { get; set; }
        public string TicketType { set; get; }
        public string CreatedBy { set; get; }
        public string CustomerName { set; get; }
        public DateTime CreatedDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string AppointmentStartTime { set; get; }
        public string AppointmentEndTime { set; get; }
        public bool IsPrimary { get; set; }
        public bool NotificationOnly { get; set; }
        public string AssignedUserName { get; set; }

        public int CountTicket { get; set; }

        public int TotalCount { get; set; }





    }
}
