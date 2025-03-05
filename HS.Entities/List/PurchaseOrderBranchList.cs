using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PurchaseOrderBranchList", Namespace = "http://www.piistech.com//list")]	
	public class PurchaseOrderBranchList : BaseCollection<PurchaseOrderBranch>
	{
		#region Constructors
	    public PurchaseOrderBranchList() : base() { }
        public PurchaseOrderBranchList(PurchaseOrderBranch[] list) : base(list) { }
        public PurchaseOrderBranchList(List<PurchaseOrderBranch> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

