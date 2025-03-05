using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class PurchaseOrderBranch 
	{
		public string TechName { get; set; }
        public string Email { get; set; }
        public int DOTId { get; set; }
        public int TicketId { get; set; }
    }

    public class OpenDemandOrderBranchModel
    {
        public PurchaseOrderBranch PurchaseOrderBranch { set; get; }
        public List<PurchaseOrderDetail> PurchaseOrderDetail { set; get; }
    }

}
