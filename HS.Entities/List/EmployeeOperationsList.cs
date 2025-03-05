using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeOperationsList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeOperationsList : BaseCollection<EmployeeOperations>
	{
		#region Constructors
	    public EmployeeOperationsList() : base() { }
        public EmployeeOperationsList(EmployeeOperations[] list) : base(list) { }
        public EmployeeOperationsList(List<EmployeeOperations> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
