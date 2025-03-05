using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestMenuItemCategoryList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestMenuItemCategoryList : BaseCollection<RestMenuItemCategory>
	{
		#region Constructors
	    public RestMenuItemCategoryList() : base() { }
        public RestMenuItemCategoryList(RestMenuItemCategory[] list) : base(list) { }
        public RestMenuItemCategoryList(List<RestMenuItemCategory> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
