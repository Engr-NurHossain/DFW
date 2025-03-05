using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AdjustmentFundingList", Namespace = "http://www.hims-tech.com//list")]	
	public class AdjustmentFundingList : BaseCollection<AdjustmentFunding>
	{
		#region Constructors
	    public AdjustmentFundingList() : base() { }
        public AdjustmentFundingList(AdjustmentFunding[] list) : base(list) { }
        public AdjustmentFundingList(List<AdjustmentFunding> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
