using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerAddendumList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerAddendumList : BaseCollection<CustomerAddendum>
	{
		#region Constructors
	    public CustomerAddendumList() : base() { }
        public CustomerAddendumList(CustomerAddendum[] list) : base(list) { }
        public CustomerAddendumList(List<CustomerAddendum> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

