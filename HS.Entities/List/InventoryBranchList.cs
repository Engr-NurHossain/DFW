using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "InventoryBranchList", Namespace = "http://www.piistech.com//list")]	
	public class InventoryBranchList : BaseCollection<InventoryBranch>
	{
		#region Constructors
	    public InventoryBranchList() : base() { }
        public InventoryBranchList(InventoryBranch[] list) : base(list) { }
        public InventoryBranchList(List<InventoryBranch> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

