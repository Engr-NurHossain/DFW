using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "OverrideList", Namespace = "http://www.piistech.com//list")]	
	public class OverrideList : BaseCollection<Override>
	{
		#region Constructors
	    public OverrideList() : base() { }
        public OverrideList(Override[] list) : base(list) { }
        public OverrideList(List<Override> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

