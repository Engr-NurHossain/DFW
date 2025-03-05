using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EmergencyContact 
	{
		public int LeadId { get; set; }
        public int EmergencyNewId { get; set; }
        public int EmergencyId { get; set; }
        public string Token { get; set; }

        public string RelationShipVal { get; set; }

    }
}
