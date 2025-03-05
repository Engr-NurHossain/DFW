using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class UserContact 
	{
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserBusinessName { get; set; }
        public string OpportunityName { get; set; }
        public int UserIntId { get; set; }
    }
}
