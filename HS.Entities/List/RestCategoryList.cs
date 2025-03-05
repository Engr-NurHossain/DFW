using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestCategoryList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestCategoryList : BaseCollection<RestCategory>
	{
		#region Constructors
	    public RestCategoryList() : base() { }
        public RestCategoryList(RestCategory[] list) : base(list) { }
        public RestCategoryList(List<RestCategory> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
