using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class BillDetail 
	{
        public string EquipmentName { set; get; }
        public string EquipmentDescription { set; get; }
    }
}
