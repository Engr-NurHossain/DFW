using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerAdditionalContactList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerAdditionalContactList : BaseCollection<CustomerAdditionalContact>
	{
		#region Constructors
	    public CustomerAdditionalContactList() : base() { }
        public CustomerAdditionalContactList(CustomerAdditionalContact[] list) : base(list) { }
        public CustomerAdditionalContactList(List<CustomerAdditionalContact> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

