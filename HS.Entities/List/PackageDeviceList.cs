using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PackageDeviceList", Namespace = "http://www.piistech.com//list")]	
	public class PackageDeviceList : BaseCollection<PackageDevice>
	{
		#region Constructors
	    public PackageDeviceList() : base() { }
        public PackageDeviceList(PackageDevice[] list) : base(list) { }
        public PackageDeviceList(List<PackageDevice> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

