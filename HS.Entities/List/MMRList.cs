using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MMRList", Namespace = "http://www.piistech.com//list")]	
	public class MMRList : BaseCollection<MMR>
	{
		#region Constructors
	    public MMRList() : base() { }
        public MMRList(MMR[] list) : base(list) { }
        public MMRList(List<MMR> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

