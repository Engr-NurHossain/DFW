using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ContractAgreementTemplateList", Namespace = "http://www.hims-tech.com//list")]	
	public class ContractAgreementTemplateList : BaseCollection<ContractAgreementTemplate>
	{
		#region Constructors
	    public ContractAgreementTemplateList() : base() { }
        public ContractAgreementTemplateList(ContractAgreementTemplate[] list) : base(list) { }
        public ContractAgreementTemplateList(List<ContractAgreementTemplate> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
