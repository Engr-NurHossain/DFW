using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TechnicianInventoryList", Namespace = "http://www.piistech.com//list")]	
	public class TechnicianInventoryList : BaseCollection<TechnicianInventory>
	{
		#region Constructors
	    public TechnicianInventoryList() : base() { }
        public TechnicianInventoryList(TechnicianInventory[] list) : base(list) { }
        public TechnicianInventoryList(List<TechnicianInventory> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

