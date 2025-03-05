using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "NotificationUserList", Namespace = "http://www.piistech.com//list")]	
	public class NotificationUserList : BaseCollection<NotificationUser>
	{
		#region Constructors
	    public NotificationUserList() : base() { }
        public NotificationUserList(NotificationUser[] list) : base(list) { }
        public NotificationUserList(List<NotificationUser> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

