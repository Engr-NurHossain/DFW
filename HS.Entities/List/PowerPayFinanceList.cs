using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PowerPayFinanceList", Namespace = "http://www.hims-tech.com//list")]	
	public class PowerPayFinanceList : BaseCollection<PowerPayFinance>
	{
		#region Constructors
	    public PowerPayFinanceList() : base() { }
        public PowerPayFinanceList(PowerPayFinance[] list) : base(list) { }
        public PowerPayFinanceList(List<PowerPayFinance> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
