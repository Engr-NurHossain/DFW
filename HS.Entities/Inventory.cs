using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.Web;

namespace HS.Entities
{
    public partial class Inventory
    {
        public string EquipmentServiceName { get; set; }
        public string CreatedByName { set; get; }
    }
    public class FilterEquipment
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public int BranchId { get; set; }
        public int ActiveStatus { get; set; }
        public string ActiveInactiveStatus { get; set; }
        public int EquipmentClass { get; set; }
        public int EquipmentCategory { get; set; }
        public int StockStatus { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public string SearchText { get; set; }
        public string EmployeeRole { get; set; }
        public string order { get; set; }
        public string category { get; set; }
        public string manufact { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public bool? GetReport { get; set; }
        public string technician { get; set; }
        public string RepType { get; set; }
        public DateTime Transferred_Date_To { get; set; }
        public DateTime Transferred_Date_From { get; set; }

        //public DateTime Transferred_Date_ToStr { get; set; }
        //public DateTime Transferred_Date_FromStr { get; set; }

        private string _Transferred_Date_ToStr { set; get; }
        private string _Transferred_Date_FromStr { set; get; }
        public string Transferred_Date_ToStr
        {
            get { return this._Transferred_Date_ToStr; }
            set
            {
                _Transferred_Date_ToStr = string.IsNullOrWhiteSpace(value) ? "" : HttpUtility.UrlDecode(value);
                this.Transferred_Date_To = value.ToDateTime();
            }
        }
        public string Transferred_Date_FromStr
        {
            get { return this._Transferred_Date_FromStr; }
            set
            {
                _Transferred_Date_FromStr = string.IsNullOrWhiteSpace(value) ? "" : HttpUtility.UrlDecode(value); ;
                this.Transferred_Date_From = value.ToDateTime();
            }
        }


        public bool? isTransferInventoryReport { get; set; }
        public List<GridSetting> GridList { get; set; }

    }
    public class TotalEquipmentCount
    {
        public int Counter { get; set; }
    }
    public class EquipmentListWithCountModel
    {
        public List<Equipment> EquipmentList { get; set; }
        public TotalEquipmentCount TotalEquipmentCount { get; set; }
        public TotalVendorCost TotalVendorCost { get; set; }
        public TotalTruckInvenorty TotalTruckInvenorty { get; set; }
        public double TotalAmt { get; set; }
        public int TotalQty { get; set; }
        public int TotalPoint { get; set; }

        public double TotalSupplierCost { get; set; }

        public double TotalCost { get; set; }

        public double TotalRetail { get; set; }
    }
    public class TotalTruckInvenorty
    {
        public double TotalSupplierCost { get; set; }
        public int TotalQuantity { get; set; }
        public double TotalAmount { get; set; }
    }
    public class TotalVendorCost
    {
        public double VendorCost { get; set; }
        public double TotalQuantity { get; set; }
    }
    public class PurchaseOrderCount
    {
        public double TotalAmount { get; set; }
        public int TotalQuantity { get; set; }
    }
    public class UsagebyAccountQuantity
    {
        public int TotalInstalled { get; set; }
        public int TotalService { get; set; }
    }
    public class UsagebyAccount
    {
        public List<UsagebyAccount> UsagebyAccountList { set; get; }
        public List<UsageEquipmentList> UsageEquipmentPartialList { set; get; }
        public int Id { set; get; }
        public Guid CustomerId { set; get; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public int InstalledQuantity { get; set; }
        public int ServiceQuantity { get; set; }
        public TotalEquipmentCount Count { set; get; }
        public UsagebyAccountQuantity TotalCount { get; set; }
    }
    public class UsageEquipmentList
    {
        public List<UsageEquipmentList> UsageEquipmentPartialList { set; get; }
        public string EquipName { get; set; }
        public int Quantity { get; set; }
        public int TicketIntId { get; set; }
    }
    public class RMAEquipment
    {
        public List<RMAEquipment> RMAEquipmentList { set; get; }
        public Guid CompanyId { get; set; }
        public string SKU { get; set; }
        public string EquipmentName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }

        public double AmountDouble { get; set; }
        public string Createdby { get; set; }
        public DateTime RMADate { get; set; }
        public bool Status { get; set; }
        public TotalEquipmentCount Count { set; get; }
        public PurchaseOrderCount TotalRMA { set; get; }
    }

    public class PurchaseOrderReportList
    {
        public List<PurchaseOrderReportList> PurchaseOrderList { set; get; }
        public List<PurchaseOrderItemList> PurchaseOrderItemList { get; set; }

        public int Id { get; set; }
        public string PurchaseOrderId { get; set; }
        public string SupplierName { get; set; }
        public int TotalAmount { get; set; }

        public double TotalAmountDouble { get; set; }

        public int Quantity { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
        public TotalEquipmentCount Count { set; get; }
        public PurchaseOrderCount TotalCount { get; set; }
    }
    public class PurchaseOrderItemList
    {
        public List<PurchaseOrderItemList> PurchaseOrderItem { set; get; }
        public string EquipName { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int TotalPrice { get; set; }
    }

    #region Digiture

    #region Filters

    public class DetailedHistoryFilter
    {
        public string SearchText { set; get; }
        public string SearchtextRcv { set; get; }
        public int PageNo { set; get; }
        public int PageNoTransfers { set; get; }
        public int PageSize { set; get; }
        public Guid CompanyId { set; get; }
        public string[] EmployeeIds { set; get; }
        public string[] EquipmentIds { get; set; }
        public string[] ManufacturerIds { get; set; }
        public int BranchId { set; get; }
        public string order { get; set; }
        public string orderrcv { get; set; }
        public bool? IsAllTechPO { get; set; }
        public string selectsts { get; set; }
        public string EstimatorId { get; set; }
        public int ActiveStatus { get; set; }
        public int EquipmentClass { get; set; }
        public int EquipmentCategory { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        //public DetailedHistoryFilter()
        //{
        //    EquipmentIds = new int[] { 0 };
        //}

        public bool GetReport { get; set; }

    }

    public class TechTransferFilter
    {
        public string Searchtext { set; get; }
        public int PageNoTrf { set; get; }
        public int PageNoRcv { set; get; }
        public int PageSize { set; get; }
        public Guid CompanyId { get; set; }
        public Guid EmployeeId { set; get; }
        public string[] TFEmployeeIds { set; get; }
        public string[] TTEmployeeIds { set; get; }
        public string[] RFEmployeeIds { set; get; }
        public string[] RTEmployeeIds { set; get; }
        public string order { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool GetReport { get; set; }

    }

    #endregion Filters


    #region VM

    public class DetailedHistoryVM
    {
        public List<DetailedHistoryItem> Items { set; get; }
        public int ItemsCount { get; set; }

        public List<GenericDropDownFilter> EquipmentsList { set; get; }
        public List<GenericDropDownFilter> ManufacturersList { set; get; }
    }

    public class DetailedEquipmentVM
    {
        public List<DetailedEquipmentTicketListItem> Tickets { set; get; }
        public int TicketsCount { get; set; }
        public List<DetailedEquipmentTransferListItem> Transfers { set; get; }
        public int TransfersCount { get; set; }
    }

    public class TransferRequestVM
    {
        public List<TransferRequest> Items { set; get; }
        public int ItemsCount { get; set; }
    }

    #endregion VM



    public class DetailedHistoryItem
    {
        public int Id { set; get; }
        public string EquipmentId { set; get; }
        public string EquipmentName { set; get; }
        public string SKU { set; get; }
        public int MfgId { get; set; }
        public string Manufacturer { get; set; }
        public int Start { set; get; }
        public int AddToTicket { set; get; }
        public int PulledFromTicket { set; get; }
        public int TransferredOut { set; get; }
        public int TransferredIn { set; get; }
        public int WH_In { set; get; }
        public int WH_Out { set; get; }

        public int OnHand { set; get; }

    }

    public class DetailedEquipmentTicketListItem
    {
        public int TicketId { set; get; }
        public int Id { set; get; }
        public string EquipmentId { set; get; }
        public string EquipmentName { set; get; }
        public string SKU { set; get; }
        
        public int Quantity { set; get; }
        public string Date { set; get; }
    }

    public class DetailedEquipmentTransferListItem
    {
        public string TechName { set; get; }
        public int EquipmentId { set; get; }
        public string EquipmentName { set; get; }
        public string SKU { set; get; }

        public int Quantity { set; get; }
        public string Date { set; get; }
        public DateTime _ReceivedDate { get; set; }
        public DateTime _CreatedDate { get; set; }
    }
    
    public class TransferRequest
    {
        public int TransferRequestId { set; get; }
        public string EquipmentName { set; get; }
        public string SKU { set; get; }
        public string TransferFrom { set; get; }
        public int Qty { set; get; }
        public int OnHand { set; get; }
        public Nullable<DateTime> TransferDate { set; get; }

    }
    
    public class TechTransferRequestItem
    {
        public Guid EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public Guid TechnicianId { get; set; }
        public string TechnicianName { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityToTransfer { get; set; }
    }

    public class TechTransferRequest
    {
        public List<AssignedInventoryTechReceived> Items { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ReceivedDate { get; set; }

        public List<TechTransferRequestItem> ItemList { get; set; }


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
            set
            {
                _ReceiveNow = value;
            }
            get
            {
                return _ReceiveNow;
            }
        }
    }

    //PVM is View Model for Partial View
    //public class Equipmenent_List_PVM
    //{
    //    List<SelectListItem> Equipments { get; set; }
    //}


    public class BulkTechReceiveRequest
    {
        public int id { get; set; }
        public Guid eqpid { get; set; }
        public Guid techid { get; set; }
        public int qty { get; set; }
        public string reqSrc { get; set; }
    }

        #endregion Digiture
    }
