using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TimeClockList", Namespace = "http://www.piistech.com//list")]	
	public class TimeClockList : BaseCollection<TimeClock>
	{
		#region Constructors
	    public TimeClockList() : base() { }
        public TimeClockList(TimeClock[] list) : base(list) { }
        public TimeClockList(List<TimeClock> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

