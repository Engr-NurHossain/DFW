using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ServiceEquipmentList", Namespace = "http://www.piistech.com//list")]	
	public class ServiceEquipmentList : BaseCollection<ServiceEquipment>
	{
		#region Constructors
	    public ServiceEquipmentList() : base() { }
        public ServiceEquipmentList(ServiceEquipment[] list) : base(list) { }
        public ServiceEquipmentList(List<ServiceEquipment> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

