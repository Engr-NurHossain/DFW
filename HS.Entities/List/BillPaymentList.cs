using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BillPaymentList", Namespace = "http://www.piistech.com//list")]	
	public class BillPaymentList : BaseCollection<BillPayment>
	{
		#region Constructors
	    public BillPaymentList() : base() { }
        public BillPaymentList(BillPayment[] list) : base(list) { }
        public BillPaymentList(List<BillPayment> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

