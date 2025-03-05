using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollDetailList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollDetailList : BaseCollection<PayrollDetail>
	{
		#region Constructors
	    public PayrollDetailList() : base() { }
        public PayrollDetailList(PayrollDetail[] list) : base(list) { }
        public PayrollDetailList(List<PayrollDetail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
