using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
    public class InventoryTechModel
    {
        public List<InventoryTech> InventoryTech { set; get; }
        public string Searchtext { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public int TotalCount { set; get; }
        public int TotalQuantity { get; set; }
    }
    public partial class InventoryTech
    {
        public DateTime TransferredDate { get; set; }
        public string TechnicianName { get; set; }
        public string Category { get; set; }
        public string Manufacture { get; set; }
        public string EquipName { get; set; }
        public string Desc { get; set; }
        public string Sku { get; set; }
        public string Technician { get; set; }
        public string empName { get; set; }

    }
    public partial class TransferFromTechnicianModel
    {
        public string Equipmentidlist { get; set; }
        public Guid Technicianid { get; set; }
        public Guid FromTechnicianid { get; set; }
    }
}
