using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeOccurencesList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeOccurencesList : BaseCollection<EmployeeOccurences>
	{
		#region Constructors
	    public EmployeeOccurencesList() : base() { }
        public EmployeeOccurencesList(EmployeeOccurences[] list) : base(list) { }
        public EmployeeOccurencesList(List<EmployeeOccurences> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

