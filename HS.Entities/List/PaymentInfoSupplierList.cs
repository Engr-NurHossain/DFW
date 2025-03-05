using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PaymentInfoSupplierList", Namespace = "http://www.piistech.com//list")]	
	public class PaymentInfoSupplierList : BaseCollection<PaymentInfoSupplier>
	{
		#region Constructors
	    public PaymentInfoSupplierList() : base() { }
        public PaymentInfoSupplierList(PaymentInfoSupplier[] list) : base(list) { }
        public PaymentInfoSupplierList(List<PaymentInfoSupplier> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

