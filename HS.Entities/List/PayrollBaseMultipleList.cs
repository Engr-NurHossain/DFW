using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollBaseMultipleList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollBaseMultipleList : BaseCollection<PayrollBaseMultiple>
	{
		#region Constructors
	    public PayrollBaseMultipleList() : base() { }
        public PayrollBaseMultipleList(PayrollBaseMultiple[] list) : base(list) { }
        public PayrollBaseMultipleList(List<PayrollBaseMultiple> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
