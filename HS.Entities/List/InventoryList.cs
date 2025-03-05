using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "InventoryList", Namespace = "http://www.piistech.com//list")]	
	public class InventoryList : BaseCollection<Inventory>
	{
		#region Constructors
	    public InventoryList() : base() { }
        public InventoryList(Inventory[] list) : base(list) { }
        public InventoryList(List<Inventory> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

