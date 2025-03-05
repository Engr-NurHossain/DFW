using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PurchaseOrderWarehouseList", Namespace = "http://www.piistech.com//list")]	
	public class PurchaseOrderWarehouseList : BaseCollection<PurchaseOrderWarehouse>
	{
		#region Constructors
	    public PurchaseOrderWarehouseList() : base() { }
        public PurchaseOrderWarehouseList(PurchaseOrderWarehouse[] list) : base(list) { }
        public PurchaseOrderWarehouseList(List<PurchaseOrderWarehouse> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

