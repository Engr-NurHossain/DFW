using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CompanyBranchList", Namespace = "http://www.piistech.com//list")]	
	public class CompanyBranchList : BaseCollection<CompanyBranch>
	{
		#region Constructors
	    public CompanyBranchList() : base() { }
        public CompanyBranchList(CompanyBranch[] list) : base(list) { }
        public CompanyBranchList(List<CompanyBranch> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

