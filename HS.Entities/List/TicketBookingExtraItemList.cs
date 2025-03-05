using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TicketBookingExtraItemList", Namespace = "http://www.piistech.com//list")]	
	public class TicketBookingExtraItemList : BaseCollection<TicketBookingExtraItem>
	{
		#region Constructors
	    public TicketBookingExtraItemList() : base() { }
        public TicketBookingExtraItemList(TicketBookingExtraItem[] list) : base(list) { }
        public TicketBookingExtraItemList(List<TicketBookingExtraItem> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

