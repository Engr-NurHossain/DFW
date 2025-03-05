using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "InventoryWarehouseList", Namespace = "http://www.piistech.com//list")]	
	public class InventoryWarehouseList : BaseCollection<InventoryWarehouse>
	{
		#region Constructors
	    public InventoryWarehouseList() : base() { }
        public InventoryWarehouseList(InventoryWarehouse[] list) : base(list) { }
        public InventoryWarehouseList(List<InventoryWarehouse> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

