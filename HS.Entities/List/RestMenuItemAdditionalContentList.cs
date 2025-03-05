using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestMenuItemAdditionalContentList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestMenuItemAdditionalContentList : BaseCollection<RestMenuItemAdditionalContent>
	{
		#region Constructors
	    public RestMenuItemAdditionalContentList() : base() { }
        public RestMenuItemAdditionalContentList(RestMenuItemAdditionalContent[] list) : base(list) { }
        public RestMenuItemAdditionalContentList(List<RestMenuItemAdditionalContent> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
