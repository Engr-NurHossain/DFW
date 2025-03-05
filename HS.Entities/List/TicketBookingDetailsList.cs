using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TicketBookingDetailsList", Namespace = "http://www.piistech.com//list")]	
	public class TicketBookingDetailsList : BaseCollection<TicketBookingDetails>
	{
		#region Constructors
	    public TicketBookingDetailsList() : base() { }
        public TicketBookingDetailsList(TicketBookingDetails[] list) : base(list) { }
        public TicketBookingDetailsList(List<TicketBookingDetails> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

