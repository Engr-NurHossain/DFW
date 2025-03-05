using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerReferList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerReferList : BaseCollection<CustomerRefer>
	{
		#region Constructors
	    public CustomerReferList() : base() { }
        public CustomerReferList(CustomerRefer[] list) : base(list) { }
        public CustomerReferList(List<CustomerRefer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

