using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollHoldBackList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollHoldBackList : BaseCollection<PayrollHoldBack>
	{
		#region Constructors
	    public PayrollHoldBackList() : base() { }
        public PayrollHoldBackList(PayrollHoldBack[] list) : base(list) { }
        public PayrollHoldBackList(List<PayrollHoldBack> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
