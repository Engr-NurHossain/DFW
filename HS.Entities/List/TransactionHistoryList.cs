using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TransactionHistoryList", Namespace = "http://www.piistech.com//list")]	
	public class TransactionHistoryList : BaseCollection<TransactionHistory>
	{
		#region Constructors
	    public TransactionHistoryList() : base() { }
        public TransactionHistoryList(TransactionHistory[] list) : base(list) { }
        public TransactionHistoryList(List<TransactionHistory> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

