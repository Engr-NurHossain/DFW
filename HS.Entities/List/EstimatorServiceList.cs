using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EstimatorServiceList", Namespace = "http://www.piistech.com//list")]	
	public class EstimatorServiceList : BaseCollection<EstimatorService>
	{
		#region Constructors
	    public EstimatorServiceList() : base() { }
        public EstimatorServiceList(EstimatorService[] list) : base(list) { }
        public EstimatorServiceList(List<EstimatorService> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
