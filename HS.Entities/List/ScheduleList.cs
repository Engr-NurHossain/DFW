using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ScheduleList", Namespace = "http://www.piistech.com//list")]	
	public class ScheduleList : BaseCollection<Schedule>
	{
		#region Constructors
	    public ScheduleList() : base() { }
        public ScheduleList(Schedule[] list) : base(list) { }
        public ScheduleList(List<Schedule> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

