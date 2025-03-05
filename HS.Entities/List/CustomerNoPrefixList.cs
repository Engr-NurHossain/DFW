using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerNoPrefixList", Namespace = "http://www.hims-tech.com//list")]	
	public class CustomerNoPrefixList : BaseCollection<CustomerNoPrefix>
	{
		#region Constructors
	    public CustomerNoPrefixList() : base() { }
        public CustomerNoPrefixList(CustomerNoPrefix[] list) : base(list) { }
        public CustomerNoPrefixList(List<CustomerNoPrefix> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
