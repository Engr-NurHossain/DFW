using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CommisionTypeList", Namespace = "http://www.piistech.com//list")]	
	public class CommisionTypeList : BaseCollection<CommisionType>
	{
		#region Constructors
	    public CommisionTypeList() : base() { }
        public CommisionTypeList(CommisionType[] list) : base(list) { }
        public CommisionTypeList(List<CommisionType> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

