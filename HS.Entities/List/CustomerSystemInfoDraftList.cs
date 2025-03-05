using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerSystemInfoDraftList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerSystemInfoDraftList : BaseCollection<CustomerSystemInfoDraft>
	{
		#region Constructors
	    public CustomerSystemInfoDraftList() : base() { }
        public CustomerSystemInfoDraftList(CustomerSystemInfoDraft[] list) : base(list) { }
        public CustomerSystemInfoDraftList(List<CustomerSystemInfoDraft> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

