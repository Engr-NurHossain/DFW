using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EquipmentReturnList", Namespace = "http://www.piistech.com//list")]	
	public class EquipmentReturnList : BaseCollection<EquipmentReturn>
	{
		#region Constructors
	    public EquipmentReturnList() : base() { }
        public EquipmentReturnList(EquipmentReturn[] list) : base(list) { }
        public EquipmentReturnList(List<EquipmentReturn> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

