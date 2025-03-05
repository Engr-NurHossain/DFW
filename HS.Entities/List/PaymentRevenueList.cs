using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PaymentRevenueList", Namespace = "http://www.piistech.com//list")]	
	public class PaymentRevenueList : BaseCollection<PaymentRevenue>
	{
		#region Constructors
	    public PaymentRevenueList() : base() { }
        public PaymentRevenueList(PaymentRevenue[] list) : base(list) { }
        public PaymentRevenueList(List<PaymentRevenue> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

