using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeCommissionList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeCommissionList : BaseCollection<EmployeeCommission>
	{
		#region Constructors
	    public EmployeeCommissionList() : base() { }
        public EmployeeCommissionList(EmployeeCommission[] list) : base(list) { }
        public EmployeeCommissionList(List<EmployeeCommission> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

