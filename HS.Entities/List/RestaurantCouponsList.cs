﻿using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestaurantCouponsList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestaurantCouponsList : BaseCollection<RestaurantCoupons>
	{
		#region Constructors
	    public RestaurantCouponsList() : base() { }
        public RestaurantCouponsList(RestaurantCoupons[] list) : base(list) { }
        public RestaurantCouponsList(List<RestaurantCoupons> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
