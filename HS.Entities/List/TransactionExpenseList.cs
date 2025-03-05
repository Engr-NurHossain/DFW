using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TransactionExpenseList", Namespace = "http://www.piistech.com//list")]	
	public class TransactionExpenseList : BaseCollection<TransactionExpense>
	{
		#region Constructors
	    public TransactionExpenseList() : base() { }
        public TransactionExpenseList(TransactionExpense[] list) : base(list) { }
        public TransactionExpenseList(List<TransactionExpense> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

