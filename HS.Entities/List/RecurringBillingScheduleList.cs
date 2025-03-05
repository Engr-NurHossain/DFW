using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RecurringBillingScheduleList", Namespace = "http://www.hims-tech.com//list")]	
	public class RecurringBillingScheduleList : BaseCollection<RecurringBillingSchedule>
	{
		#region Constructors
	    public RecurringBillingScheduleList() : base() { }
        public RecurringBillingScheduleList(RecurringBillingSchedule[] list) : base(list) { }
        public RecurringBillingScheduleList(List<RecurringBillingSchedule> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
