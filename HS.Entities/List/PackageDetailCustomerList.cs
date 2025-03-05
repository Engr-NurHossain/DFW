using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PackageDetailCustomerList", Namespace = "http://www.piistech.com//list")]	
	public class PackageDetailCustomerList : BaseCollection<PackageDetailCustomer>
	{
		#region Constructors
	    public PackageDetailCustomerList() : base() { }
        public PackageDetailCustomerList(PackageDetailCustomer[] list) : base(list) { }
        public PackageDetailCustomerList(List<PackageDetailCustomer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

