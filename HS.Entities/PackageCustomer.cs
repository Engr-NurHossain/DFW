using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
    public partial class PackageCustomer
    {
        public int PackageCustomerId { get; set; }
        public string ContractType { get; set; }
        public string PackageName { get; set; }
        public string UserType { set; get; }
        public string PackageCode { set; get; }
        public bool NonConforming{ set; get; }
        public double MinCredit { set; get; }
        public double MaxCredit { set; get; }
        public string SmartInstallTypeVal { get; set; }
        public string SmartSystemTypeVal { get; set; }
        public string ManufacturerName { set; get; }
        public string PackageType { set; get; }
        public string CustomerNum { get; set; }
        public double AdditionFee { get; set; }
        public double FirstMonths { get; set; }
        public double Tax { get; set; }
        public double SalesTax { get; set; }
        public List<SmartPackageDropdownList> PackageDropdownList { get; set; }
        public string SummaryStatus { get; set; }
        public double EquipmentAmount { get; set; }
        public double ServiceFee { get; set; }
        public double AdvancedMonitoring { get; set; }

        public string CustomerNo { get; set; }

        public string BusinessName { get; set; }

        public string LeadSource { get; set; }

        public string SalesLocation { get; set; }

        public double FinancedAmount { get; set; }

        public string Type { get; set; }

        public string SalesPerson { get; set; }

        public string TicketId { get; set; }

    }
    public class PackageCustomerModel
    {
        public List<PackageCustomer> packageCustomers { get; set; }
        public int Totalcount { get; set; }
        public int pageno { get; set; }
        public int pagesize { get; set; }
        public double TotalAdditionFee { get; set; }
        public double TotalFirstMonth { get; set; }
        public double TotalEquipmentAmount { get; set; }
        public double TotalServiceFee { get; set; }
        public double TotalAdvancedMonitoring { get; set; }
        public double TotalTax { get; set; }
        public double TotalSalesTax { get; set; }
        public double SumTotalAdditionFee { get; set; }
        public double SumTotalFirstMonth { get; set; }
        public double SumTotalEquipmentAmount { get; set; }
        public double SumTotalServiceFee { get; set; }
        public double SumTotalAdvancedMonitoring { get; set; }
        public double SumTotalWoTax { get; set; }

        public double SumTotalTax { get; set; }
        public double SumTotalSalesTax { get; set; }
    }
}
