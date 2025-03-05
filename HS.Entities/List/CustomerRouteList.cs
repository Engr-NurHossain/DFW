using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerRouteList", Namespace = "http://www.hims-tech.com//list")]	
	public class CustomerRouteList : BaseCollection<CustomerRoute>
	{
		#region Constructors
	    public CustomerRouteList() : base() { }
        public CustomerRouteList(CustomerRoute[] list) : base(list) { }
        public CustomerRouteList(List<CustomerRoute> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
