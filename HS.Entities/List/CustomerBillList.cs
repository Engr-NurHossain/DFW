using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerBillList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerBillList : BaseCollection<CustomerBill>
	{
		#region Constructors
	    public CustomerBillList() : base() { }
        public CustomerBillList(CustomerBill[] list) : base(list) { }
        public CustomerBillList(List<CustomerBill> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

