using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerCancellationQueueList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerCancellationQueueList : BaseCollection<CustomerCancellationQueue>
	{
		#region Constructors
	    public CustomerCancellationQueueList() : base() { }
        public CustomerCancellationQueueList(CustomerCancellationQueue[] list) : base(list) { }
        public CustomerCancellationQueueList(List<CustomerCancellationQueue> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

