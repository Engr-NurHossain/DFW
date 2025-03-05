using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class PowerPayFinance
    {
        public string DefiSourceSystemId { get; set; }
        public int DefiDealerId { get; set; }
        public string ApplicationAffiliate { get; set; }

    }
}
