using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerAgreementList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerAgreementList : BaseCollection<CustomerAgreement>
	{
		#region Constructors
	    public CustomerAgreementList() : base() { }
        public CustomerAgreementList(CustomerAgreement[] list) : base(list) { }
        public CustomerAgreementList(List<CustomerAgreement> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

