using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EstimateImageList", Namespace = "http://www.hims-tech.com//list")]	
	public class EstimateImageList : BaseCollection<EstimateImage>
	{
		#region Constructors
	    public EstimateImageList() : base() { }
        public EstimateImageList(EstimateImage[] list) : base(list) { }
        public EstimateImageList(List<EstimateImage> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
