using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AdjustmentRuleList", Namespace = "http://www.piistech.com//list")]	
	public class AdjustmentRuleList : BaseCollection<AdjustmentRule>
	{
		#region Constructors
	    public AdjustmentRuleList() : base() { }
        public AdjustmentRuleList(AdjustmentRule[] list) : base(list) { }
        public AdjustmentRuleList(List<AdjustmentRule> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

