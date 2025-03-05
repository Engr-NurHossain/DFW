using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "UserActivityCustomerList", Namespace = "http://www.piistech.com//list")]	
	public class UserActivityCustomerList : BaseCollection<UserActivityCustomer>
	{
		#region Constructors
	    public UserActivityCustomerList() : base() { }
        public UserActivityCustomerList(UserActivityCustomer[] list) : base(list) { }
        public UserActivityCustomerList(List<UserActivityCustomer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

