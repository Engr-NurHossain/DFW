using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PackageCommissionList", Namespace = "http://www.piistech.com//list")]	
	public class PackageCommissionList : BaseCollection<PackageCommission>
	{
		#region Constructors
	    public PackageCommissionList() : base() { }
        public PackageCommissionList(PackageCommission[] list) : base(list) { }
        public PackageCommissionList(List<PackageCommission> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

