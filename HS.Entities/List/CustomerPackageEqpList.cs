using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerPackageEqpList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerPackageEqpList : BaseCollection<CustomerPackageEqp>
	{
		#region Constructors
	    public CustomerPackageEqpList() : base() { }
        public CustomerPackageEqpList(CustomerPackageEqp[] list) : base(list) { }
        public CustomerPackageEqpList(List<CustomerPackageEqp> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

