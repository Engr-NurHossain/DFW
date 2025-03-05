using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class InvoiceDetail 
	{
        public string EquipmentName { set; get; }
        public string EquipmentDescription { set; get; }
        public EquipmentFile EquipmentFile { get; set; }
        public double VendorPrice { get; set; }
        public double TotalRetail { get; set; }
        public int Order { set; get; }
        public int EquipmentClassId { get; set; }

        public double Equipmentvendorcost { get; set; }
    }
}
