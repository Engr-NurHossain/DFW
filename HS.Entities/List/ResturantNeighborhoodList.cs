using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ResturantNeighborhoodList", Namespace = "http://www.hims-tech.com//list")]	
	public class ResturantNeighborhoodList : BaseCollection<ResturantNeighborhood>
	{
		#region Constructors
	    public ResturantNeighborhoodList() : base() { }
        public ResturantNeighborhoodList(ResturantNeighborhood[] list) : base(list) { }
        public ResturantNeighborhoodList(List<ResturantNeighborhood> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
