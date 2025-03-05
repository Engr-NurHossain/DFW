using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PurchaseOrderTechList", Namespace = "http://www.piistech.com//list")]	
	public class PurchaseOrderTechList : BaseCollection<PurchaseOrderTech>
	{
		#region Constructors
	    public PurchaseOrderTechList() : base() { }
        public PurchaseOrderTechList(PurchaseOrderTech[] list) : base(list) { }
        public PurchaseOrderTechList(List<PurchaseOrderTech> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

