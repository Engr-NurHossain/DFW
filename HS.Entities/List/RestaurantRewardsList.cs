using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestaurantRewardsList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestaurantRewardsList : BaseCollection<RestaurantRewards>
	{
		#region Constructors
	    public RestaurantRewardsList() : base() { }
        public RestaurantRewardsList(RestaurantRewards[] list) : base(list) { }
        public RestaurantRewardsList(List<RestaurantRewards> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
