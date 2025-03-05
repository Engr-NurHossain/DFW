using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RecruitmentFormList", Namespace = "http://www.piistech.com//list")]	
	public class RecruitmentFormList : BaseCollection<RecruitmentForm>
	{
		#region Constructors
	    public RecruitmentFormList() : base() { }
        public RecruitmentFormList(RecruitmentForm[] list) : base(list) { }
        public RecruitmentFormList(List<RecruitmentForm> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

