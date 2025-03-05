using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ServiceDetailInfoList", Namespace = "http://www.piistech.com//list")]	
	public class ServiceDetailInfoList : BaseCollection<ServiceDetailInfo>
	{
		#region Constructors
	    public ServiceDetailInfoList() : base() { }
        public ServiceDetailInfoList(ServiceDetailInfo[] list) : base(list) { }
        public ServiceDetailInfoList(List<ServiceDetailInfo> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

