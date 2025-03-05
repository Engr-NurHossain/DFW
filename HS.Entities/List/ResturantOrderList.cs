using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ResturantOrderList", Namespace = "http://www.hims-tech.com//list")]	
	public class ResturantOrderList : BaseCollection<ResturantOrder>
	{
		#region Constructors
	    public ResturantOrderList() : base() { }
        public ResturantOrderList(ResturantOrder[] list) : base(list) { }
        public ResturantOrderList(List<ResturantOrder> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
