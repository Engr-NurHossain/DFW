using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ServiceCallCommissionList", Namespace = "http://www.piistech.com//list")]	
	public class ServiceCallCommissionList : BaseCollection<ServiceCallCommission>
	{
		#region Constructors
	    public ServiceCallCommissionList() : base() { }
        public ServiceCallCommissionList(ServiceCallCommission[] list) : base(list) { }
        public ServiceCallCommissionList(List<ServiceCallCommission> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

