using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PaymentList", Namespace = "http://www.piistech.com//list")]	
	public class PaymentList : BaseCollection<Payment>
	{
		#region Constructors
	    public PaymentList() : base() { }
        public PaymentList(Payment[] list) : base(list) { }
        public PaymentList(List<Payment> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

