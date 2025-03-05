using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
    public partial class Supplier
    {
        public List<ShowBillModel> SupplierBillList { get; set; }
        public List<BillPaymentHistory> SupplierPaymentList { get; set; }
        public List<BillDetail> objBillDetailsVendor { get; set; }
        public string ContactPersonName { get; set; }
        public double Cost { get; set; }
        public string SupplierAddress { get; set; }
        public TotalCount TotalCount { get; set; }
    }
    public partial class SupplierCustom
    {
        public int Id { get; set; }
        public Guid SupplierId { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public string SalesRepName { get; set; }
        public double Cost { get; set; }
    }
    public partial class SupplierIdList
    {
        public Guid SupplierId { get; set; }
    }
}
