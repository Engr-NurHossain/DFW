using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SmartPackageEquipmentServiceList", Namespace = "http://www.piistech.com//list")]	
	public class SmartPackageEquipmentServiceList : BaseCollection<SmartPackageEquipmentService>
	{
		#region Constructors
	    public SmartPackageEquipmentServiceList() : base() { }
        public SmartPackageEquipmentServiceList(SmartPackageEquipmentService[] list) : base(list) { }
        public SmartPackageEquipmentServiceList(List<SmartPackageEquipmentService> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

