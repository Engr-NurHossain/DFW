using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PaymentInfoList", Namespace = "http://www.piistech.com//list")]	
	public class PaymentInfoList : BaseCollection<PaymentInfo>
	{
		#region Constructors
	    public PaymentInfoList() : base() { }
        public PaymentInfoList(PaymentInfo[] list) : base(list) { }
        public PaymentInfoList(List<PaymentInfo> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

