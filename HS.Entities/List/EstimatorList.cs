using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EstimatorList", Namespace = "http://www.hims-tech.com//list")]	
	public class EstimatorList : BaseCollection<Estimator>
	{
		#region Constructors
	    public EstimatorList() : base() { }
        public EstimatorList(Estimator[] list) : base(list) { }
        public EstimatorList(List<Estimator> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
