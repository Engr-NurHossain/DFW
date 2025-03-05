using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class CustomerPackageService 
	{
        public double UnitPrice { set; get; }
        public double DiscountUnitPricce { set; get; }
        public string EquipmentServiceName { set; get; }

        public string Manufacturer { set; get; }
        public string Location { set; get; }
        public string Type { set; get; }
        public string Model { set; get; }
        public string Finish { set; get; }
        public string Capacity { set; get; }

        public string SKU { set; get; }

        public bool? ChargeForFirstEquipment { set; get; }
        public string EquipmentName { get; set; }
        public bool IsARBEnabled { set; get; }
    }
}
