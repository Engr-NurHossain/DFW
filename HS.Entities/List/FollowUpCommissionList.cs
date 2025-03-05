using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "FollowUpCommissionList", Namespace = "http://www.piistech.com//list")]	
	public class FollowUpCommissionList : BaseCollection<FollowUpCommission>
	{
		#region Constructors
	    public FollowUpCommissionList() : base() { }
        public FollowUpCommissionList(FollowUpCommission[] list) : base(list) { }
        public FollowUpCommissionList(List<FollowUpCommission> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

