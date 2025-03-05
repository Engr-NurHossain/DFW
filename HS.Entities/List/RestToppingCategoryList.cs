using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestToppingCategoryList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestToppingCategoryList : BaseCollection<RestToppingCategory>
	{
		#region Constructors
	    public RestToppingCategoryList() : base() { }
        public RestToppingCategoryList(RestToppingCategory[] list) : base(list) { }
        public RestToppingCategoryList(List<RestToppingCategory> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
