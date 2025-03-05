using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TicketCustomerNotificationList", Namespace = "http://www.piistech.com//list")]	
	public class TicketCustomerNotificationList : BaseCollection<TicketCustomerNotification>
	{
		#region Constructors
	    public TicketCustomerNotificationList() : base() { }
        public TicketCustomerNotificationList(TicketCustomerNotification[] list) : base(list) { }
        public TicketCustomerNotificationList(List<TicketCustomerNotification> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

