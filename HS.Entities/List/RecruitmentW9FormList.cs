using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RecruitmentW9FormList", Namespace = "http://www.piistech.com//list")]	
	public class RecruitmentW9FormList : BaseCollection<RecruitmentW9Form>
	{
		#region Constructors
	    public RecruitmentW9FormList() : base() { }
        public RecruitmentW9FormList(RecruitmentW9Form[] list) : base(list) { }
        public RecruitmentW9FormList(List<RecruitmentW9Form> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

