using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerThirdPartyAgencyList", Namespace = "http://www.hims-tech.com//list")]	
	public class CustomerThirdPartyAgencyList : BaseCollection<CustomerThirdPartyAgency>
	{
		#region Constructors
	    public CustomerThirdPartyAgencyList() : base() { }
        public CustomerThirdPartyAgencyList(CustomerThirdPartyAgency[] list) : base(list) { }
        public CustomerThirdPartyAgencyList(List<CustomerThirdPartyAgency> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
