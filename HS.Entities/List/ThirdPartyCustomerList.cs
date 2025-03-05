using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ThirdPartyCustomerList", Namespace = "http://www.hims-tech.com//list")]	
	public class ThirdPartyCustomerList : BaseCollection<ThirdPartyCustomer>
	{
		#region Constructors
	    public ThirdPartyCustomerList() : base() { }
        public ThirdPartyCustomerList(ThirdPartyCustomer[] list) : base(list) { }
        public ThirdPartyCustomerList(List<ThirdPartyCustomer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
