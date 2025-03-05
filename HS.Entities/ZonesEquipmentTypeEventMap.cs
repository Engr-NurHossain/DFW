using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class ZonesEquipmentTypeEventMap 
	{
        
    }
    public partial class ZonesEquipmentTypeEventMapModel
    {

        public string EquipTypeVal { get; set; }
        public string EquipmentTypeId { get; set; }
        public string EventId { get; set; }
        public int ID { get; set; }
    }
}
