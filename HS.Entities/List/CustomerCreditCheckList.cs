using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerCreditCheckList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerCreditCheckList : BaseCollection<CustomerCreditCheck>
	{
		#region Constructors
	    public CustomerCreditCheckList() : base() { }
        public CustomerCreditCheckList(CustomerCreditCheck[] list) : base(list) { }
        public CustomerCreditCheckList(List<CustomerCreditCheck> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

