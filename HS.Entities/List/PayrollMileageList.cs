using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollMileageList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollMileageList : BaseCollection<PayrollMileage>
	{
		#region Constructors
	    public PayrollMileageList() : base() { }
        public PayrollMileageList(PayrollMileage[] list) : base(list) { }
        public PayrollMileageList(List<PayrollMileage> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
