using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TicketReplyList", Namespace = "http://www.piistech.com//list")]	
	public class TicketReplyList : BaseCollection<TicketReply>
	{
		#region Constructors
	    public TicketReplyList() : base() { }
        public TicketReplyList(TicketReply[] list) : base(list) { }
        public TicketReplyList(List<TicketReply> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

