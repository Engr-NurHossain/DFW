using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Equipment 
	{
        public string EquipmentClass { get; set; }
        public int QtyOnHand { get; set; }
        public int InQueue { get; set; }
        public int LocQoH{ get; set; }
        public double TotalPrice { set; get; }
        public double UnitPriceAppointmentEquipment { set; get; }
        public double QuantityAppointmentEquipment { set; get; }
        public string EquipmentType { get; set; }
        public string ManufacturerName { set; get; }
        public double ActivationFee { get; set; }
        public string SupplierName { get; set; }
        public string ProfilePicture { set; get; }
        public string Category { get; set; }
        public List<ServiceEquipment> ServiceEquipments { set; get; }
        public List<EquipmentVendor> EquipmentVendorList { get; set; }

        public List<EquipmentManufacturer> EquipmentManufacturerList { get; set; }

        private string _StrAsOfDate;
        public string TechnicianName { get; set; }
        public string ReceivedBy { get; set; }
        public double AmountTruck { get; set; }
        public DateTime TransferredDate { get; set; }
        public string TransferDescription { get; set; }
        public bool IsTransfered { get; set; }
        public string StrAsOfDate
        {
            get
            {
                return _StrAsOfDate;
            }
            set
            {
                _StrAsOfDate = value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var values = value.Trim().Split('/');
                    if (values.Length == 3)
                    {
                        int Day = 0;
                        int Month = 0;
                        int Year = 0;
                        if (int.TryParse(values[0], out Month) && Month > 0 && Month < 13
                            && int.TryParse(values[1], out Day) && Day > 0 && Day < 32
                            && int.TryParse(values[2], out Year) && Year > 1980)
                        {
                            this.AsOfDate = new DateTime(Year, Month, Day);
                        }
                    }
                }
            }
        }

        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double DiscountUnitPricce { get; set; }
        public double Total { get; set; }
        public double TotalestimatorTaxAmount { get; set; }
        public double MonthlyRate { set; get; }
        public double DiscountRate { set; get; }
        public string Description { set; get; }
        public int technician { get; set; }
        public int TotalEq { get; set; }
        public bool IsEqpExist { get; set; }
        public double TotalDiscountUnitPrice { get; set; }
        public double DiscountPercentage { get; set; }
        public double DiscountInCurrency { get; set; }

        public double FIFO { get; set; }

    }

    public class CreateEquipment
    {
        public Equipment Equipment { set; get; }
        public List<Inventory> InventoryList { set; get; }
        public List<InventoryHistory> InventoryHistoryList { set; get; }

        public List<InventoryHistory> Top50InventoryHistoryList { set; get; }

        public List<ServiceEquipment> ServiceEquipmentList { set; get; }
        public List<InventoryTech> ListInventoryTech { get; set; }
        
    }
    public class MassRestock
    {
        public Guid EquipmentId { get; set; }
        public Guid TechnicianId { get; set; }
        public string Name { get; set; }
        public string ManufacturerName { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public int ReorderPoint { get; set; }
        public int WHReorderPoint { get; set; }
        public int Have { get; set; }
        public int New { get; set; }
    }
    public class MassPO
    {
        public string DemandOrderId { get; set; }
        public string TechDemandOrderId { get; set; }
        public Guid EquipmentId { get; set; }
        public string Name { get; set; }
        public string ManufacturerName { get; set; }
        public string SKU { get; set; }
        public string PrimaryVendor { get; set; }
        public Guid SupplierId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
    public class TotalInventoryCount
    {
        public int TotalOnHandStartDate { get; set; }
        public int TotalOnHandEndDate { get; set; }
        public int TotalUsed { get; set; }
        public int TotalPurchase { get; set; }
        public int TotalRMA { get; set; }
    }
    public class InventoryCount
    {
        //public InventoryCount InventoryCountModel { get; set; }
        public List<InventoryCount> InventoryCountList { get; set; }
        public Count Count { get; set; }
        public Guid EquipmentId { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public int QuantityOnStartDate { get; set; }
        public int QuantityOnEndDate { get; set; }
        public int Used { get; set; }
        public int Purchase { get; set; }
        public int RMA { get; set; }
        public int pageno { get; set; }
        public int pagesize { get; set; }
        public TotalInventoryCount TotalInventoryCount { get; set; }
    }
    public class Count
    {
        public int TotalCount { get; set; }
    }
    public class UsedInventoryCount
    {
        public List<UsedInventoryCount> UsedInventoryCountList { get; set; }
        public int CustomerId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
    }
    public class PurchaseInventoryCount
    {
        public List<PurchaseInventoryCount> PurchaseInventoryCountList { get; set; }
        public int Id { get; set; }
        public string PurchaseOrderId { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
    public class RMADetails
    {
        public List<RMADetails> RMADetailsList { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
    }
    public class MassRestockModel
    {
        public List<MassRestock> MassRestockList { get; set; }
        public int TotalQty { get; set; }
        public int TotalHave { get; set; }
        public int TotalPoint { get; set; }
        public int TotalWHPoint { get; set; }
    }

}
