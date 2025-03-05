using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RescheduleCommissionList", Namespace = "http://www.piistech.com//list")]	
	public class RescheduleCommissionList : BaseCollection<RescheduleCommission>
	{
		#region Constructors
	    public RescheduleCommissionList() : base() { }
        public RescheduleCommissionList(RescheduleCommission[] list) : base(list) { }
        public RescheduleCommissionList(List<RescheduleCommission> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

