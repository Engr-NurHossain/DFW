using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PaymentInfoCustomerList", Namespace = "http://www.piistech.com//list")]	
	public class PaymentInfoCustomerList : BaseCollection<PaymentInfoCustomer>
	{
		#region Constructors
	    public PaymentInfoCustomerList() : base() { }
        public PaymentInfoCustomerList(PaymentInfoCustomer[] list) : base(list) { }
        public PaymentInfoCustomerList(List<PaymentInfoCustomer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

