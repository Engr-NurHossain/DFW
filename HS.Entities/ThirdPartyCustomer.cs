using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class ThirdPartyCustomer 
	{
        public List<ThirdPartyCustomer> ThirdPartyCustomerList { get; set; }

        public string CustomerLastname { get; set; }

        public int TempOfcontact { get; set; }
        public int CustomerInt { get; set; }
	}
}
