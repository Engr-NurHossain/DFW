using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class PurchaseOrder 
	{
		public string SupplierName { set; get; }
        public string SoldByVal { set; get; }
        public string ShippingViaVal { set; get; }
        public int BranchId { set; get; }
        public string Action { set; get; }
        public bool IsReceived { set; get; }
        #region Date Perpouse
        private string _StrOrderDate;
        public string StrOrderDate
        {
            get
            {
                return _StrOrderDate;
            }
            set
            {
                _StrOrderDate = value;
                var values = value.Trim().Split('/');
                if (values.Length==3)
                {
                    int Day = 0;
                    int Month = 0;
                    int Year = 0;
                    if(int.TryParse(values[0],out Month) && Month>0  && Month < 13
                        && int.TryParse(values[1], out Day) && Day > 0 && Day < 32
                        && int.TryParse(values[2], out Year) && Year > 1980)
                    {
                        this.OrderDate = new DateTime(Year, Month, Day);
                    }
                }
            }
        }
        #endregion
    }
    public class POListModel
    {
        public List<PurchaseOrder> PurchaseOrderList { set; get; }
        public List<PurchaseOrderWarehouse> PurchaseOrderWarehouseList { set; get; }
        public List<PurchaseOrderTech> PurchaseOrderTechList { set; get; }
        public List<PurchaseOrderBranch> PurchaseOrderBranchList { set; get; }
        public List<PurchaseOrderTechReceived> PurchaseOrderTechReceivedList { set; get; }
        public List<POReport> POReportList { set; get; }
        public List<PONewReport> POReportNewList { set; get; }
        public string Searchtext { set; get; }
        public string PrefixForPurchaseOrderId { get; set; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public int TotalCount { set; get; }
        public int TotalQuantity { get; set; }
        public double TotalAmount { get; set; }
        public List<SKU> equipmentSKUs { set; get; }
        public PurchaseOrderWarehouse PurchaseOrderWarehouse { set; get; }
    }
    public class POReport
    {
        public int Id { set; get; }
        public string PurchaseOrderId { set; get; }
        public string CreatedBy { set; get; }
        public string ReceivedBy { set; get; }
        public int Quantity { set; get; }
        public string Status { set; get; }
        public double PoAmount { set; get; }
        public string Searchtext { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public int TotalCount { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime ReceivedDate { set; get; }
    }


    public class PONewReport
    {
        public int Id { set; get; }
        public string PurchaseOrderId { set; get; }
        public string CreatedBy { set; get; }
        public string ReceivedBy { set; get; }
        public int Quantity { set; get; }
        public int RecieveQty { set; get; }
        public string Status { set; get; }
        public double PoAmount { set; get; }
        public string Searchtext { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public int TotalCount { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime OrderDate { set; get; }

        public DateTime ReceivedDate { set; get; }
        public string Vender { set; get; }
        public string Category { set; get; }
        public string Manufacturer { set; get; }
        public string Description { set; get; }
        public string SKU { set; get; }
        public double UnitPrice { set; get; }
        public double TotalPrice { set; get; }

        public string EquipName { set; get; }
        public string Po_Description { get; set; }

        public string TechnicianName { get; set; }


    }

    public class BIListModel
    {
        public List<EquipmentReturn> EquipmentReturnList { set; get; }
        public string Searchtext { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public int TotalCount { set; get; }
        public int TotalQuantity { get; set; }
    }

    public class PurchaseOrderFilter
    {
        public string Searchtext { set; get; }
        public string SearchtextRcv { set; get; }
        public int PageNo { set; get; }
        public int PageNoRcv { set; get; }
        public int PageSize { set; get; }
        public Guid CompanyId { set; get; }
        public Guid EmployeeId { set; get; }
        public int BranchId { set; get; }
        public string order { get; set; }
        public string orderrcv { get; set; }
        public bool? IsAllTechPO { get; set; }
        public string selectsts { get; set; }
        public string EstimatorId { get; set; }
        public string Start { get; set; }
        public string End { get; set; }  
        public bool GetReport { get; set; }
    }

    public class TechReceiveFilter
    {
        public string Searchtext { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public Guid CompanyId { get; set; }
        public Guid EmployeeId { set; get; }
        public string order { get; set; }
        public string Type { get; set; }
    }

    public class BadInventoryFilter
    {
        public string Searchtext { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public Guid CompanyId { set; get; }
        //public Guid EmployeeId { set; get; }
        //public int BranchId { set; get; }
        public string order { get; set; }
        public Guid TechnicianId { get; set; }
        public string Status { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool? GetReport { get; set; }
        public string tab { get; set; }

        public string TechnicianIDList { get; set; }
        public string StatusIDList { get; set; }
        public DateTime Purchase_Date_From { get; set; }
        public DateTime Purchase_Date_To { get; set; }
        
    }
    public class MassRestockFilter
    {
        public string Searchtext { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public Guid CompanyId { set; get; }
        public int Id { get; set; }
        public string Order { get; set; }
        public bool ShowAll { get; set; }
        public Guid TechnicianId { get; set; }
        public DateTime FDate { get; set; }
        public DateTime LDate { get; set; }
    }
    public class CreatePurchaseOrder
    {
        public PurchaseOrderTech PurchaseOrderTech { set; get; }
        public PurchaseOrderBranch PurchaseOrderBranch { set; get; }
        public PurchaseOrderWarehouse PurchaseOrderWarehouse { set; get; }
        public PurchaseOrder PurchaseOrder { set; get; }
        public List<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
        public Supplier Supplier { set; get; }
        public Company Company { set; get; }
        public string CompanyAddressFormat { set; get; }
        public string SupplierAddressFormat { set; get; }
        public string ShortUrl { set; get; }
        public string OpenTab { set; get; }
        public Guid TechnicianId { get; set; }
        public int TicketId { get; set; }
        private bool _ReceiveNow = false;
        public bool ReceiveNow
        {
            set {
                _ReceiveNow = value;
            }
            get {
                return _ReceiveNow;
            }
        }
    }
}
