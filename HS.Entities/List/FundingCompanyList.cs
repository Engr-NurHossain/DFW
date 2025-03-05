using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "FundingCompanyList", Namespace = "http://www.piistech.com//list")]	
	public class FundingCompanyList : BaseCollection<FundingCompany>
	{
		#region Constructors
	    public FundingCompanyList() : base() { }
        public FundingCompanyList(FundingCompany[] list) : base(list) { }
        public FundingCompanyList(List<FundingCompany> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

