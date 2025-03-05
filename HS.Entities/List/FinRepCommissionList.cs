using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "FinRepCommissionList", Namespace = "http://www.hims-tech.com//list")]	
	public class FinRepCommissionList : BaseCollection<FinRepCommission>
	{
		#region Constructors
	    public FinRepCommissionList() : base() { }
        public FinRepCommissionList(FinRepCommission[] list) : base(list) { }
        public FinRepCommissionList(List<FinRepCommission> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
