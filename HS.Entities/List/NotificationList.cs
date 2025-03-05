using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "NotificationList", Namespace = "http://www.piistech.com//list")]	
	public class NotificationList : BaseCollection<Notification>
	{
		#region Constructors
	    public NotificationList() : base() { }
        public NotificationList(Notification[] list) : base(list) { }
        public NotificationList(List<Notification> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

