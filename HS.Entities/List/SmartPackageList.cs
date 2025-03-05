using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SmartPackageList", Namespace = "http://www.piistech.com//list")]	
	public class SmartPackageList : BaseCollection<SmartPackage>
	{
		#region Constructors
	    public SmartPackageList() : base() { }
        public SmartPackageList(SmartPackage[] list) : base(list) { }
        public SmartPackageList(List<SmartPackage> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

