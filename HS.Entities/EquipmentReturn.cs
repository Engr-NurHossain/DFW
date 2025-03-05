using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EquipmentReturn 
	{
        public string CustomerName { get; set; }
        public string TechnicianName { get; set; }
        public string EquipmentName { get; set; }
        public int CusIdInt { get; set; }
    }
}
