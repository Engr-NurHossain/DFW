using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "FundList", Namespace = "http://www.piistech.com//list")]	
	public class FundList : BaseCollection<Fund>
	{
		#region Constructors
	    public FundList() : base() { }
        public FundList(Fund[] list) : base(list) { }
        public FundList(List<Fund> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

