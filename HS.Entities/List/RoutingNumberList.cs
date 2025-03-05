using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RoutingNumberList", Namespace = "http://www.piistech.com//list")]	
	public class RoutingNumberList : BaseCollection<RoutingNumber>
	{
		#region Constructors
	    public RoutingNumberList() : base() { }
        public RoutingNumberList(RoutingNumber[] list) : base(list) { }
        public RoutingNumberList(List<RoutingNumber> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
