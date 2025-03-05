using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerBillDetailList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerBillDetailList : BaseCollection<CustomerBillDetail>
	{
		#region Constructors
	    public CustomerBillDetailList() : base() { }
        public CustomerBillDetailList(CustomerBillDetail[] list) : base(list) { }
        public CustomerBillDetailList(List<CustomerBillDetail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

