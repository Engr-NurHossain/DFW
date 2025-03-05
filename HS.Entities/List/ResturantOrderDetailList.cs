using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ResturantOrderDetailList", Namespace = "http://www.hims-tech.com//list")]	
	public class ResturantOrderDetailList : BaseCollection<ResturantOrderDetail>
	{
		#region Constructors
	    public ResturantOrderDetailList() : base() { }
        public ResturantOrderDetailList(ResturantOrderDetail[] list) : base(list) { }
        public ResturantOrderDetailList(List<ResturantOrderDetail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
