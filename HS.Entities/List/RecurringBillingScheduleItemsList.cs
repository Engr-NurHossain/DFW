using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RecurringBillingScheduleItemsList", Namespace = "http://www.hims-tech.com//list")]	
	public class RecurringBillingScheduleItemsList : BaseCollection<RecurringBillingScheduleItems>
	{
		#region Constructors
	    public RecurringBillingScheduleItemsList() : base() { }
        public RecurringBillingScheduleItemsList(RecurringBillingScheduleItems[] list) : base(list) { }
        public RecurringBillingScheduleItemsList(List<RecurringBillingScheduleItems> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
