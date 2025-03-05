using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerAgreementTemplateList", Namespace = "http://www.hims-tech.com//list")]	
	public class CustomerAgreementTemplateList : BaseCollection<CustomerAgreementTemplate>
	{
		#region Constructors
	    public CustomerAgreementTemplateList() : base() { }
        public CustomerAgreementTemplateList(CustomerAgreementTemplate[] list) : base(list) { }
        public CustomerAgreementTemplateList(List<CustomerAgreementTemplate> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
