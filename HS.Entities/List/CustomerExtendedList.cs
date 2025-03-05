using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerExtendedList", Namespace = "http://www.hims-tech.com//list")]	
	public class CustomerExtendedList : BaseCollection<CustomerExtended>
	{
		#region Constructors
	    public CustomerExtendedList() : base() { }
        public CustomerExtendedList(CustomerExtended[] list) : base(list) { }
        public CustomerExtendedList(List<CustomerExtended> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
