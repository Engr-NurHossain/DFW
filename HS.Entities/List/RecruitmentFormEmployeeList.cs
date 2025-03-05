using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RecruitmentFormEmployeeList", Namespace = "http://www.piistech.com//list")]	
	public class RecruitmentFormEmployeeList : BaseCollection<RecruitmentFormEmployee>
	{
		#region Constructors
	    public RecruitmentFormEmployeeList() : base() { }
        public RecruitmentFormEmployeeList(RecruitmentFormEmployee[] list) : base(list) { }
        public RecruitmentFormEmployeeList(List<RecruitmentFormEmployee> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

