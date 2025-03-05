using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CommisionSessionList", Namespace = "http://www.piistech.com//list")]	
	public class CommisionSessionList : BaseCollection<CommisionSession>
	{
		#region Constructors
	    public CommisionSessionList() : base() { }
        public CommisionSessionList(CommisionSession[] list) : base(list) { }
        public CommisionSessionList(List<CommisionSession> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

