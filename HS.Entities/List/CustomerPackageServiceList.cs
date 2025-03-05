using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerPackageServiceList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerPackageServiceList : BaseCollection<CustomerPackageService>
	{
		#region Constructors
	    public CustomerPackageServiceList() : base() { }
        public CustomerPackageServiceList(CustomerPackageService[] list) : base(list) { }
        public CustomerPackageServiceList(List<CustomerPackageService> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

