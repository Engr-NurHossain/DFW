using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PaymentProfileCustomerList", Namespace = "http://www.piistech.com//list")]	
	public class PaymentProfileCustomerList : BaseCollection<PaymentProfileCustomer>
	{
		#region Constructors
	    public PaymentProfileCustomerList() : base() { }
        public PaymentProfileCustomerList(PaymentProfileCustomer[] list) : base(list) { }
        public PaymentProfileCustomerList(List<PaymentProfileCustomer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

