using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerAddressList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerAddressList : BaseCollection<CustomerAddress>
	{
		#region Constructors
	    public CustomerAddressList() : base() { }
        public CustomerAddressList(CustomerAddress[] list) : base(list) { }
        public CustomerAddressList(List<CustomerAddress> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

