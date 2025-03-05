using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerCancelList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerCancelList : BaseCollection<CustomerCancel>
	{
		#region Constructors
	    public CustomerCancelList() : base() { }
        public CustomerCancelList(CustomerCancel[] list) : base(list) { }
        public CustomerCancelList(List<CustomerCancel> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

