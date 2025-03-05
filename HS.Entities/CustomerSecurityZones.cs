using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class CustomerSecurityZones 
	{
        public string EventCodeVal { get; set; }
        public string LocationVal { get; set; }
        public string EquipmentTypeVal { get; set; }
        public string NmcEqpType { get; set; }
        public string NmcEqpLoc { get; set; }
    }
}
