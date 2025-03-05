using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeRouteList", Namespace = "http://www.hims-tech.com//list")]	
	public class EmployeeRouteList : BaseCollection<EmployeeRoute>
	{
		#region Constructors
	    public EmployeeRouteList() : base() { }
        public EmployeeRouteList(EmployeeRoute[] list) : base(list) { }
        public EmployeeRouteList(List<EmployeeRoute> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
