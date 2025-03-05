using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PaymentInfoCustomerDraftList", Namespace = "http://www.piistech.com//list")]	
	public class PaymentInfoCustomerDraftList : BaseCollection<PaymentInfoCustomerDraft>
	{
		#region Constructors
	    public PaymentInfoCustomerDraftList() : base() { }
        public PaymentInfoCustomerDraftList(PaymentInfoCustomerDraft[] list) : base(list) { }
        public PaymentInfoCustomerDraftList(List<PaymentInfoCustomerDraft> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

