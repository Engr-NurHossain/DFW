using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PackageSystemCustomerList", Namespace = "http://www.piistech.com//list")]	
	public class PackageSystemCustomerList : BaseCollection<PackageSystemCustomer>
	{
		#region Constructors
	    public PackageSystemCustomerList() : base() { }
        public PackageSystemCustomerList(PackageSystemCustomer[] list) : base(list) { }
        public PackageSystemCustomerList(List<PackageSystemCustomer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

