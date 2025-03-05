using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EquipmentClassList", Namespace = "http://www.piistech.com//list")]	
	public class EquipmentClassList : BaseCollection<EquipmentClass>
	{
		#region Constructors
	    public EquipmentClassList() : base() { }
        public EquipmentClassList(EquipmentClass[] list) : base(list) { }
        public EquipmentClassList(List<EquipmentClass> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

