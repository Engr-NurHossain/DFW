using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeTimeClockList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeTimeClockList : BaseCollection<EmployeeTimeClock>
	{
		#region Constructors
	    public EmployeeTimeClockList() : base() { }
        public EmployeeTimeClockList(EmployeeTimeClock[] list) : base(list) { }
        public EmployeeTimeClockList(List<EmployeeTimeClock> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

