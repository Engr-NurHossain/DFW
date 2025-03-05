using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class AssignedInventoryTechReceived 
	{
		public List<InventoryTech> ListInventoryTech { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string ReceivedByName { get; set; }
        public string TransferByName { get; set; }
        public int TotalQuantity { get; set; }
        public string Type { get; set; }
        public string created_BY { get; set; }
        public string Closed_By { get; set; }
    }

    public class TechReceiveListModel
    {
        public List<AssignedInventoryTechReceived> ListAssignedInventoryTechReceived { set; get; }
        public List<AssignedInventoryTechReceived> ListAssignedInventoryTechApprove { set; get; }
        public string Searchtext { set; get; }
        public int PageNo { set; get; }
        public int PageNoRcv { set; get; }
        public int PageSize { set; get; }
        public int TotalCountTrf { set; get; }
        public int TotalCountRcv { set; get; }

        public List<GenericDropDownFilter> TechTrfFromList { set; get; }
        public List<GenericDropDownFilter> TechTrfToList { set; get; }

        public List<GenericDropDownFilter> TechRcvFromList { set; get; }
        public List<GenericDropDownFilter> TechRcvToList { set; get; }
    }
}
