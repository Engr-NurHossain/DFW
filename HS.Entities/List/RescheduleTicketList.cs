using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RescheduleTicketList", Namespace = "http://www.piistech.com//list")]	
	public class RescheduleTicketList : BaseCollection<RescheduleTicket>
	{
		#region Constructors
	    public RescheduleTicketList() : base() { }
        public RescheduleTicketList(RescheduleTicket[] list) : base(list) { }
        public RescheduleTicketList(List<RescheduleTicket> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

