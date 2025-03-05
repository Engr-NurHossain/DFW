using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EquipmentList", Namespace = "http://www.piistech.com//list")]	
	public class EquipmentList : BaseCollection<Equipment>
	{
		#region Constructors
	    public EquipmentList() : base() { }
        public EquipmentList(Equipment[] list) : base(list) { }
        public EquipmentList(List<Equipment> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

