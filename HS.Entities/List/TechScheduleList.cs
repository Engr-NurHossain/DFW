using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TechScheduleList", Namespace = "http://www.piistech.com//list")]	
	public class TechScheduleList : BaseCollection<TechSchedule>
	{
		#region Constructors
	    public TechScheduleList() : base() { }
        public TechScheduleList(TechSchedule[] list) : base(list) { }
        public TechScheduleList(List<TechSchedule> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

