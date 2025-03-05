using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerCancellationReasonList", Namespace = "http://www.hims-tech.com//list")]	
	public class CustomerCancellationReasonList : BaseCollection<CustomerCancellationReason>
	{
		#region Constructors
	    public CustomerCancellationReasonList() : base() { }
        public CustomerCancellationReasonList(CustomerCancellationReason[] list) : base(list) { }
        public CustomerCancellationReasonList(List<CustomerCancellationReason> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
