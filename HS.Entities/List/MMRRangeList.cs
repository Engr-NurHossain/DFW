using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MMRRangeList", Namespace = "http://www.piistech.com//list")]	
	public class MMRRangeList : BaseCollection<MMRRange>
	{
		#region Constructors
	    public MMRRangeList() : base() { }
        public MMRRangeList(MMRRange[] list) : base(list) { }
        public MMRRangeList(List<MMRRange> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

