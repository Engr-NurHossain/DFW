using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ServiceDetailInfoViewList", Namespace = "http://www.piistech.com//list")]	
	public class ServiceDetailInfoViewList : BaseCollection<ServiceDetailInfoView>
	{
		#region Constructors
	    public ServiceDetailInfoViewList() : base() { }
        public ServiceDetailInfoViewList(ServiceDetailInfoView[] list) : base(list) { }
        public ServiceDetailInfoViewList(List<ServiceDetailInfoView> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

