using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SupplierBillDetailList", Namespace = "http://www.piistech.com//list")]	
	public class SupplierBillDetailList : BaseCollection<SupplierBillDetail>
	{
		#region Constructors
	    public SupplierBillDetailList() : base() { }
        public SupplierBillDetailList(SupplierBillDetail[] list) : base(list) { }
        public SupplierBillDetailList(List<SupplierBillDetail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

