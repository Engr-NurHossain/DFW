using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TransactionList", Namespace = "http://www.piistech.com//list")]	
	public class TransactionList : BaseCollection<Transaction>
	{
		#region Constructors
	    public TransactionList() : base() { }
        public TransactionList(Transaction[] list) : base(list) { }
        public TransactionList(List<Transaction> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

