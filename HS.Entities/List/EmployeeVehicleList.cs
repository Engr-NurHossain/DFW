using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeVehicleList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeVehicleList : BaseCollection<EmployeeVehicle>
	{
		#region Constructors
	    public EmployeeVehicleList() : base() { }
        public EmployeeVehicleList(EmployeeVehicle[] list) : base(list) { }
        public EmployeeVehicleList(List<EmployeeVehicle> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

