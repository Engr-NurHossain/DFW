using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BrinksCustomerList", Namespace = "http://www.hims-tech.com//list")]	
	public class BrinksCustomerList : BaseCollection<BrinksCustomer>
	{
		#region Constructors
	    public BrinksCustomerList() : base() { }
        public BrinksCustomerList(BrinksCustomer[] list) : base(list) { }
        public BrinksCustomerList(List<BrinksCustomer> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
