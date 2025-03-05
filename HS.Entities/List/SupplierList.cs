using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SupplierList", Namespace = "http://www.piistech.com//list")]	
	public class SupplierList : BaseCollection<Supplier>
	{
		#region Constructors
	    public SupplierList() : base() { }
        public SupplierList(Supplier[] list) : base(list) { }
        public SupplierList(List<Supplier> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

