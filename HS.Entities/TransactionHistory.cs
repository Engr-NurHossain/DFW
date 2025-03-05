using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class TransactionHistory 
	{
		public string InvoiceNumber { set; get; }
        public string CustomerName { set; get; }
        public DateTime TransacationDate { set; get; }
        public double InvoiceTotal { set; get; }
        public double InvoiceBalanceDue { set; get; }

    }
}
