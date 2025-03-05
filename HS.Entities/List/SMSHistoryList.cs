using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SMSHistoryList", Namespace = "http://www.piistech.com//list")]	
	public class SMSHistoryList : BaseCollection<SMSHistory>
	{
		#region Constructors
	    public SMSHistoryList() : base() { }
        public SMSHistoryList(SMSHistory[] list) : base(list) { }
        public SMSHistoryList(List<SMSHistory> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

