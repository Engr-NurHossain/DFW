using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BillPaymentHistoryList", Namespace = "http://www.piistech.com//list")]	
	public class BillPaymentHistoryList : BaseCollection<BillPaymentHistory>
	{
		#region Constructors
	    public BillPaymentHistoryList() : base() { }
        public BillPaymentHistoryList(BillPaymentHistory[] list) : base(list) { }
        public BillPaymentHistoryList(List<BillPaymentHistory> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

