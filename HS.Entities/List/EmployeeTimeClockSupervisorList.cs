using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeTimeClockSupervisorList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeTimeClockSupervisorList : BaseCollection<EmployeeTimeClockSupervisor>
	{
		#region Constructors
	    public EmployeeTimeClockSupervisorList() : base() { }
        public EmployeeTimeClockSupervisorList(EmployeeTimeClockSupervisor[] list) : base(list) { }
        public EmployeeTimeClockSupervisorList(List<EmployeeTimeClockSupervisor> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

