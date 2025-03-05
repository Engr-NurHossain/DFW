using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class QAFinance 
	{
		
	}
    public class CreateQAFinanceModel
    {
        public Customer customer { get; set; }
        public CustomerExtended customerExtend { get; set; }
        public QAFinance qAFinance { get; set; }
        public List<string> NoteList { get; set; }
        public string UserName { get; set; }
        public double MonthlyRate { get; set; }
        public double FinancedAmount { get; set; }
        //public double ActivationFee { get; set; }
        //public double EquipmentFee { get; set; }
    }
}
