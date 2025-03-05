using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerVaultList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerVaultList : BaseCollection<CustomerVault>
	{
		#region Constructors
	    public CustomerVaultList() : base() { }
        public CustomerVaultList(CustomerVault[] list) : base(list) { }
        public CustomerVaultList(List<CustomerVault> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

