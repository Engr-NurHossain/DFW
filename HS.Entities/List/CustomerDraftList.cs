using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerDraftList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerDraftList : BaseCollection<CustomerDraft>
	{
		#region Constructors
	    public CustomerDraftList() : base() { }
        public CustomerDraftList(CustomerDraft[] list) : base(list) { }
        public CustomerDraftList(List<CustomerDraft> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

