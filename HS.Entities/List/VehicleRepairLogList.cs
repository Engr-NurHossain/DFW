using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "VehicleRepairLogList", Namespace = "http://www.piistech.com//list")]	
	public class VehicleRepairLogList : BaseCollection<VehicleRepairLog>
	{
		#region Constructors
	    public VehicleRepairLogList() : base() { }
        public VehicleRepairLogList(VehicleRepairLog[] list) : base(list) { }
        public VehicleRepairLogList(List<VehicleRepairLog> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

