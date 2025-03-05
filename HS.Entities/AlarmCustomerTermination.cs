using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class AlarmCustomerTermination 
	{
        public string TerminationBy { get; set; }
    }
    public partial class AlarmCustomerTerminationViewModel
    {
        public int Id { get; set; }
        public int AlarmId { get; set; }
        public string TerminationReason { get; set; }
        public string TerminationBy { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime TerminationDate { get; set; }
    }
}
