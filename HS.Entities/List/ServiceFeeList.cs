using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ServiceFeeList", Namespace = "http://www.piistech.com//list")]	
	public class ServiceFeeList : BaseCollection<ServiceFee>
	{
		#region Constructors
	    public ServiceFeeList() : base() { }
        public ServiceFeeList(ServiceFee[] list) : base(list) { }
        public ServiceFeeList(List<ServiceFee> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

