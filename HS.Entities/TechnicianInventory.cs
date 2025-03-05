using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class TechnicianInventory 
	{
		public string EquipmentName { get; set; }
        public string EquipmentType { get; set; }

    }
}
