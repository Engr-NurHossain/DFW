using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerCompanyList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerCompanyList : BaseCollection<CustomerCompany>
	{
		#region Constructors
	    public CustomerCompanyList() : base() { }
        public CustomerCompanyList(CustomerCompany[] list) : base(list) { }
        public CustomerCompanyList(List<CustomerCompany> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

