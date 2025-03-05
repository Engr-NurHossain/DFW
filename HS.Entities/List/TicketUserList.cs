using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TicketUserList", Namespace = "http://www.piistech.com//list")]	
	public class TicketUserList : BaseCollection<TicketUser>
	{
		#region Constructors
	    public TicketUserList() : base() { }
        public TicketUserList(TicketUser[] list) : base(list) { }
        public TicketUserList(List<TicketUser> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

