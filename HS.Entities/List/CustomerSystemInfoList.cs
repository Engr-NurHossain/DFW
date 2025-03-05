using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerSystemInfoList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerSystemInfoList : BaseCollection<CustomerSystemInfo>
	{
		#region Constructors
	    public CustomerSystemInfoList() : base() { }
        public CustomerSystemInfoList(CustomerSystemInfo[] list) : base(list) { }
        public CustomerSystemInfoList(List<CustomerSystemInfo> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

