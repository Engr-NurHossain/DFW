using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class RestaurantSiteConfiguration 
	{
		
	}
    public class RestaurantSiteConfigurationListModel
    {
        public List<RestaurantSiteConfiguration> restaurantSiteConfiguration { get; set; }
        public string Searchtext { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string order { get; set; }
        public int TotalCount { get; set; }
    }
}
