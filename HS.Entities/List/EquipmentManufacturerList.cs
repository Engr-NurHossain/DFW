using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EquipmentManufacturerList", Namespace = "http://www.hims-tech.com//list")]	
	public class EquipmentManufacturerList : BaseCollection<EquipmentManufacturer>
	{
		#region Constructors
	    public EquipmentManufacturerList() : base() { }
        public EquipmentManufacturerList(EquipmentManufacturer[] list) : base(list) { }
        public EquipmentManufacturerList(List<EquipmentManufacturer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
