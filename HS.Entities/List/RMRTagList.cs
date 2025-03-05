using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RMRTagList", Namespace = "http://www.piistech.com//list")]	
	public class RMRTagList : BaseCollection<RMRTag>
	{
		#region Constructors
	    public RMRTagList() : base() { }
        public RMRTagList(RMRTag[] list) : base(list) { }
        public RMRTagList(List<RMRTag> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

