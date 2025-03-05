using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class PaymentInfoCustomer 
	{
		public int PaymentCustomerId { get; set; }
        public string PaymentFor { get; set; }

		public string PaymentType { get; set; }
    }
}
