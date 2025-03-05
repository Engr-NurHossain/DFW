using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestaurantContentList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestaurantContentList : BaseCollection<RestaurantContent>
	{
		#region Constructors
	    public RestaurantContentList() : base() { }
        public RestaurantContentList(RestaurantContent[] list) : base(list) { }
        public RestaurantContentList(List<RestaurantContent> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
