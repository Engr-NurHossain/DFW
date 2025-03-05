using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "VehicleMileageLogList", Namespace = "http://www.piistech.com//list")]	
	public class VehicleMileageLogList : BaseCollection<VehicleMileageLog>
	{
		#region Constructors
	    public VehicleMileageLogList() : base() { }
        public VehicleMileageLogList(VehicleMileageLog[] list) : base(list) { }
        public VehicleMileageLogList(List<VehicleMileageLog> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

