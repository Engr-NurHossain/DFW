using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerSystemNoDraftList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerSystemNoDraftList : BaseCollection<CustomerSystemNoDraft>
	{
		#region Constructors
	    public CustomerSystemNoDraftList() : base() { }
        public CustomerSystemNoDraftList(CustomerSystemNoDraft[] list) : base(list) { }
        public CustomerSystemNoDraftList(List<CustomerSystemNoDraft> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

