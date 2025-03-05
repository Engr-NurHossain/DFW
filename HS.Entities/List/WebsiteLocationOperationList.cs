using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "WebsiteLocationOperationList", Namespace = "http://www.hims-tech.com//list")]	
	public class WebsiteLocationOperationList : BaseCollection<WebsiteLocationOperation>
	{
		#region Constructors
	    public WebsiteLocationOperationList() : base() { }
        public WebsiteLocationOperationList(WebsiteLocationOperation[] list) : base(list) { }
        public WebsiteLocationOperationList(List<WebsiteLocationOperation> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
