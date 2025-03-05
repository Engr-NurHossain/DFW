using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ServiceAreaZipcodeList", Namespace = "http://www.piistech.com//list")]	
	public class ServiceAreaZipcodeList : BaseCollection<ServiceAreaZipcode>
	{
		#region Constructors
	    public ServiceAreaZipcodeList() : base() { }
        public ServiceAreaZipcodeList(ServiceAreaZipcode[] list) : base(list) { }
        public ServiceAreaZipcodeList(List<ServiceAreaZipcode> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

