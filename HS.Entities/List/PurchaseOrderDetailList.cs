using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PurchaseOrderDetailList", Namespace = "http://www.piistech.com//list")]	
	public class PurchaseOrderDetailList : BaseCollection<PurchaseOrderDetail>
	{
		#region Constructors
	    public PurchaseOrderDetailList() : base() { }
        public PurchaseOrderDetailList(PurchaseOrderDetail[] list) : base(list) { }
        public PurchaseOrderDetailList(List<PurchaseOrderDetail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

