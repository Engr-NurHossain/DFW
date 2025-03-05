using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollTermSheetManagerList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollTermSheetManagerList : BaseCollection<PayrollTermSheetManager>
	{
		#region Constructors
	    public PayrollTermSheetManagerList() : base() { }
        public PayrollTermSheetManagerList(PayrollTermSheetManager[] list) : base(list) { }
        public PayrollTermSheetManagerList(List<PayrollTermSheetManager> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
