using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollAdminFeeList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollAdminFeeList : BaseCollection<PayrollAdminFee>
	{
		#region Constructors
	    public PayrollAdminFeeList() : base() { }
        public PayrollAdminFeeList(PayrollAdminFee[] list) : base(list) { }
        public PayrollAdminFeeList(List<PayrollAdminFee> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
