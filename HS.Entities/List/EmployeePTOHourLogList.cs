using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeePTOHourLogList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeePTOHourLogList : BaseCollection<EmployeePTOHourLog>
	{
		#region Constructors
	    public EmployeePTOHourLogList() : base() { }
        public EmployeePTOHourLogList(EmployeePTOHourLog[] list) : base(list) { }
        public EmployeePTOHourLogList(List<EmployeePTOHourLog> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
