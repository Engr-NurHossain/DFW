using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ResturantOrderInstructionToppingList", Namespace = "http://www.hims-tech.com//list")]	
	public class ResturantOrderInstructionToppingList : BaseCollection<ResturantOrderInstructionTopping>
	{
		#region Constructors
	    public ResturantOrderInstructionToppingList() : base() { }
        public ResturantOrderInstructionToppingList(ResturantOrderInstructionTopping[] list) : base(list) { }
        public ResturantOrderInstructionToppingList(List<ResturantOrderInstructionTopping> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
