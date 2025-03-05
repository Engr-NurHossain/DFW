using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "WebsiteLocationMapInfoList", Namespace = "http://www.hims-tech.com//list")]	
	public class WebsiteLocationMapInfoList : BaseCollection<WebsiteLocationMapInfo>
	{
		#region Constructors
	    public WebsiteLocationMapInfoList() : base() { }
        public WebsiteLocationMapInfoList(WebsiteLocationMapInfo[] list) : base(list) { }
        public WebsiteLocationMapInfoList(List<WebsiteLocationMapInfo> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
