using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerIsPcCreditApplicationList", Namespace = "http://www.hims-tech.com//list")]	
	public class CustomerIsPcCreditApplicationList : BaseCollection<CustomerIsPcCreditApplication>
	{
		#region Constructors
	    public CustomerIsPcCreditApplicationList() : base() { }
        public CustomerIsPcCreditApplicationList(CustomerIsPcCreditApplication[] list) : base(list) { }
        public CustomerIsPcCreditApplicationList(List<CustomerIsPcCreditApplication> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
