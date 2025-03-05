using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ServiceMapList", Namespace = "http://www.piistech.com//list")]	
	public class ServiceMapList : BaseCollection<ServiceMap>
	{
		#region Constructors
	    public ServiceMapList() : base() { }
        public ServiceMapList(ServiceMap[] list) : base(list) { }
        public ServiceMapList(List<ServiceMap> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

