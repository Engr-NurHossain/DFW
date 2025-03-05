using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class QA2Script 
	{
		
	}
    public class CreateQA2Model
    {
        public Customer customer { get; set; }
        public CustomerExtended cusextnd {get;set;}
        public QA2Script qa2Script { get; set; }
        public TicketUser TicketTech { get; set; }
        public List<string> NoteList { get; set; }
        public string UserName { get; set; }
        public double MonthlyRate { get; set; }
        public double ActivationFee { get; set; }
        public double EquipmentFee { get; set; }
    }
}
