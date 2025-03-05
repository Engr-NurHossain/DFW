using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class PurchaseOrderWarehouse 
	{
		public string Name { get; set; }
        public string VendorName { get; set; }
        public string SoldByVal { set; get; }
        public string ShippingViaVal { set; get; }
        public string FinalStatus { get; set; }
        public string TechnicianName { get; set; }
        public int EstimatorIntId { get; set; }
    }
}
