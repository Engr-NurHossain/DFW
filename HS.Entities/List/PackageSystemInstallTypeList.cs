using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PackageSystemInstallTypeList", Namespace = "http://www.piistech.com//list")]	
	public class PackageSystemInstallTypeList : BaseCollection<PackageSystemInstallType>
	{
		#region Constructors
	    public PackageSystemInstallTypeList() : base() { }
        public PackageSystemInstallTypeList(PackageSystemInstallType[] list) : base(list) { }
        public PackageSystemInstallTypeList(List<PackageSystemInstallType> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

