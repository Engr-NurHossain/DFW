using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "LeadCorrespondenceList", Namespace = "http://www.piistech.com//list")]	
	public class LeadCorrespondenceList : BaseCollection<LeadCorrespondence>
	{
		#region Constructors
	    public LeadCorrespondenceList() : base() { }
        public LeadCorrespondenceList(LeadCorrespondence[] list) : base(list) { }
        public LeadCorrespondenceList(List<LeadCorrespondence> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

