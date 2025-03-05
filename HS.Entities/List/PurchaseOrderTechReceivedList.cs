using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PurchaseOrderTechReceivedList", Namespace = "http://www.piistech.com//list")]	
	public class PurchaseOrderTechReceivedList : BaseCollection<PurchaseOrderTechReceived>
	{
		#region Constructors
	    public PurchaseOrderTechReceivedList() : base() { }
        public PurchaseOrderTechReceivedList(PurchaseOrderTechReceived[] list) : base(list) { }
        public PurchaseOrderTechReceivedList(List<PurchaseOrderTechReceived> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

