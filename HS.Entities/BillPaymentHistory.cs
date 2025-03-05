using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class BillPaymentHistory 
	{
        public List <BillPaymentHistory> BillPaymentHistoryList { get; set; }

        public string BillNo { set; get; }
        public string SupplierName { set; get; }
        public string SupplierCompanyName { set; get; }
        public DateTime TransacationDate { set; get; }
        public int BpaymentId { get; set; }
        public double Tamount { get; set; }
        public DateTime Ddate { get; set; }
        public string Bstatus { get; set; }
        public string BType { get; set; }
        public double BBalance { get; set; }
        public string Bmethod { get; set; }
        public string Bbillid { get; set; }
        public string Bname { get; set; }
        public string BReferenceNo { get; set; }

        public double TotalAmount { get; set; }
        
    }
}
