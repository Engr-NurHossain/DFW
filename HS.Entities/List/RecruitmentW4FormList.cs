using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RecruitmentW4FormList", Namespace = "http://www.piistech.com//list")]	
	public class RecruitmentW4FormList : BaseCollection<RecruitmentW4Form>
	{
		#region Constructors
	    public RecruitmentW4FormList() : base() { }
        public RecruitmentW4FormList(RecruitmentW4Form[] list) : base(list) { }
        public RecruitmentW4FormList(List<RecruitmentW4Form> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

