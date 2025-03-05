using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CommisionRangeList", Namespace = "http://www.piistech.com//list")]	
	public class CommisionRangeList : BaseCollection<CommisionRange>
	{
		#region Constructors
	    public CommisionRangeList() : base() { }
        public CommisionRangeList(CommisionRange[] list) : base(list) { }
        public CommisionRangeList(List<CommisionRange> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

