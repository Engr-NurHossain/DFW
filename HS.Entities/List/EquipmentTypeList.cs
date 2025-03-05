using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EquipmentTypeList", Namespace = "http://www.piistech.com//list")]	
	public class EquipmentTypeList : BaseCollection<EquipmentType>
	{
		#region Constructors
	    public EquipmentTypeList() : base() { }
        public EquipmentTypeList(EquipmentType[] list) : base(list) { }
        public EquipmentTypeList(List<EquipmentType> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

