using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmployeeReviewList", Namespace = "http://www.piistech.com//list")]	
	public class EmployeeReviewList : BaseCollection<EmployeeReview>
	{
		#region Constructors
	    public EmployeeReviewList() : base() { }
        public EmployeeReviewList(EmployeeReview[] list) : base(list) { }
        public EmployeeReviewList(List<EmployeeReview> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

