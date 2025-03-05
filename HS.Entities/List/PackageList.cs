using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PackageList", Namespace = "http://www.piistech.com//list")]	
	public class PackageList : BaseCollection<Package>
	{
		#region Constructors
	    public PackageList() : base() { }
        public PackageList(Package[] list) : base(list) { }
        public PackageList(List<Package> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

