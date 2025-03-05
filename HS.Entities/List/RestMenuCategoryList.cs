using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestMenuCategoryList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestMenuCategoryList : BaseCollection<RestMenuCategory>
	{
		#region Constructors
	    public RestMenuCategoryList() : base() { }
        public RestMenuCategoryList(RestMenuCategory[] list) : base(list) { }
        public RestMenuCategoryList(List<RestMenuCategory> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
