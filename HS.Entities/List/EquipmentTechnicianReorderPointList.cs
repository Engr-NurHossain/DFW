using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EquipmentTechnicianReorderPointList", Namespace = "http://www.piistech.com//list")]	
	public class EquipmentTechnicianReorderPointList : BaseCollection<EquipmentTechnicianReorderPoint>
	{
		#region Constructors
	    public EquipmentTechnicianReorderPointList() : base() { }
        public EquipmentTechnicianReorderPointList(EquipmentTechnicianReorderPoint[] list) : base(list) { }
        public EquipmentTechnicianReorderPointList(List<EquipmentTechnicianReorderPoint> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

