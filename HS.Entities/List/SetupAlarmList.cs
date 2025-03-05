using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SetupAlarmList", Namespace = "http://www.piistech.com//list")]	
	public class SetupAlarmList : BaseCollection<SetupAlarm>
	{
		#region Constructors
	    public SetupAlarmList() : base() { }
        public SetupAlarmList(SetupAlarm[] list) : base(list) { }
        public SetupAlarmList(List<SetupAlarm> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

