using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BundleEquipmentList", Namespace = "http://www.piistech.com//list")]	
	public class BundleEquipmentList : BaseCollection<BundleEquipment>
	{
		#region Constructors
	    public BundleEquipmentList() : base() { }
        public BundleEquipmentList(BundleEquipment[] list) : base(list) { }
        public BundleEquipmentList(List<BundleEquipment> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

