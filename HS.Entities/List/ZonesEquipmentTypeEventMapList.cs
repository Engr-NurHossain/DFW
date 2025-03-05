using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ZonesEquipmentTypeEventMapList", Namespace = "http://www.hims-tech.com//list")]	
	public class ZonesEquipmentTypeEventMapList : BaseCollection<ZonesEquipmentTypeEventMap>
	{
		#region Constructors
	    public ZonesEquipmentTypeEventMapList() : base() { }
        public ZonesEquipmentTypeEventMapList(ZonesEquipmentTypeEventMap[] list) : base(list) { }
        public ZonesEquipmentTypeEventMapList(List<ZonesEquipmentTypeEventMap> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
