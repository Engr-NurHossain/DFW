using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SalesCommissionList", Namespace = "http://www.piistech.com//list")]	
	public class SalesCommissionList : BaseCollection<SalesCommission>
	{
		#region Constructors
	    public SalesCommissionList() : base() { }
        public SalesCommissionList(SalesCommission[] list) : base(list) { }
        public SalesCommissionList(List<SalesCommission> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

