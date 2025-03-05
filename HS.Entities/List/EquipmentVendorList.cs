using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EquipmentVendorList", Namespace = "http://www.piistech.com//list")]	
	public class EquipmentVendorList : BaseCollection<EquipmentVendor>
	{
		#region Constructors
	    public EquipmentVendorList() : base() { }
        public EquipmentVendorList(EquipmentVendor[] list) : base(list) { }
        public EquipmentVendorList(List<EquipmentVendor> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

