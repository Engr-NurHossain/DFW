using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ResturantReviewList", Namespace = "http://www.hims-tech.com//list")]	
	public class ResturantReviewList : BaseCollection<ResturantReview>
	{
		#region Constructors
	    public ResturantReviewList() : base() { }
        public ResturantReviewList(ResturantReview[] list) : base(list) { }
        public ResturantReviewList(List<ResturantReview> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
