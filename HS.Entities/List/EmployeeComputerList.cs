using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeComputerList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeComputerList : BaseCollection<EmployeeComputer>
	{
		#region Constructors
	    public EmployeeComputerList() : base() { }
        public EmployeeComputerList(EmployeeComputer[] list) : base(list) { }
        public EmployeeComputerList(List<EmployeeComputer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

