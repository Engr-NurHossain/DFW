using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class InventoryWarehouse 
	{
		public string Name { get; set; }
	}
    public partial class InventoryHistory
    {
        public string EquipmentName { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string Name { get; set; }
        public string PurchaseOrderId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public int TicketId { get; set; }
        public ProductCount Count { get; set; }
    }
    public class InventoryHistoryModel
    {
        public List<InventoryHistory> InventoryHistoryList { get; set; }
        public ProductCount TotalProductCount { get; set; }
    }
    public class ProductCount
    {
        public int TotalProduct { get; set; }
    }
}
