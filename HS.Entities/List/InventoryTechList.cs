using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "InventoryTechList", Namespace = "http://www.piistech.com//list")]	
	public class InventoryTechList : BaseCollection<InventoryTech>
	{
		#region Constructors
	    public InventoryTechList() : base() { }
        public InventoryTechList(InventoryTech[] list) : base(list) { }
        public InventoryTechList(List<InventoryTech> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

