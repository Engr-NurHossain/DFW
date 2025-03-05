using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerSecurityZonesList", Namespace = "http://www.hims-tech.com//list")]	
	public class CustomerSecurityZonesList : BaseCollection<CustomerSecurityZones>
	{
		#region Constructors
	    public CustomerSecurityZonesList() : base() { }
        public CustomerSecurityZonesList(CustomerSecurityZones[] list) : base(list) { }
        public CustomerSecurityZonesList(List<CustomerSecurityZones> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
