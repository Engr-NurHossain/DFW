using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollInstallationFeeList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollInstallationFeeList : BaseCollection<PayrollInstallationFee>
	{
		#region Constructors
	    public PayrollInstallationFeeList() : base() { }
        public PayrollInstallationFeeList(PayrollInstallationFee[] list) : base(list) { }
        public PayrollInstallationFeeList(List<PayrollInstallationFee> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
