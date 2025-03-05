using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TicketStatusImageSettingList", Namespace = "http://www.piistech.com//list")]	
	public class TicketStatusImageSettingList : BaseCollection<TicketStatusImageSetting>
	{
		#region Constructors
	    public TicketStatusImageSettingList() : base() { }
        public TicketStatusImageSettingList(TicketStatusImageSetting[] list) : base(list) { }
        public TicketStatusImageSettingList(List<TicketStatusImageSetting> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

