using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ZonesEquipmentTypeList", Namespace = "http://www.hims-tech.com//list")]	
	public class ZonesEquipmentTypeList : BaseCollection<ZonesEquipmentType>
	{
		#region Constructors
	    public ZonesEquipmentTypeList() : base() { }
        public ZonesEquipmentTypeList(ZonesEquipmentType[] list) : base(list) { }
        public ZonesEquipmentTypeList(List<ZonesEquipmentType> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
