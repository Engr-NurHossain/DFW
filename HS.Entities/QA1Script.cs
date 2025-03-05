using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class QA1Script 
	{
		
	}
    public class CreateQA1Model
    {
        public Customer customer { get; set; }
        public Ticket ticket { get; set; }
        public QA1Script qa1Script { get; set; }
        public List<CustomerAppointmentEquipment> eqpList { get; set; }
        public List<string> NoteList { get; set; }
        public string UserName { get; set; }
        public string PaymentType { get; set; }
        public double MonthlyRate { get; set; }
        public double ActivationFee { get; set; }
        public double EquipmentFee { get; set; }
        public double DiscountAmount { get; set; }
    }
}
