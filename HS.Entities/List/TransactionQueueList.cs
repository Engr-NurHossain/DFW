using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TransactionQueueList", Namespace = "http://www.hims-tech.com//list")]	
	public class TransactionQueueList : BaseCollection<TransactionQueue>
	{
		#region Constructors
	    public TransactionQueueList() : base() { }
        public TransactionQueueList(TransactionQueue[] list) : base(list) { }
        public TransactionQueueList(List<TransactionQueue> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
