using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "GeeseRouteList", Namespace = "http://www.hims-tech.com//list")]	
	public class GeeseRouteList : BaseCollection<GeeseRoute>
	{
		#region Constructors
	    public GeeseRouteList() : base() { }
        public GeeseRouteList(GeeseRoute[] list) : base(list) { }
        public GeeseRouteList(List<GeeseRoute> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
