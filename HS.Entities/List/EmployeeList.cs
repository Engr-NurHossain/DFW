using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeList : BaseCollection<Employee>
	{
		#region Constructors
	    public EmployeeList() : base() { }
        public EmployeeList(Employee[] list) : base(list) { }
        public EmployeeList(List<Employee> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

