using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CommisionList", Namespace = "http://www.piistech.com//list")]	
	public class CommisionList : BaseCollection<Commision>
	{
		#region Constructors
	    public CommisionList() : base() { }
        public CommisionList(Commision[] list) : base(list) { }
        public CommisionList(List<Commision> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

