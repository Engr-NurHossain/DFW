using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class PayrollBrinks 
	{
        public int CustomerIntId { get; set; }
        public string CustomerName { get; set; }
        public double EquipmentRepCost { get; set; }
        public double UpFrontCollect { get; set; }
        public double EquipmentAdjustment { get; set; }
        public double InstallationFee { get; set; }
        public double TotalPassThrus { get; set; }
        public List<EquipmentAdjust> EquipmentAdjustList { get; set; }
    }
    public class EquipmentAdjust
    {
        public string EquipmentName { get; set; }
        public double Cost { get; set; }
    }
}
