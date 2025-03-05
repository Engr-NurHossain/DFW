using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class PurchaseOrderDetail 
	{
		public int QuantityAvailable { set; get; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public string ReceiveBy { get; set; }
        public string ReceiveFor { get; set; }
        public DateTime RecieveDate { get; set; }
        public string eqDescription { get; set; }

        public int CurrentQty { get; set; }
    }
}
