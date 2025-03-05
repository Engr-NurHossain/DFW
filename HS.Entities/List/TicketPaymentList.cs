using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TicketPaymentList", Namespace = "http://www.piistech.com//list")]	
	public class TicketPaymentList : BaseCollection<TicketPayment>
	{
		#region Constructors
	    public TicketPaymentList() : base() { }
        public TicketPaymentList(TicketPayment[] list) : base(list) { }
        public TicketPaymentList(List<TicketPayment> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

