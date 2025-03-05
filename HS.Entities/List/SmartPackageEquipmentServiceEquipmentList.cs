using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SmartPackageEquipmentServiceEquipmentList", Namespace = "http://www.piistech.com//list")]	
	public class SmartPackageEquipmentServiceEquipmentList : BaseCollection<SmartPackageEquipmentServiceEquipment>
	{
		#region Constructors
	    public SmartPackageEquipmentServiceEquipmentList() : base() { }
        public SmartPackageEquipmentServiceEquipmentList(SmartPackageEquipmentServiceEquipment[] list) : base(list) { }
        public SmartPackageEquipmentServiceEquipmentList(List<SmartPackageEquipmentServiceEquipment> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

