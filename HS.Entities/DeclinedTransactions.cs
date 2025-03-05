using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class DeclinedTransactions 
	{
		public string CustomerName { set; get; }
        public string CustomerBusinessName { set; get; }
        public int CustomerIdValue { get; set; }

        public int InvId { get; set; }
    }
    public class DeclinedTransactionView
    {
        public List<DeclinedTransactions> DeclinedTransaction { set; get; }
        public int TotalCount { set; get; }

        public double TotalAmountByPage { get; set; }
    }

}
