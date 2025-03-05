using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RecruitmentDocFormList", Namespace = "http://www.piistech.com//list")]	
	public class RecruitmentDocFormList : BaseCollection<RecruitmentDocForm>
	{
		#region Constructors
	    public RecruitmentDocFormList() : base() { }
        public RecruitmentDocFormList(RecruitmentDocForm[] list) : base(list) { }
        public RecruitmentDocFormList(List<RecruitmentDocForm> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

