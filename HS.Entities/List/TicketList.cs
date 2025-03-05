using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TicketList", Namespace = "http://www.piistech.com//list")]	
	public class TicketList : BaseCollection<Ticket>
	{
		#region Constructors
	    public TicketList() : base() { }
        public TicketList(Ticket[] list) : base(list) { }
        public TicketList(List<Ticket> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

