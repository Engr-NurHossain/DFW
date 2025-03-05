using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class CustomerAgreementTemplate 
	{
		public int leadid { get; set; }
        public Guid cusId { get; set; }
        public int estId { get; set; }
        public string invoiceid { get; set; }
        public string CancellationDate { get; set; }
        public float? RemainingBalance { get; set; }
        public bool IsCustomerSignRequired { get; set; }

    }
}
