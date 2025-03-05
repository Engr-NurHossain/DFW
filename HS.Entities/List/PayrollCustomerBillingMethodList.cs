using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollCustomerBillingMethodList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollCustomerBillingMethodList : BaseCollection<PayrollCustomerBillingMethod>
	{
		#region Constructors
	    public PayrollCustomerBillingMethodList() : base() { }
        public PayrollCustomerBillingMethodList(PayrollCustomerBillingMethod[] list) : base(list) { }
        public PayrollCustomerBillingMethodList(List<PayrollCustomerBillingMethod> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
