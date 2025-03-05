using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TicketTimeClockList", Namespace = "http://www.piistech.com//list")]	
	public class TicketTimeClockList : BaseCollection<TicketTimeClock>
	{
		#region Constructors
	    public TicketTimeClockList() : base() { }
        public TicketTimeClockList(TicketTimeClock[] list) : base(list) { }
        public TicketTimeClockList(List<TicketTimeClock> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

