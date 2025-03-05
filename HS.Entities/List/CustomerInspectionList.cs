using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerInspectionList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerInspectionList : BaseCollection<CustomerInspection>
	{
		#region Constructors
	    public CustomerInspectionList() : base() { }
        public CustomerInspectionList(CustomerInspection[] list) : base(list) { }
        public CustomerInspectionList(List<CustomerInspection> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

