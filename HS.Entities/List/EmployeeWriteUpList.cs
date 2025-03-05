using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeWriteUpList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeWriteUpList : BaseCollection<EmployeeWriteUp>
	{
		#region Constructors
	    public EmployeeWriteUpList() : base() { }
        public EmployeeWriteUpList(EmployeeWriteUp[] list) : base(list) { }
        public EmployeeWriteUpList(List<EmployeeWriteUp> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

