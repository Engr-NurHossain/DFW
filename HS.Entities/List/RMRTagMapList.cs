using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RMRTagMapList", Namespace = "http://www.piistech.com//list")]	
	public class RMRTagMapList : BaseCollection<RMRTagMap>
	{
		#region Constructors
	    public RMRTagMapList() : base() { }
        public RMRTagMapList(RMRTagMap[] list) : base(list) { }
        public RMRTagMapList(List<RMRTagMap> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

