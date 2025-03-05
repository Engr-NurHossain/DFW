using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "OverrideRangeList", Namespace = "http://www.piistech.com//list")]	
	public class OverrideRangeList : BaseCollection<OverrideRange>
	{
		#region Constructors
	    public OverrideRangeList() : base() { }
        public OverrideRangeList(OverrideRange[] list) : base(list) { }
        public OverrideRangeList(List<OverrideRange> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

