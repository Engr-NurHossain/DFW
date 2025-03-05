using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class CustomerSystemNo 
	{
		public string Prefix { get; set; }
        public int StartingNumber { get; set; }
        public int TotalNumber { get; set; }
        public string CustomerName { get; set; }
    }
}
