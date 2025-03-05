using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "OpportunityList", Namespace = "http://www.piistech.com//list")]	
	public class OpportunityList : BaseCollection<Opportunity>
	{
		#region Constructors
	    public OpportunityList() : base() { }
        public OpportunityList(Opportunity[] list) : base(list) { }
        public OpportunityList(List<Opportunity> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

