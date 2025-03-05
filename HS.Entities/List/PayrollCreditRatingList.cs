using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollCreditRatingList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollCreditRatingList : BaseCollection<PayrollCreditRating>
	{
		#region Constructors
	    public PayrollCreditRatingList() : base() { }
        public PayrollCreditRatingList(PayrollCreditRating[] list) : base(list) { }
        public PayrollCreditRatingList(List<PayrollCreditRating> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
