using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestToppingList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestToppingList : BaseCollection<RestTopping>
	{
		#region Constructors
	    public RestToppingList() : base() { }
        public RestToppingList(RestTopping[] list) : base(list) { }
        public RestToppingList(List<RestTopping> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
