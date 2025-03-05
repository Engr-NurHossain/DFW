using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
    public partial class CustomerAppointmentEquipment
    {
        //public string ServiceOrderEquipmentServiceName { get; set; }
        //public string WorkOrderEquipmentServiceName { get; set; }
        public int EquipmentClassId { set; get; }
        public int technician { get; set; }
        public int TotalEq { get; set; }
        public int QtyOnInstall { get; set; }
        public double CostQtyOnInstall { get; set; }
        public int QtyOnService { get; set; }
        public double CostQtyOnService { get; set; }
        public double CurrentInventoryValue { get; set; }
        public string EquipmentServiceName { set; get; }
        public string SnapshotDescription { get; set; }
        //public string LeadEquipmentName { get; set; }
        public double EquipmentOldPrice { get; set; }
        public bool IsDeletable { set; get; }
        public int AppointmentEquipmentId { get; set; }
        public int PDCId { get; set; }
        public string FileName { get; set; }
        public string FileFullName { get; set; }
        public string FileType { get; set; }
        public string TicketsId { get; set; }
        public string FileDescription { get; set; }
        public int QuantityOnHand { get; set; }
        public int QTYPending { get; set; }
        public int OrderingQuantity { get; set; }
        public Guid TechnicianId { get; set; }
        public double SupplierCost { get; set; }
        public Guid SupplierId { get; set; }
        public double Retail { get; set; }
        public double RepCost { get; set; }
        public bool CheckedEqp { get; set; }
        public string SKU { set; get; }
        public int UpsoldEquipments { get; set; }
        public int UpsoldServices { get; set; }
        public double UpsoldEquipmentsTotalPrice { get; set; }
        public double UpsoldServicesTotalPrice { get; set; }
        public double UpsoldEquipmentsTotalQuantity { get; set; }
        public double UpsoldServicesTotalQuantity { get; set; }
        public int TotalUpsold { get; set; }
        public int InventoryTechQty { get; set; }
        public int TicketIntId { get; set; }
        public bool BillCheck { get; set; }
        public int WarehouseQTY { get; set; }
        public int QTYOnHand { get; set; }
        public string ProductSKU { get; set; }
        public string ManufacturerName { get; set; }
        public string ProductType { get; set; }
        public double CostPerItem { get; set; }
        public double CostQuantityNeeded { get; set; }
        public double CostQTYOrder { get; set; }
        public string EquipmentName { get; set; }
        public string PickupShipped { get; set; }
        public string FullfillmentDate { get; set; }
        public double Point { get; set; }
        public double TotalPoint { get; set; }
        public bool IsARBEnabled { get; set; }
        public double Equipmentvendorcost { get; set; }
    }
    public class CustomerAppointmentEquipmentListModel
    {
        public List<CustomerAppointmentEquipment> CustomerAppointmentEquipments { set; get; }
        public string Searchtext { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public int TotalCount { set; get; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
    public class CustomerAppointmentEquipmentPoint
    {
        public string EmployeeName { set; get; }
        public double Point { set; get; }
    }
}
