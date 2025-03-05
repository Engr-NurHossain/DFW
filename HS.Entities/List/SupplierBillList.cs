using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SupplierBillList", Namespace = "http://www.piistech.com//list")]	
	public class SupplierBillList : BaseCollection<SupplierBill>
	{
		#region Constructors
	    public SupplierBillList() : base() { }
        public SupplierBillList(SupplierBill[] list) : base(list) { }
        public SupplierBillList(List<SupplierBill> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

