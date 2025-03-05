using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestaurantLocationList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestaurantLocationList : BaseCollection<RestaurantLocation>
	{
		#region Constructors
	    public RestaurantLocationList() : base() { }
        public RestaurantLocationList(RestaurantLocation[] list) : base(list) { }
        public RestaurantLocationList(List<RestaurantLocation> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
