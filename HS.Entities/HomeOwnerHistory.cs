using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class HomeOwnerHistory 
	{
        public string CustomerName { get; set; }
        public string RequestedByVal { get; set; }
    }
}
