using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollMonthlyProductionBonusList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollMonthlyProductionBonusList : BaseCollection<PayrollMonthlyProductionBonus>
	{
		#region Constructors
	    public PayrollMonthlyProductionBonusList() : base() { }
        public PayrollMonthlyProductionBonusList(PayrollMonthlyProductionBonus[] list) : base(list) { }
        public PayrollMonthlyProductionBonusList(List<PayrollMonthlyProductionBonus> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
