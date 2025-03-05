using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PurchaseOrderList", Namespace = "http://www.piistech.com//list")]	
	public class PurchaseOrderList : BaseCollection<PurchaseOrder>
	{
		#region Constructors
	    public PurchaseOrderList() : base() { }
        public PurchaseOrderList(PurchaseOrder[] list) : base(list) { }
        public PurchaseOrderList(List<PurchaseOrder> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

