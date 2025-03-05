using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerViewList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerViewList : BaseCollection<CustomerView>
	{
		#region Constructors
	    public CustomerViewList() : base() { }
        public CustomerViewList(CustomerView[] list) : base(list) { }
        public CustomerViewList(List<CustomerView> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

