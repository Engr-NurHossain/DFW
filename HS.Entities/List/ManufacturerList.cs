using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ManufacturerList", Namespace = "http://www.piistech.com//list")]	
	public class ManufacturerList : BaseCollection<Manufacturer>
	{
		#region Constructors
	    public ManufacturerList() : base() { }
        public ManufacturerList(Manufacturer[] list) : base(list) { }
        public ManufacturerList(List<Manufacturer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

