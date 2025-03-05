using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EstimatorDetailList", Namespace = "http://www.hims-tech.com//list")]	
	public class EstimatorDetailList : BaseCollection<EstimatorDetail>
	{
		#region Constructors
	    public EstimatorDetailList() : base() { }
        public EstimatorDetailList(EstimatorDetail[] list) : base(list) { }
        public EstimatorDetailList(List<EstimatorDetail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
