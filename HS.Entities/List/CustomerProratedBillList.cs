using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerProratedBillList", Namespace = "http://www.hims-tech.com//list")]	
	public class CustomerProratedBillList : BaseCollection<CustomerProratedBill>
	{
		#region Constructors
	    public CustomerProratedBillList() : base() { }
        public CustomerProratedBillList(CustomerProratedBill[] list) : base(list) { }
        public CustomerProratedBillList(List<CustomerProratedBill> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
