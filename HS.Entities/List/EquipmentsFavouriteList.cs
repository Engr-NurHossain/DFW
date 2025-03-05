using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EquipmentsFavouriteList", Namespace = "http://www.hims-tech.com//list")]	
	public class EquipmentsFavouriteList : BaseCollection<EquipmentsFavourite>
	{
		#region Constructors
	    public EquipmentsFavouriteList() : base() { }
        public EquipmentsFavouriteList(EquipmentsFavourite[] list) : base(list) { }
        public EquipmentsFavouriteList(List<EquipmentsFavourite> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
