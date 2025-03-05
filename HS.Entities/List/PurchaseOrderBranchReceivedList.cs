using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PurchaseOrderBranchReceivedList", Namespace = "http://www.piistech.com//list")]	
	public class PurchaseOrderBranchReceivedList : BaseCollection<PurchaseOrderBranchReceived>
	{
		#region Constructors
	    public PurchaseOrderBranchReceivedList() : base() { }
        public PurchaseOrderBranchReceivedList(PurchaseOrderBranchReceived[] list) : base(list) { }
        public PurchaseOrderBranchReceivedList(List<PurchaseOrderBranchReceived> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

