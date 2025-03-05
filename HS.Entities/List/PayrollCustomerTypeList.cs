using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollCustomerTypeList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollCustomerTypeList : BaseCollection<PayrollCustomerType>
	{
		#region Constructors
	    public PayrollCustomerTypeList() : base() { }
        public PayrollCustomerTypeList(PayrollCustomerType[] list) : base(list) { }
        public PayrollCustomerTypeList(List<PayrollCustomerType> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
