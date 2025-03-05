using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestMenuList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestMenuList : BaseCollection<RestMenu>
	{
		#region Constructors
	    public RestMenuList() : base() { }
        public RestMenuList(RestMenu[] list) : base(list) { }
        public RestMenuList(List<RestMenu> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
