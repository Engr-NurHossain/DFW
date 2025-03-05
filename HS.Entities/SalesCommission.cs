using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class SalesCommission 
	{
        public string CustomerName { get; set; }
        public string SalesPerson { get; set; }
        public int TiketIdValue { get; set; }
        public int CustomerIdValue { get; set; }
        public double BalanceDue { set; get; }
    }
}
