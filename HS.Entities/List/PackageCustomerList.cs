using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PackageCustomerList", Namespace = "http://www.piistech.com//list")]	
	public class PackageCustomerList : BaseCollection<PackageCustomer>
	{
		#region Constructors
	    public PackageCustomerList() : base() { }
        public PackageCustomerList(PackageCustomer[] list) : base(list) { }
        public PackageCustomerList(List<PackageCustomer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

