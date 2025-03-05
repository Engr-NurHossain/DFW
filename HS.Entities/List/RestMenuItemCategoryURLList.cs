using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestMenuItemCategoryURLList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestMenuItemCategoryURLList : BaseCollection<RestMenuItemCategoryURL>
	{
		#region Constructors
	    public RestMenuItemCategoryURLList() : base() { }
        public RestMenuItemCategoryURLList(RestMenuItemCategoryURL[] list) : base(list) { }
        public RestMenuItemCategoryURLList(List<RestMenuItemCategoryURL> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
