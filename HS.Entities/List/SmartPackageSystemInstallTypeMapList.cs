using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SmartPackageSystemInstallTypeMapList", Namespace = "http://www.piistech.com//list")]	
	public class SmartPackageSystemInstallTypeMapList : BaseCollection<SmartPackageSystemInstallTypeMap>
	{
		#region Constructors
	    public SmartPackageSystemInstallTypeMapList() : base() { }
        public SmartPackageSystemInstallTypeMapList(SmartPackageSystemInstallTypeMap[] list) : base(list) { }
        public SmartPackageSystemInstallTypeMapList(List<SmartPackageSystemInstallTypeMap> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

