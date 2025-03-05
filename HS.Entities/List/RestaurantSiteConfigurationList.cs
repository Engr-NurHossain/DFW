using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestaurantSiteConfigurationList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestaurantSiteConfigurationList : BaseCollection<RestaurantSiteConfiguration>
	{
		#region Constructors
	    public RestaurantSiteConfigurationList() : base() { }
        public RestaurantSiteConfigurationList(RestaurantSiteConfiguration[] list) : base(list) { }
        public RestaurantSiteConfigurationList(List<RestaurantSiteConfiguration> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
