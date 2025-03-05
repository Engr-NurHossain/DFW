using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TicketNotificationEmailList", Namespace = "http://www.hims-tech.com//list")]	
	public class TicketNotificationEmailList : BaseCollection<TicketNotificationEmail>
	{
		#region Constructors
	    public TicketNotificationEmailList() : base() { }
        public TicketNotificationEmailList(TicketNotificationEmail[] list) : base(list) { }
        public TicketNotificationEmailList(List<TicketNotificationEmail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
