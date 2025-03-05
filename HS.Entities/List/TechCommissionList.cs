using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TechCommissionList", Namespace = "http://www.piistech.com//list")]	
	public class TechCommissionList : BaseCollection<TechCommission>
	{
		#region Constructors
	    public TechCommissionList() : base() { }
        public TechCommissionList(TechCommission[] list) : base(list) { }
        public TechCommissionList(List<TechCommission> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

