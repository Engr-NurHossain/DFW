using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EstimatorPDFFilterList", Namespace = "http://www.hims-tech.com//list")]	
	public class EstimatorPDFFilterList : BaseCollection<EstimatorPDFFilter>
	{
		#region Constructors
	    public EstimatorPDFFilterList() : base() { }
        public EstimatorPDFFilterList(EstimatorPDFFilter[] list) : base(list) { }
        public EstimatorPDFFilterList(List<EstimatorPDFFilter> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
