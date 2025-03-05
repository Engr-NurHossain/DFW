using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ZonesEquipmentLocationList", Namespace = "http://www.hims-tech.com//list")]	
	public class ZonesEquipmentLocationList : BaseCollection<ZonesEquipmentLocation>
	{
		#region Constructors
	    public ZonesEquipmentLocationList() : base() { }
        public ZonesEquipmentLocationList(ZonesEquipmentLocation[] list) : base(list) { }
        public ZonesEquipmentLocationList(List<ZonesEquipmentLocation> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
