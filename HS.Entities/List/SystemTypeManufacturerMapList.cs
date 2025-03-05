using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SystemTypeManufacturerMapList", Namespace = "http://www.piistech.com//list")]	
	public class SystemTypeManufacturerMapList : BaseCollection<SystemTypeManufacturerMap>
	{
		#region Constructors
	    public SystemTypeManufacturerMapList() : base() { }
        public SystemTypeManufacturerMapList(SystemTypeManufacturerMap[] list) : base(list) { }
        public SystemTypeManufacturerMapList(List<SystemTypeManufacturerMap> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

