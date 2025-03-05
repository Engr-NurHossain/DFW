using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RMRBillingMismatchList", Namespace = "http://www.hims-tech.com//list")]	
	public class RMRBillingMismatchList : BaseCollection<RMRBillingMismatch>
	{
		#region Constructors
	    public RMRBillingMismatchList() : base() { }
        public RMRBillingMismatchList(RMRBillingMismatch[] list) : base(list) { }
        public RMRBillingMismatchList(List<RMRBillingMismatch> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
