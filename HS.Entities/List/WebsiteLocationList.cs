using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "WebsiteLocationList", Namespace = "http://www.hims-tech.com//list")]	
	public class WebsiteLocationList : BaseCollection<WebsiteLocation>
	{
		#region Constructors
	    public WebsiteLocationList() : base() { }
        public WebsiteLocationList(WebsiteLocation[] list) : base(list) { }
        public WebsiteLocationList(List<WebsiteLocation> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
