using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SiteContactList", Namespace = "http://www.hims-tech.com//list")]	
	public class SiteContactList : BaseCollection<SiteContact>
	{
		#region Constructors
	    public SiteContactList() : base() { }
        public SiteContactList(SiteContact[] list) : base(list) { }
        public SiteContactList(List<SiteContact> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
