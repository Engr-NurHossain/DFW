using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerCreditList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerCreditList : BaseCollection<CustomerCredit>
	{
		#region Constructors
	    public CustomerCreditList() : base() { }
        public CustomerCreditList(CustomerCredit[] list) : base(list) { }
        public CustomerCreditList(List<CustomerCredit> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

