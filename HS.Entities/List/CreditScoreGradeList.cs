using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CreditScoreGradeList", Namespace = "http://www.piistech.com//list")]	
	public class CreditScoreGradeList : BaseCollection<CreditScoreGrade>
	{
		#region Constructors
	    public CreditScoreGradeList() : base() { }
        public CreditScoreGradeList(CreditScoreGrade[] list) : base(list) { }
        public CreditScoreGradeList(List<CreditScoreGrade> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

