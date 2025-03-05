using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PackageIncludeList", Namespace = "http://www.piistech.com//list")]	
	public class PackageIncludeList : BaseCollection<PackageInclude>
	{
		#region Constructors
	    public PackageIncludeList() : base() { }
        public PackageIncludeList(PackageInclude[] list) : base(list) { }
        public PackageIncludeList(List<PackageInclude> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

