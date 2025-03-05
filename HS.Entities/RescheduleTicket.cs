using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class RescheduleTicket 
	{
		public DateTime CompletionDate { get; set; }
        public Guid CustomerId { get; set; }
        public List<AdditionalMember> AdditionalMemberList { get; set; }
    }
}
