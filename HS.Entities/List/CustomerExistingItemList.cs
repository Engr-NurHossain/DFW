using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerExistingItemList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerExistingItemList : BaseCollection<CustomerExistingItem>
	{
		#region Constructors
	    public CustomerExistingItemList() : base() { }
        public CustomerExistingItemList(CustomerExistingItem[] list) : base(list) { }
        public CustomerExistingItemList(List<CustomerExistingItem> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

