using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TicketFileList", Namespace = "http://www.piistech.com//list")]	
	public class TicketFileList : BaseCollection<TicketFile>
	{
		#region Constructors
	    public TicketFileList() : base() { }
        public TicketFileList(TicketFile[] list) : base(list) { }
        public TicketFileList(List<TicketFile> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

