using System;
namespace HS.Entities
{
    public class InventoryAssignedPO
    {
        //Not included to create any table in database but used to insert data
        public string TechnicianId { get; set; }
        public string EquipmentId { get; set;}

        public string CreatedBy { get; set; }

        public bool IsReceived { get; set; }
        public bool IsApprove { get; set; }
        public bool IsDecline { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int Quantity { get; set; }

        public string Description { get; set; }
        public string ReqSrc { get; set; }
        public string ClosedBy { get; set; }
        public string ReceivedBy { get; set; }
    }
}
