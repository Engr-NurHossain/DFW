using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "LocalizeResourceList", Namespace = "http://www.piistech.com//list")]	
	public class LocalizeResourceList : BaseCollection<LocalizeResource>
	{
		#region Constructors
	    public LocalizeResourceList() : base() { }
        public LocalizeResourceList(LocalizeResource[] list) : base(list) { }
        public LocalizeResourceList(List<LocalizeResource> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

