using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerSystemNoList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerSystemNoList : BaseCollection<CustomerSystemNo>
	{
		#region Constructors
	    public CustomerSystemNoList() : base() { }
        public CustomerSystemNoList(CustomerSystemNo[] list) : base(list) { }
        public CustomerSystemNoList(List<CustomerSystemNo> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

