using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestMenuItemList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestMenuItemList : BaseCollection<RestMenuItem>
	{
		#region Constructors
	    public RestMenuItemList() : base() { }
        public RestMenuItemList(RestMenuItem[] list) : base(list) { }
        public RestMenuItemList(List<RestMenuItem> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
