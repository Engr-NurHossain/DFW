using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Bill 
	{
		public string RefNo { set; get; }
        public string SupplierName { set; get; }
        public string SupplierCompanyName { set; get; }
    }
    public class BillingReportModel
    {
        public List<Bill> BillList { get; set; }
        public TotalBill TotalBill { get; set; }
    }
    public class TotalBill
    {
        public double TotalAmount { get; set; }
    }
}
