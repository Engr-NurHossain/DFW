using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollTermSheetList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollTermSheetList : BaseCollection<PayrollTermSheet>
	{
		#region Constructors
	    public PayrollTermSheetList() : base() { }
        public PayrollTermSheetList(PayrollTermSheet[] list) : base(list) { }
        public PayrollTermSheetList(List<PayrollTermSheet> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
