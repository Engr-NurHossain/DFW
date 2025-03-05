using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "WebsiteConfigurationList", Namespace = "http://www.hims-tech.com//list")]	
	public class WebsiteConfigurationList : BaseCollection<WebsiteConfiguration>
	{
		#region Constructors
	    public WebsiteConfigurationList() : base() { }
        public WebsiteConfigurationList(WebsiteConfiguration[] list) : base(list) { }
        public WebsiteConfigurationList(List<WebsiteConfiguration> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
