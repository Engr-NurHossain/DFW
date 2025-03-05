using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EquipmentReturnVendorList", Namespace = "http://www.piistech.com//list")]	
	public class EquipmentReturnVendorList : BaseCollection<EquipmentReturnVendor>
	{
		#region Constructors
	    public EquipmentReturnVendorList() : base() { }
        public EquipmentReturnVendorList(EquipmentReturnVendor[] list) : base(list) { }
        public EquipmentReturnVendorList(List<EquipmentReturnVendor> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

