using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "VehicleDetailList", Namespace = "http://www.piistech.com//list")]	
	public class VehicleDetailList : BaseCollection<VehicleDetail>
	{
		#region Constructors
	    public VehicleDetailList() : base() { }
        public VehicleDetailList(VehicleDetail[] list) : base(list) { }
        public VehicleDetailList(List<VehicleDetail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

