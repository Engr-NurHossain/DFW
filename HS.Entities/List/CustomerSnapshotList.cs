using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerSnapshotList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerSnapshotList : BaseCollection<CustomerSnapshot>
	{
		#region Constructors
	    public CustomerSnapshotList() : base() { }
        public CustomerSnapshotList(CustomerSnapshot[] list) : base(list) { }
        public CustomerSnapshotList(List<CustomerSnapshot> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

