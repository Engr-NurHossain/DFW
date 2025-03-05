using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerSpouseList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerSpouseList : BaseCollection<CustomerSpouse>
	{
		#region Constructors
	    public CustomerSpouseList() : base() { }
        public CustomerSpouseList(CustomerSpouse[] list) : base(list) { }
        public CustomerSpouseList(List<CustomerSpouse> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

