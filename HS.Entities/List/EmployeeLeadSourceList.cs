using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeLeadSourceList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeLeadSourceList : BaseCollection<EmployeeLeadSource>
	{
		#region Constructors
	    public EmployeeLeadSourceList() : base() { }
        public EmployeeLeadSourceList(EmployeeLeadSource[] list) : base(list) { }
        public EmployeeLeadSourceList(List<EmployeeLeadSource> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

