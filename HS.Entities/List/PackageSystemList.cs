using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PackageSystemList", Namespace = "http://www.piistech.com//list")]	
	public class PackageSystemList : BaseCollection<PackageSystem>
	{
		#region Constructors
	    public PackageSystemList() : base() { }
        public PackageSystemList(PackageSystem[] list) : base(list) { }
        public PackageSystemList(List<PackageSystem> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

