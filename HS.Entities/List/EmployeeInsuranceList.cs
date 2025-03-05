using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeInsuranceList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeInsuranceList : BaseCollection<EmployeeInsurance>
	{
		#region Constructors
	    public EmployeeInsuranceList() : base() { }
        public EmployeeInsuranceList(EmployeeInsurance[] list) : base(list) { }
        public EmployeeInsuranceList(List<EmployeeInsurance> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

