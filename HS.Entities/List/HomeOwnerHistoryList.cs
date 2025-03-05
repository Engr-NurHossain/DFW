using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "HomeOwnerHistoryList", Namespace = "http://www.hims-tech.com//list")]	
	public class HomeOwnerHistoryList : BaseCollection<HomeOwnerHistory>
	{
		#region Constructors
	    public HomeOwnerHistoryList() : base() { }
        public HomeOwnerHistoryList(HomeOwnerHistory[] list) : base(list) { }
        public HomeOwnerHistoryList(List<HomeOwnerHistory> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
