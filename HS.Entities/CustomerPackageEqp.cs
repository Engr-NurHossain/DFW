using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class CustomerPackageEqp 
	{
        public string EquipmentServiceName { set; get; }
        public double EquipmentOldPrice { get; set; }
        public bool IsDeletable { set; get; }
        public int AppointmentEquipmentId { get; set; }
        public int PDCId { get; set; } 
        public string SKU { set; get; }
        public string EquipmentName { get; set; }
        public double Point { get; set; }

    }
}
