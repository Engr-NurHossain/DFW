using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerCheckLogList", Namespace = "http://www.hims-tech.com//list")]	
	public class CustomerCheckLogList : BaseCollection<CustomerCheckLog>
	{
		#region Constructors
	    public CustomerCheckLogList() : base() { }
        public CustomerCheckLogList(CustomerCheckLog[] list) : base(list) { }
        public CustomerCheckLogList(List<CustomerCheckLog> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
