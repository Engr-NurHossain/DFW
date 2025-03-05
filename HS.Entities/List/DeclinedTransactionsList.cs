using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "DeclinedTransactionsList", Namespace = "http://www.piistech.com//list")]	
	public class DeclinedTransactionsList : BaseCollection<DeclinedTransactions>
	{
		#region Constructors
	    public DeclinedTransactionsList() : base() { }
        public DeclinedTransactionsList(DeclinedTransactions[] list) : base(list) { }
        public DeclinedTransactionsList(List<DeclinedTransactions> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

